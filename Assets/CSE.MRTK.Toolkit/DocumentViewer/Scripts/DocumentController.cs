namespace CSE.MRTK.Toolkit.DocumentViewer
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.MixedReality.Toolkit.UI;
    using UnityEngine;

    /// <summary>
    /// This is a helper class for working with the Paroxy component for rendering
    /// PDF pages in a HoloLens application.
    /// </summary>
    public class DocumentController : MonoBehaviour
    {
        [Tooltip("The file name of the document to load from StreamingAssets. This is for demo purposes only.")]
        [SerializeField]
        private string _pdfFileName = "Fact-Sheet_HoloLens2.pdf";

        /// <summary>
        /// Gets or sets landscape or portrait mode.
        /// </summary>
        [Tooltip("Display the page in Landscape (wide) mode, or Portrait (tall) mode.")]
        [SerializeField]
        private bool _isLandscape = true;
        public bool IsLandScape
        {
            get { return _isLandscape; }
            set { _isLandscape = value; }
        }

        [Tooltip("The expected page dimensions. Can be in any unit and gets converted into a ratio.")]
        [SerializeField]
        private Vector2 _pageRatio = new Vector2(8.5f, 11f);

        [Tooltip("Required. Gameobject containing a MeshRenderer. The first element in the Materials list is used.")]
        [SerializeField]
        private MeshRenderer _pageRenderer;
        public MeshRenderer PageRenderer => _pageRenderer;

        // progress indicator in the dialog for loading operations
        private ProgressIndicatorOrbsRotator _progressIndicator = null;

        // PDF document loaded in memory.
        private PDFDocument _pdfDocument;

        /// <summary>
        /// Gets the page number currently displayed. The page index is zero-based.
        /// </summary>
        private int currentPage = 0;
        public int CurrentPage => currentPage;

        /// <summary>
        /// Event to let others know something changed in the document page view.
        /// </summary>
        public delegate void PageChanged();
        public event PageChanged OnPageChanged;

        /// <summary>
        /// Gets the page count from the PDF Document
        /// </summary>
        public int PageCount
        {
            get { return _pdfDocument.PageCount; }
        }

        // Fixed property values required when setting textures dynamically
        // at runtime using MaterialPropertyBlocks.
        private static int _textureSTProperty = Shader.PropertyToID("_MainTex_ST");
        private static int _textureProperty = Shader.PropertyToID("_MainTex");

        /// <summary>
        /// Run op starting the DocumentViewer for the first time.
        ///
        /// TODO:
        /// This is just for demo purposes. Normally a manager will take care
        /// of selecting the appropriate document and then load a <see cref="DocumentManager"/>
        /// prefab for it.
        /// </summary>
        private async void Start()
        {
            Stream stream = File.OpenRead(Path.Combine(Application.streamingAssetsPath, _pdfFileName));
            await LoadPDFDocumentAsync(stream);
        }

        /// <summary>
        /// OnDestroy is called when the scene is unloaded or the application/game ends.
        /// </summary>
        void OnDestroy()
        {
            if (_pdfDocument != null)
            {
                _pdfDocument.Dispose();
            }
        }

        /// <summary>
        /// Loads the provided file into the <see cref="PDFDocument"/> of the Paroxe package.
        /// </summary>
        /// <param name="stream">File stream to load.</param>
        /// <param name="startPage">Start page. Zero-based.</param>
        public async Task LoadPDFDocumentAsync(Stream stream, int startPage = 0)
        {
            if (_progressIndicator == null)
            {
                _progressIndicator = GetComponentInChildren<ProgressIndicatorOrbsRotator>(true);
            }

            if (_progressIndicator == null)
            {
                Debug.LogError($"ERROR: No ProgressIndicatorOrbsRotator found. Cannot show progress.");
            }
            else
            {
                // start the progress indicator to show loading
                _progressIndicator.gameObject.SetActive(true);
                await _progressIndicator.OpenAsync();
            }

            try
            {
                // we have to read the file into memory.
                // (or the component will do that itself from the give filepath)
                if (stream != null)
                {
                    _pdfDocument = new PDFDocument(stream);
                }

                if (_pdfDocument.IsValid)
                {
                    // initialize using this document
                    int pageCount = _pdfDocument.PageCount;
                    currentPage = startPage;
                    RenderPage(currentPage);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"DocMgr.Load ERROR: error loading the document.\n{ex}");
            }

            if (_progressIndicator != null)
            {
                // stop the progress indicator
                await _progressIndicator.CloseAsync();
                _progressIndicator.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Displays the first page of the document
        /// </summary>
        public void ShowFirstPage()
        {
            if (currentPage > 0)
            {
                currentPage = 0;
                RenderPage(currentPage);
            }
        }

        /// <summary>
        /// Displays the previous page of the document
        /// </summary>
        public void ShowPreviousPage()
        {
            if (currentPage > 0)
            {
                currentPage--;
                RenderPage(currentPage);
            }
        }

        /// <summary>
        /// Displays the next page of the document
        /// </summary>
        public void ShowNextPage()
        {
            if (currentPage < _pdfDocument.PageCount - 1)
            {
                currentPage++;
                RenderPage(currentPage);
            }
        }

        /// <summary>
        /// Displays the last page of the document
        /// </summary>
        public void ShowLastPage()
        {
            if (currentPage < _pdfDocument.PageCount - 1)
            {
                currentPage = _pdfDocument.PageCount - 1;
                RenderPage(currentPage);
            }
        }

        /// <summary>
        /// Alternates the aspect ratio of the document viewer between portrait and landscape mode.
        /// </summary>
        public void ToggleAspectRatio()
        {
            _isLandscape = !_isLandscape;
            RenderPage(currentPage);
        }

        /// <summary>
        /// Loads a specific document page by creating a Texture2D and assigning it
        /// as the main texture of an existing material in the MeshRenderer.
        /// </summary>
        /// <param name="pageNumber">The page number to display. Zero-based.</param>
        public void RenderPage(int pageNumber)
        {
            try
            {
                if (_pdfDocument.IsValid)
                {
                    if (pageNumber < _pdfDocument.PageCount)
                    {
                        // NOTE: Texture is cleaned up by _pdfDocument
                        // at the appropriate moment.
                        Texture2D tex = _pdfDocument.RenderPageToTexture(pageNumber);

                        // Texture filtering (aka texture smoothing) is the method used to determine
                        // the texture color for a texture mapped pixel, using the colors of nearby
                        // texels (pixels of the texture).
                        // Important for magnification and minification of textures.
                        tex.filterMode = FilterMode.Bilinear;

                        // Anisotropic filtering is a method of enhancing the image quality of textures
                        // on surfaces of computer graphics that are at oblique viewing angles.
                        // This degree refers to the maximum ratio of anisotropy supported by
                        // the filtering process.
                        tex.anisoLevel = 8;

                        // Assign the new page texture to the default material of the MeshRenderer.
                        // Using MaterialPropertyBlock instead of accessing the texture directly
                        // on the material is preferred since it accesses the current texture instance
                        // instead of creating a copy, which is more efficient in terms of performance
                        // and memory usage.
                        MaterialPropertyBlock block = new MaterialPropertyBlock();
                        Vector2 tiling = Vector2.one;
                        Vector2 offset = Vector2.zero;

                        // Ratio remains 1:1 when in portrait since we show the full page
                        if (_isLandscape)
                        {
                            float ratio = 2f - _pageRatio.y / _pageRatio.x;
                            tiling.y = ratio;
                            offset.y = 1f - ratio;
                        }

                        _pageRenderer.GetPropertyBlock(block);
                        block.SetTexture(_textureProperty, tex);
                        block.SetVector(_textureSTProperty, new Vector4(tiling.x, tiling.y, offset.x, offset.y));
                        _pageRenderer.SetPropertyBlock(block);

                        HandInteractionPanZoom pan = _pageRenderer.GetComponent<HandInteractionPanZoom>();
                        if (pan != null)
                        {
                            // reset scale and position so next page is scrolled to the top
                            pan.Reset();
                        }

                        // let others know page has changed
                        this.OnPageChanged?.Invoke();
                    }
                }
                else
                {
                    Debug.LogError($"Cannot display page {pageNumber} for invalid document.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error occurred displaying page {pageNumber} for document: {ex}");
            }
        }
    }
}
