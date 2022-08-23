namespace CSE.MRTK.Toolkit.DocumentViewer
{
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// Class to show the number of pages of the document and
    /// the current page number.
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    public class DocumentPageNumber : DocumentSubscriber
    {
        protected override void PageChanged()
        {
            if (Controller == null) { return; }

            TextMeshPro tmp = GetComponent<TextMeshPro>();
            tmp.text = (Controller.CurrentPage + 1) + "/" + Controller.PageCount;
        }
    }
}
