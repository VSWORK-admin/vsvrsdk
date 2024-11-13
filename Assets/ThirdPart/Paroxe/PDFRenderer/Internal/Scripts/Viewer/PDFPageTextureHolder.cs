using Paroxe.PdfRenderer.WebGL;
using UnityEngine;
using UnityEngine.UI;

namespace Paroxe.PdfRenderer.Internal.Viewer
{
    public class PDFPageTextureHolder
    {
	    private int m_PageIndex;
	    private GameObject m_Page;
	    private PDFViewer m_Viewer;
#if UNITY_WEBGL
	    private bool m_RenderingStarted;
	    private bool m_Visible;
	    private IPDFJS_Promise m_RenderingPromise;
#endif

        private Texture2D m_Texture;
        private RawImage m_RawImage;

        public int PageIndex
        {
	        get { return m_PageIndex; }
	        set { m_PageIndex = value; }
        }

        public GameObject Page
        {
	        get { return m_Page; }
	        set { m_Page = value; }
        }

        public PDFViewer Viewer
        {
	        get { return m_Viewer; }
	        set { m_Viewer = value; }
        }

#if UNITY_WEBGL
	    public bool RenderingStarted
	    {
		    get { return m_RenderingStarted; }
		    set { m_RenderingStarted = value; }
	    }

	    public bool Visible
        {
		    get { return m_Visible; }
		    set { m_Visible = value; }
	    }

	    public IPDFJS_Promise RenderingPromise
        {
		    get { return m_RenderingPromise; }
		    set { m_RenderingPromise = value; }
	    }
#endif

        public void RefreshTexture()
        {
            Texture = m_Texture;
        }

        public Texture2D Texture
        {
            get
            {
                return m_Texture;
            }
            set
            {
                m_Texture = value;

                if (m_RawImage == null)
                {
	                m_RawImage = m_Page.GetComponent<RawImage>();

                    if (m_RawImage == null)
	                    m_RawImage = m_Page.AddComponent<RawImage>();
                }
                
                if (value != null)
                {
	                m_RawImage.texture = value;
	                m_RawImage.uvRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
	                m_RawImage.color = Color.white;
                }
                else
                {
	                m_RawImage.texture = null;
#if UNITY_WEBGL
					m_RawImage.color = Color.white;
#else
	                m_RawImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
#endif
                }
            }
        }
    }
}