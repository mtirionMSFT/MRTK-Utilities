// TODO:
// If the Paroxe library is installed in your project
// uncomment the define below to activate the code to
// use that library. You can of course also clean up the
// code to remove the demo part.
// If you are using a different PDF render library, this
// is the class to put the implementation, which will 
// automatically be used by the DocumentController class.
//
// The Paroxy component can be purchased and imported from the Unity Asset Store.
// Website: http://paroxe.com/products/pdf-renderer/
// Store: https://assetstore.unity.com/packages/tools/gui/pdf-renderer-32815
//
//#define PAROXE_INSTALLED

namespace CSE.MRTK.Toolkit.DocumentViewer
{
    using System;
    using System.IO;
    using UnityEngine;

    public class PDFDocument : IDisposable
    {
        /// <summary>
        /// Gets the number of pages of the document.
        /// </summary>
        private int _pageCount;
        public int PageCount => _pageCount;

        /// <summary>
        /// Gets the indication whether the document is valid.
        /// </summary>
        private bool _isValid;
        public bool IsValid => _isValid;


        // Keep page texture reference so we can clean up on
        // Dispose or when new page is requested.
        private Texture2D _pageTexture;

#if PAROXE_INSTALLED
        // the internal Paroxe PDF document
        private Paroxe.PdfRenderer.PDFDocument _pdfDocument;
#endif

        // Used to enhance the resolution of page textures
        private int _textureResolutionRatio = 4;

        /// <summary>
        /// Initializes a new instance of a <see cref="PDFDocument"/> object.
        /// </summary>
        /// <param name="fileStream">File stream to load.</param>
        /// <exception cref="ArgumentNullException">File stream is null.</exception>
        public PDFDocument(Stream fileStream)
        {
            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

#if PAROXE_INSTALLED
            byte[] content;
            using (MemoryStream ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                content = ms.ToArray();
                _pdfDocument = new Paroxe.PdfRenderer.PDFDocument(content);
                _isValid = _pdfDocument.IsValid;
                _pageCount = _pdfDocument.GetPageCount();
            }
#else
            // FOR DEMO PURPOSES ONLY
            // We have 4 demo pages to show when Paroxe is not installed
            _isValid = true;
            _pageCount = 4;
#endif
        }

        /// <summary>
        /// Get the requested page number as <see cref="Texture2D"/> object.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <returns>A <see cref="Texture2D"/> object.</returns>
        /// <exception cref="ArgumentException">Requested page is out of range.</exception>
        public Texture2D RenderPageToTexture(int page)
        {
            if (page >= _pageCount)
            {
                throw new ArgumentException($"Requested page {page} is out of range. Max {_pageCount}.");
            }

            if (_pageTexture != null)
            {
                // clean up last page texture
                UnityEngine.Object.Destroy(_pageTexture);
            }

#if PAROXE_INSTALLED
            // Retrieve the page to render in memory.
            using (Paroxe.PdfRenderer.PDFPage pdfPage = _pdfDocument.GetPage(page))
            {
                // render the request page texture
                _pageTexture = _pdfDocument.Renderer.RenderPageToTexture(pdfPage,
                                        (int)pdfPage.GetPageSize().x * _textureResolutionRatio,
                                        (int)pdfPage.GetPageSize().y * _textureResolutionRatio);
            }
#else
            // FOR DEMO PURPOSES
            // If Paroxe is not installed, we have 4 image pages to render
            string imageFileName = $"PdfDemoPage{page}.png";
            Stream fileStream = File.OpenRead(Path.Combine(Application.streamingAssetsPath, imageFileName));
            byte[] content;
            using (MemoryStream ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                content = ms.ToArray();
                _pageTexture = new Texture2D(1, 1);
                _pageTexture.LoadImage(content);
            }
#endif
            return _pageTexture;
        }

        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
#if PAROXE_INSTALLED
            if (_pdfDocument != null)
            {
                _pdfDocument.Dispose();
            }
#endif

            if (_pageTexture != null)
            {
                UnityEngine.Object.Destroy(_pageTexture);
            }
        }
    }
}
