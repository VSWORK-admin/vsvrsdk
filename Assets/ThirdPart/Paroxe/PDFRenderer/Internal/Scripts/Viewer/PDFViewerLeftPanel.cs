using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Paroxe.PdfRenderer.Internal.Viewer
{
    public class PDFViewerLeftPanel : UIBehaviour
    {
        [SerializeField]
        private RectTransform m_Bookmarks;
        [SerializeField]
        private Image m_BookmarksTab;
        [SerializeField]
        private Text m_BookmarksTabTitle;
        [SerializeField]
        private Sprite m_ClosedTabSprite;
        [SerializeField]
        private Sprite m_CloseSprite;
        [SerializeField]
        private float m_MaxWidth = 500.0f;
        [SerializeField]
        private float m_MinWidth = 250.0f;
        [SerializeField]
        private Sprite m_OpenedTabSprite;
        [SerializeField]
        private Sprite m_OpenSprite;
        [SerializeField]
        private Texture2D m_ResizeCursor;
        [SerializeField]
        private Image m_SideBarImage;
        [SerializeField]
        private RectTransform m_Thumbnails;
        [SerializeField]
        private Image m_ThumbnailsTab;
        [SerializeField]
        private Text m_ThumbnailsTabTitle;
        [SerializeField]
        private PDFThumbnailsViewer m_ThumbnailsViewer;

        private float m_LastPanelWidth;
        private bool m_Opened = true;
        private bool m_Drag;
        private CanvasGroup m_BookmarksCanvasGroup;
        private CanvasGroup m_ThumbnailsCanvasGroup;
        private PDFViewer m_Viewer;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        private bool m_PointerIn;
#endif

        public RectTransform Bookmarks { get { return m_Bookmarks; } }
        public Image BookmarksTab { get { return m_BookmarksTab; } }
        public PDFThumbnailsViewer ThumbnailsViewer { get { return m_ThumbnailsViewer; } }
        public RectTransform Thumbnails { get { return m_Thumbnails; } }
        public Image ThumbnailsTab { get { return m_ThumbnailsTab; } }

        private RectTransform RectTransform { get { return (RectTransform)transform; } }

        private PDFViewer Viewer
	    {
		    get
		    {
			    if (m_Viewer == null)
				    m_Viewer = GetComponentInParent<PDFViewer>();
			    return m_Viewer;
		    }
	    }

	    private CanvasGroup BookmarksCanvasGroup
	    {
		    get
		    {
			    if (m_BookmarksCanvasGroup == null)
				    m_BookmarksCanvasGroup = m_Bookmarks.GetComponent<CanvasGroup>();
			    return m_BookmarksCanvasGroup;
		    }
	    }

	    private CanvasGroup ThumbnailsCanvasGroup
	    {
		    get
		    {
			    if (m_ThumbnailsCanvasGroup == null)
				    m_ThumbnailsCanvasGroup = m_Thumbnails.GetComponent<CanvasGroup>();
			    return m_ThumbnailsCanvasGroup;
		    }
	    }

        public void OnBeginDrag()
        {
            if (!m_Opened)
                return;

            m_Drag = true;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            Cursor.SetCursor(m_ResizeCursor, new Vector2(16.0f, 16.0f), CursorMode.Auto);
#endif
        }

        public void OnDrag(BaseEventData eventData)
        {
            if (!m_Drag)
                return;

            PointerEventData pointerData = eventData as PointerEventData;
            if (pointerData == null)
                return;

            RectTransform.sizeDelta += new Vector2(pointerData.delta.x, 0.0f);
            RectTransform.sizeDelta = new Vector2(Mathf.Clamp(RectTransform.sizeDelta.x, m_MinWidth, m_MaxWidth), RectTransform.sizeDelta.y);
            m_LastPanelWidth = RectTransform.sizeDelta.x;

            UpdateViewport();
        }

        public void OnEndDrag()
        {
            if (!m_Drag || !m_Opened)
                return;

            m_Drag = false;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (!m_PointerIn)
	            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
#endif
        }

        public void OnPointerEnter()
        {
            if (!m_Opened)
	            return;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            Cursor.SetCursor(m_ResizeCursor, new Vector2(16.0f, 16.0f), CursorMode.Auto);
            m_PointerIn = true;
#endif
        }

        public void OnPointerExit()
        {
            if (!m_Opened)
	            return;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (!m_Drag)
	            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            m_PointerIn = false;
#endif
        }

        public void OnBookmarksTabClicked()
        {
	        m_BookmarksTab.sprite = m_OpenedTabSprite;
	        m_BookmarksTabTitle.color = Color.black;
	        m_ThumbnailsTab.sprite = m_ClosedTabSprite;
	        m_ThumbnailsTabTitle.color = new Color(0.50f, 0.50f, 0.50f);

	        BookmarksCanvasGroup.alpha = 1.0f;
	        BookmarksCanvasGroup.interactable = true;
	        BookmarksCanvasGroup.blocksRaycasts = true;

	        ThumbnailsCanvasGroup.alpha = 0.0f;
	        ThumbnailsCanvasGroup.interactable = false;
	        ThumbnailsCanvasGroup.blocksRaycasts = false;
        }

        public void OnThumbnailsTabClicked()
        {
            m_BookmarksTab.sprite = m_ClosedTabSprite;
            m_BookmarksTabTitle.color = new Color(0.50f, 0.50f, 0.50f);
            m_ThumbnailsTab.sprite = m_OpenedTabSprite;
            m_ThumbnailsTabTitle.color = Color.black;

            BookmarksCanvasGroup.alpha = 0.0f;
            BookmarksCanvasGroup.interactable = false;
            BookmarksCanvasGroup.blocksRaycasts = false;

            ThumbnailsCanvasGroup.alpha = 1.0f;
            ThumbnailsCanvasGroup.interactable = true;
            ThumbnailsCanvasGroup.blocksRaycasts = true;
        }

        public void SetActive(bool active)
        {
	        gameObject.SetActive(active);

            if (!active)
            {
	            Viewer.m_Internal.Viewport.offsetMin = new Vector2(0.0f, Viewer.m_Internal.Viewport.offsetMin.y);
	            Viewer.m_Internal.HorizontalScrollBar.offsetMin = new Vector2(0.0f, Viewer.m_Internal.HorizontalScrollBar.offsetMin.y);
            }
            else
            {
                UpdateViewport();
            }
        }

        public bool IsOpened
        {
            get { return m_Opened; }
        }

        public void SetOpened(bool opened)
        {
            m_Opened = opened;
            UpdateGraphics();
            UpdateViewport();
        }

        public void Toggle()
        {
            m_Opened = !m_Opened;
            UpdateGraphics();
            UpdateViewport();
        }

        protected override void OnEnable()
        {
	        m_LastPanelWidth = 350.0f;

            UpdateViewport();
        }

        private void UpdateGraphics()
        {
            m_SideBarImage.sprite = m_Opened ? m_CloseSprite : m_OpenSprite;

            if (m_Opened)
            {
	            RectTransform.sizeDelta = new Vector2(m_LastPanelWidth, RectTransform.sizeDelta.y);
            }
            else
            {
	            RectTransform.sizeDelta = new Vector2(24.0f, RectTransform.sizeDelta.y);
            }
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (m_Opened)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
#endif
        }

        private void UpdateViewport()
        {
	        RectTransform viewport = Viewer.m_Internal.Viewport;
	        RectTransform horizontalScrollBar = Viewer.m_Internal.HorizontalScrollBar;

            if (Math.Abs(viewport.offsetMin.x - RectTransform.sizeDelta.x) > 0.01f)
            {
	            viewport.offsetMin = new Vector2(RectTransform.sizeDelta.x, viewport.offsetMin.y);
	            horizontalScrollBar.offsetMin = new Vector2(RectTransform.sizeDelta.x, horizontalScrollBar.offsetMin.y);
            }
        }
    }
}