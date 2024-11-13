using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Paroxe.PdfRenderer.Internal.Viewer
{
    public class PDFViewerLeftPanelScrollbar : UIBehaviour
    {
        private CanvasGroup m_CanvasGroup;
        private Scrollbar m_Scrollbar;
        private float m_Alpha = 1.0f;

        public float Alpha
        {
	        get { return m_Alpha; }
	        set
	        {
		        if (Math.Abs(m_Alpha - value) > float.Epsilon)
		        {
			        m_Alpha = value;

			        UpdateCanvasGroup();
                }
	        }
        }

        private void LateUpdate()
        {
	        UpdateCanvasGroup();
        }

        private void UpdateCanvasGroup()
        {
            if (m_Scrollbar == null)
				m_Scrollbar = GetComponent<Scrollbar>();

            if (m_CanvasGroup == null)
	            m_CanvasGroup = GetComponent<CanvasGroup>();

            if (m_Scrollbar.size >= 0.98f && Math.Abs(m_CanvasGroup.alpha) > 0.01f)
	        {
		        m_CanvasGroup.alpha = 0.0f;
	        }
	        else if (m_Scrollbar.size < 0.98f && Math.Abs(m_CanvasGroup.alpha - 1.0f * Alpha) > 0.01f)
	        {
		        m_CanvasGroup.alpha = 1.0f * Alpha;
	        }
        }
    }
}