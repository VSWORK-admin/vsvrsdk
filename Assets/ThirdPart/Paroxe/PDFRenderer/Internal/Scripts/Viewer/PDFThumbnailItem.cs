using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Paroxe.PdfRenderer.Internal.Viewer
{
    public class PDFThumbnailItem : UIBehaviour
    {
        [SerializeField]
        private AspectRatioFitter m_AspectRatioFitter;
        [SerializeField]
        private Image m_Highlighted;
        [SerializeField]
        private LayoutElement m_LayoutElement;
        [SerializeField]
        private Text m_PageIndexLabel;
        [SerializeField]
        private RawImage m_PageThumbnailRawImage;
        [SerializeField]
        private RectTransform m_RectTransform;

        public AspectRatioFitter AspectRatioFitter
        {
	        get { return m_AspectRatioFitter; }
        }

        public RawImage PageThumbnailRawImage
        {
	        get { return m_PageThumbnailRawImage; }
        }

        public Image Highlighted
        {
	        get { return m_Highlighted; }
        }

        public Text PageIndexLabel
        {
	        get { return m_PageIndexLabel; }
        }

        public void OnThumbnailClicked()
        {
            GetComponentInParent<PDFViewer>().GoToPage(int.Parse(m_PageIndexLabel.text) - 1);
        }

        private void LateUpdate()
        {
	        m_LayoutElement.preferredHeight = 180.0f * (m_RectTransform.sizeDelta.x / 320.0f);
        }
    }
}