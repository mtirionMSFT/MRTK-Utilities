namespace CSE.MRTK.Toolkit.DocumentViewer
{
    using UnityEngine;

    /// <summary>
    /// Inherit from this abstract class if you're interested in
    /// changes in the page view of the <see cref="DocumentController"/>.
    /// This class automatically subscribes on enabling the game
    /// object or unsubscribes when disabled.
    /// To use, just implement the <see cref="PageChanged"/> method.
    /// </summary>
    public abstract class DocumentSubscriber : MonoBehaviour
    {
        private DocumentController _controller = null;
        public DocumentController Controller => _controller;

        protected virtual void OnEnable()
        {
            if (_controller == null)
            {
                _controller = GetComponentInParent<DocumentController>();
                if (_controller != null)
                {
                    //PageChanged();
                    _controller.OnPageChanged += PageChanged;
                }
            }
        }

        protected virtual void OnDisable()
        {
            if (_controller != null)
            {
                _controller.OnPageChanged -= PageChanged;
                _controller = null;
            }
        }

        protected abstract void PageChanged();
    }
}
