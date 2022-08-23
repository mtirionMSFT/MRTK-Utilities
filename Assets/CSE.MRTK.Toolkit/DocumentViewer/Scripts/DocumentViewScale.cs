namespace CSE.MRTK.Toolkit.DocumentViewer
{
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.MixedReality.Toolkit.UI;
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// Displays the scale value of the HandInteractionPanZoom in a 100% format.
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    public class DocumentViewScale : DocumentSubscriber
    {
        private HandInteractionPanZoom _pan = null;
        private Coroutine _panning = null;

        private float _scale = 1f;
        private Vector2 _position = Vector2.zero;
        private TextMeshPro tmp = null;

        protected override void OnEnable()
        {
            tmp = GetComponent<TextMeshPro>();
            base.OnEnable();

            if (Controller != null && Controller.PageRenderer != null)
            {
                _pan = Controller.PageRenderer.GetComponent<HandInteractionPanZoom>();
                if (_pan != null)
                {
                    _pan.PanStarted.AddListener(PanStarted);
                    _pan.PanStopped.AddListener(PanStopped);
                }
            }
        }

        protected override void OnDisable()
        {
            if (_pan != null)
            {
                _pan.PanStarted.RemoveListener(PanStarted);
                _pan.PanStopped.RemoveListener(PanStopped);
                _pan = null;
            }

            base.OnDisable();
        }

        protected override void PageChanged()
        {
            UpdateScale();
        }

        public void UpdateScale()
        {
            if (_pan != null)
            {
                DisplayScale();
            }
        }

        private void PanStarted(HandPanEventData arg0)
        {
            if (_panning != null)
            {
                StopCoroutine(_panning);
                _panning = null;
            }

            _panning = StartCoroutine(PanUpdate());
        }

        private void PanStopped(HandPanEventData arg0)
        {
            if (_panning != null)
            {
                StopCoroutine(_panning);
                _panning = null;
            }

            DisplayScale();
        }

        private IEnumerator PanUpdate()
        {
            while (true)
            {
                yield return null;
                DisplayScale();
            }
        }

        protected virtual void DisplayScale()
        {
            _scale = 1f;
            _position = Vector2.zero;

            // HandInteractionPanZoom does not update currentScale
            // value when reset, grab from UVs instead
            MeshFilter filter = _pan.GetComponent<MeshFilter>();
            Mesh mesh = filter.mesh;
            List<Vector2> uvs = new List<Vector2>();
            mesh.GetUVs(0, uvs);

            float deltaXEven = 0f;
            float deltaY = 0f;
            float deltaX = 0;
            for (int i = 0; i < uvs.Count; i++)
            {
                if (i % 2 == 0)
                {
                    deltaXEven += uvs[i].x;
                }

                deltaY += uvs[i].y;
                deltaX += uvs[i].y;
            }

            if (deltaXEven < 0f)
            {
                _scale = deltaXEven + 1f;
            }
            else
            {
                _scale = 1f / Mathf.Max(1f - deltaXEven, 0.01f);
            }

            _position = new Vector2(2f - deltaX, 2f - deltaY);

            tmp.text = Mathf.RoundToInt(_scale * 100) + "%";
        }
    }
}
