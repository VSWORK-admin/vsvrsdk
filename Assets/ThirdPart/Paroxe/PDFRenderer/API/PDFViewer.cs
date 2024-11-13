﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Paroxe.PdfRenderer.Internal;
using Paroxe.PdfRenderer.Internal.Viewer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Paroxe.PdfRenderer.WebGL;
using System.IO;

namespace Paroxe.PdfRenderer
{
    /// <summary>
    /// PDFViewer is an Unity UI component that allow you to visualize PDF Document.
    /// </summary>
    public class PDFViewer : UIBehaviour, IPDFDevice, IPDFColoredRectListProvider
    {
        [SerializeField]
        public PDFViewerInternal m_Internal;

        private IPDFDeviceActionHandler m_BookmarksActionHandler;
        private IPDFDeviceActionHandler m_LinksActionHandler;
        private PDFPageRange m_CurrentPageRange;
        private PDFSearchResult m_CurrentSearchResult;
        private PDFDocument m_Document;
#pragma warning disable 414
        private PDFDocument m_SuppliedDocument;
#pragma warning restore 414
        private PDFPageTextureHolder[] m_PageTextureHolders;
        private int m_CurrentSearchResultIndex;
        private int m_CurrentSearchResultIndexWithinCurrentPage;
        private bool m_DelayedOnEnable;
        private float m_InvalidPasswordMessageDelay;
#if !UNITY_WEBGL || UNITY_EDITOR
        private float m_InvalidPasswordMessageDelayBeforeFade = 0.5f;
        private bool m_DownloadCanceled = false;
#endif
        private bool m_InvalidPasswordMessageVisisble;
        private bool m_IsLoaded;
        private int m_LoadAtPageIndex;
        private float OverlayAlpha = 0.50f;
        private bool OverlayVisible;
        private int m_PageCount;
        private float[] m_PageOffsets;
        private Vector2[] m_PageSizes;
        private Vector2[] m_NormalPageSizes;
        private byte[] m_PendingDocumentBuffer;
        private int m_PreviousMostVisiblePage = -1;
        private PageFittingType m_PreviousPageFitting;
        private float m_PreviousZoom;
        private float m_PreviousZoomToGo;
        private IList<PDFSearchResult>[] m_SearchResults;
        private float m_StartZoom;
        private float m_UpdateChangeDelay;
        private Vector2 m_ZoomPosition = Vector2.zero;
        private PDFRenderer m_Renderer;
        private int m_PreviousTouchCount;
        private float m_PinchZoomStartZoomFactor;
        private float m_PinchZoomStartDeltaMag;
        private Canvas m_Canvas;
        private GraphicRaycaster m_GraphicRaycaster;
        private List<Canvas> m_CanvasList = new List<Canvas>();
        private ScrollRect m_ViewportScrollRect;
        private Scrollbar m_HorizontalScrollBar;
        private Scrollbar m_VerticalScrollBar;
        private int? m_LastSetLabelPageIndex;
        private int? m_LastSetLabelPageCount;

        /// ...
        private PDFThumbnailsViewer m_ThumbnailsViewer;
        private PDFBookmarksViewer m_BookmarksViewer;

        [SerializeField]
        private bool m_AllowOpenURL = true;
        [SerializeField]
        private bool m_ChangeCursorWhenOverURL = true;
        [SerializeField]
        private GameObject m_BytesSupplierObject;
        [SerializeField]
        private Component m_BytesSupplierComponent;
        [SerializeField]
        private string m_BytesSupplierFunctionName;
        [SerializeField]
        private string m_FileName = "";
        [SerializeField]
        private string m_FilePath = "";
        [SerializeField]
        private FileSourceType m_FileSource = FileSourceType.Resources;
        [SerializeField]
        private string m_FileURL = "";
        [SerializeField]
        private string m_Folder = "";
        [SerializeField]
        private bool m_LoadOnEnable = true;
        [SerializeField]
        private float m_MaxZoomFactor = 8.0f;
        [SerializeField]
        private float m_MaxZoomFactorTextureQuality = 4.0f;
        [SerializeField]
        private float m_MinZoomFactor = 0.25f;
        [SerializeField]
        private PageFittingType m_PageFitting = PageFittingType.Zoom;
        [SerializeField]
        private string m_Password = "";
        [SerializeField]
        private PDFAsset m_PDFAsset = null;
        [SerializeField]
        private float m_ZoomFactor = 1.0f;
        [SerializeField]
        private float m_ZoomStep = 0.25f;
        [SerializeField]
        private float m_ZoomToGo;
        [SerializeField]
        private float m_VerticalMarginBetweenPages = 20.0f;
        [SerializeField]
        private bool m_UnloadOnDisable;
        [SerializeField]
        private bool m_ShowVerticalScrollBar = false;
        [SerializeField]
        private bool m_ShowBookmarksViewer = true;
        [SerializeField]
        private bool m_ShowHorizontalScrollBar = true;
        [SerializeField]
        private bool m_ShowThumbnailsViewer = true;
        [SerializeField]
        private bool m_ShowTopBar = false;
        [SerializeField]
        private float m_ScrollSensitivity = 75.0f;
        [SerializeField]
        private Color m_SearchResultColor = new Color(0.0f, 115 / 255.0f, 230 / 255.0f, 125 / 255.0f);
        [SerializeField]
        private Vector2 m_SearchResultPadding = new Vector2(2.0f, 4.0f);
        [SerializeField, Range(0.0f, 1.0f)]
        private float m_SearchTimeBudgetPerFrame = 0.60f;
        [SerializeField]
        private PDFRenderer.RenderSettings m_RenderSettings = new PDFRenderer.RenderSettings();
        [SerializeField]
        private float m_DelayAfterZoomingBeforeUpdate = 0.005f;
        [SerializeField]
        private float m_ParagraphZoomFactor = 2.0f;
        [SerializeField]
        private bool m_ParagraphZoomingEnable = true;
        [SerializeField]
        private float m_ParagraphDetectionThreshold = 12.0f;

        public delegate void CancelEventHandler(PDFViewer sender);
        public delegate void CurrentPageChangedEventHandler(PDFViewer sender, int oldPageIndex, int newPageIndex);
        public delegate void DocumentChangedEventHandler(PDFViewer sender, PDFDocument document);
        public delegate void LoadFailEventHandler(PDFViewer sender);
        public delegate void PDFViewerEventHandler(PDFViewer sender);
        public delegate void ZoomChangedEventHandler(PDFViewer sender, float oldZoom, float newZoom);

        public event CurrentPageChangedEventHandler OnCurrentPageChanged;
        public event PDFViewerEventHandler OnDisabled;
        public event DocumentChangedEventHandler OnDocumentLoaded;
        public event LoadFailEventHandler OnDocumentLoadFailed;
        public event DocumentChangedEventHandler OnDocumentUnloaded;
        public event CancelEventHandler OnDownloadCancelled;
        public event CancelEventHandler OnPasswordCancelled;
        public event ZoomChangedEventHandler OnZoomChanged;

        public enum FileSourceType
        {
            None,
            Web,
            StreamingAssets,
            Resources,
            FilePath,
            Bytes,
            Asset,
            DocumentObject,
            PersistentData
        }

        public enum PageFittingType
        {
            ViewerWidth,
            ViewerHeight,
            WholePage,
            Zoom
        }

        public enum ViewerModeType
        {
            Move,
            ZoomOut,
            ZoomIn
        }

        /// <summary>
        /// Return parent canvas.
        /// </summary>
        public Canvas canvas
        {
            get
            {
                if (m_Canvas == null)
                    CacheCanvas();
                return m_Canvas;
            }
        }

        /// <summary>
        /// Specify if the PDFViewer can open url link with external browser.
        /// </summary>
        public bool AllowOpenURL
        {
            get { return m_AllowOpenURL; }
            set { m_AllowOpenURL = value; }
        }

        /// <summary>
        /// Specify if the cursor change when over url links.
        /// </summary>
        public bool ChangeCursorWhenOverURL
        {
            get { return m_ChangeCursorWhenOverURL; }
            set { m_ChangeCursorWhenOverURL = value; }
        }

        /// <summary>
        /// Specify the viewport background color.
        /// </summary>
        public Color BackgroundColor
        {
            get { return m_Internal.Viewport.GetComponent<Image>().color; }
            set
            {
                if (m_Internal.Viewport.GetComponent<Image>().color != value)
                    m_Internal.Viewport.GetComponent<Image>().color = value;
            }
        }

        /// <summary>
        /// Specify the relative amount of time is allowed for text search per frame (0.0f to 1.0f).
        /// </summary>
        public float SearchTimeBudgetPerFrame
        {
            get { return m_SearchTimeBudgetPerFrame; }
            set { m_SearchTimeBudgetPerFrame = Mathf.Clamp01(value); }
        }

        /// <summary>
        /// Specify the action handler for bookmarks.
        /// </summary>
        public IPDFDeviceActionHandler BookmarksActionHandler
        {
            get { return m_BookmarksActionHandler; }
            set { m_BookmarksActionHandler = value; }
        }

        /// <summary>
        /// Specify the action handler for links.
        /// </summary>
        public IPDFDeviceActionHandler LinksActionHandler
        {
            get { return m_LinksActionHandler; }
            set { m_LinksActionHandler = value; }
        }

        /// <summary>
        /// This property is intended to be used along side with the bytes file source. (FileSource.Bytes)
        /// Specify from which component the byte suplier function resides.
        /// </summary>
        public Component BytesSupplierComponent
        {
            get { return m_BytesSupplierComponent; }
            set { m_BytesSupplierComponent = value; }
        }

        /// <summary>
        /// This property is intended to be used along side with the bytes file source. (FileSource.Bytes)
        /// Specify the function name whithin the byte supplier component.
        /// </summary>
        public string BytesSupplierFunctionName
        {
            get { return m_BytesSupplierFunctionName; }
            set { m_BytesSupplierFunctionName = value; }
        }

        public int CurrentPageIndex
        {
            get { return GetMostVisiblePageIndex(); }
            set
            {
                int mostVisible = GetMostVisiblePageIndex();

                if (value != mostVisible)
                    GoToPage(value);
            }
        }

        public int CurrentSearchResultIndex
        {
            get { return m_CurrentSearchResultIndex; }
        }

        /// <summary>
        /// Return the byte array of the current loaded pdf document.
        /// </summary>
        public byte[] DataBuffer
        {
            get
            {
                if (Document != null)
                    return m_Document.DocumentBuffer;
                return null;
            }
        }

        public PDFDocument Document
        {
            get { return m_Document; }
        }

        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value != null ? value.Trim() : ""; }
        }

        public string FilePath
        {
            get { return m_FilePath; }
            set { m_FilePath = value; }
        }

        public FileSourceType FileSource
        {
            get { return m_FileSource; }

            set { m_FileSource = value; }
        }

        public string FileURL
        {
            get { return m_FileURL; }
            set { m_FileURL = value; }
        }

        public string Folder
        {
            get { return m_Folder; }
            set { m_Folder = value; }
        }

        public bool IsLoaded
        {
            get { return m_IsLoaded; }
        }

        public bool LoadOnEnable
        {
            get { return m_LoadOnEnable; }
            set { m_LoadOnEnable = value; }
        }

        public float MaxZoomFactorTextureQuality
        {
            get { return m_MaxZoomFactorTextureQuality; }
            set
            {
                if (Math.Abs(Mathf.Clamp(value, MinZoomFactor, MaxZoomFactor) - m_MaxZoomFactorTextureQuality) > float.Epsilon)
                {
                    m_MaxZoomFactorTextureQuality = Mathf.Clamp(value, MinZoomFactor, MaxZoomFactor);

                    m_UpdateChangeDelay = 1.0f;
                }
            }
        }

        public float MinZoomFactor
        {
            get { return m_MinZoomFactor; }
            set
            {
                m_MinZoomFactor = value;

                if (m_MinZoomFactor < 0.01f)
                    m_MinZoomFactor = 0.01f;
            }
        }

        public float MaxZoomFactor
        {
            get { return m_MaxZoomFactor; }
            set
            {
                m_MaxZoomFactor = value;

                if (m_MaxZoomFactor < m_MinZoomFactor)
                    m_MaxZoomFactor = m_MinZoomFactor;
            }
        }

        public PageFittingType PageFitting
        {
            get { return m_PageFitting; }
            set { m_PageFitting = value; }
        }

        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        /// <summary>
        /// Intended to be used along side the Asset file source (FileSource.Asset)
        /// </summary>
        public PDFAsset PDFAsset
        {
            get { return m_PDFAsset; }
            set { m_PDFAsset = value; }
        }

        public bool RenderAnnotations
        {
            get { return m_RenderSettings.renderAnnotations; }

            set
            {
                if (m_RenderSettings.renderAnnotations != value)
                {
                    m_RenderSettings.renderAnnotations = value;

                    m_UpdateChangeDelay = 0.1f;
                }
            }
        }

        public bool RenderGrayscale
        {
            get { return m_RenderSettings.grayscale; }

            set
            {
                if (m_RenderSettings.grayscale != value)
                {
                    m_RenderSettings.grayscale = value;

                    m_UpdateChangeDelay = 0.1f;
                }
            }
        }

        public float ScrollSensitivity
        {
            get { return m_ScrollSensitivity; }
            set { m_ScrollSensitivity = value; }
        }

        public Color SearchResultColor
        {
            get { return m_SearchResultColor; }
            set
            {
                if (m_SearchResultColor != value)
                {
                    m_SearchResultColor = value;
                    m_UpdateChangeDelay = 0.25f;
                }
            }
        }

        public Vector2 SearchResultPadding
        {
            get { return m_SearchResultPadding; }
            set
            {
                if (m_SearchResultPadding != value)
                {
                    m_SearchResultPadding = value;
                    m_UpdateChangeDelay = 0.25f;
                }
            }
        }

        public bool ShowBookmarksViewer
        {
            get { return m_ShowBookmarksViewer; }
            set
            {
                if (m_ShowBookmarksViewer != value)
                {
                    m_ShowBookmarksViewer = value;

                    UpdateBookmarksViewerVisibility(m_ShowBookmarksViewer);
                }
            }
        }

        private void UpdateBookmarksViewerVisibility(bool visible)
        {
            if (visible && m_IsLoaded)
            {
#if !UNITY_WEBGL
                if (m_BookmarksViewer.RootBookmark == null || m_BookmarksViewer.RootBookmark.ChildCount == 0)
#endif
                    visible = false;
            }

            if (m_Internal.LeftPanel != null)
            {
                m_Internal.LeftPanel.Bookmarks.gameObject.SetActive(visible);
                m_Internal.LeftPanel.BookmarksTab.gameObject.SetActive(visible);

                m_Internal.LeftPanel.SetActive(m_ShowThumbnailsViewer || visible);

                if (!visible && m_ShowThumbnailsViewer)
                    m_Internal.LeftPanel.OnThumbnailsTabClicked();
                else if (visible && !m_ShowThumbnailsViewer)
                    m_Internal.LeftPanel.OnBookmarksTabClicked();
                else
                    m_Internal.LeftPanel.OnBookmarksTabClicked();
            }
        }

        public bool ShowHorizontalScrollBar
        {
            get { return m_ShowHorizontalScrollBar; }
            set
            {
                if (m_ShowHorizontalScrollBar != value)
                {
                    m_ShowHorizontalScrollBar = value;

                    UpdateScrollBarVisibility();
                }

            }
        }

        public bool ShowThumbnailsViewer
        {
            get { return m_ShowThumbnailsViewer; }
            set
            {
                if (m_ShowThumbnailsViewer != value)
                {
                    m_ShowThumbnailsViewer = value;

                    if (m_Internal.LeftPanel != null)
                    {
                        m_Internal.LeftPanel.ThumbnailsViewer.gameObject.SetActive(m_ShowThumbnailsViewer);
                        m_Internal.LeftPanel.ThumbnailsTab.gameObject.SetActive(m_ShowThumbnailsViewer);

                        m_Internal.LeftPanel.SetActive(m_ShowThumbnailsViewer || m_Internal.LeftPanel.Bookmarks.gameObject.activeSelf);

                        if (!m_Internal.LeftPanel.Bookmarks.gameObject.activeSelf && m_ShowThumbnailsViewer)
                            m_Internal.LeftPanel.OnThumbnailsTabClicked();
                        else if (m_Internal.LeftPanel.Bookmarks.gameObject.activeSelf && !m_ShowThumbnailsViewer)
                            m_Internal.LeftPanel.OnBookmarksTabClicked();
                        else
                            m_Internal.LeftPanel.OnBookmarksTabClicked();
                    }
                }
            }
        }

        public bool ShowTopBar
        {
            get { return m_ShowTopBar; }
            set
            {
                if (m_ShowTopBar != value)
                {
                    m_ShowTopBar = value;

                    if (!m_ShowTopBar)
                    {
                        m_Internal.TopPanel.gameObject.SetActive(false);
                        m_Internal.TopPanel.sizeDelta = new Vector2(0.0f, 0.0f);

                        m_Internal.Viewport.offsetMax = new Vector2(m_Internal.Viewport.offsetMax.x, 0.0f);
                        m_Internal.VerticalScrollBar.offsetMax =
                            new Vector2(m_Internal.VerticalScrollBar.offsetMax.x, 0.0f);

                        if (m_Internal.LeftPanel != null)
                        {
                            ((RectTransform)m_Internal.LeftPanel.transform).sizeDelta =
                                new Vector2(((RectTransform)m_Internal.LeftPanel.transform).sizeDelta.x, 0.0f);
                        }
                    }
                    else
                    {
                        m_Internal.TopPanel.gameObject.SetActive(true);
                        m_Internal.TopPanel.sizeDelta = new Vector2(0.0f, 60.0f);

                        m_Internal.Viewport.offsetMax = new Vector2(m_Internal.Viewport.offsetMax.x, -60.0f);
                        m_Internal.VerticalScrollBar.offsetMax =
                            new Vector2(m_Internal.VerticalScrollBar.offsetMax.x, -59.0f);

                        if (m_Internal.LeftPanel != null)
                        {
                            ((RectTransform)m_Internal.LeftPanel.transform).sizeDelta =
                                new Vector2(((RectTransform)m_Internal.LeftPanel.transform).sizeDelta.x, -59.0f);
                        }
                    }
                }
            }
        }

        public bool ShowVerticalScrollBar
        {
            get { return m_ShowVerticalScrollBar; }
            set
            {
                if (m_ShowVerticalScrollBar != value)
                {
                    m_ShowVerticalScrollBar = value;

                    UpdateScrollBarVisibility();
                }
            }
        }

        public bool UnloadOnDisable
        {
            get { return m_UnloadOnDisable; }
            set { m_UnloadOnDisable = value; }
        }

        public float VerticalMarginBetweenPages
        {
            get { return m_VerticalMarginBetweenPages; }
            set
            {
                if (m_VerticalMarginBetweenPages != value)
                {
                    if (value < 0.0f)
                    {
                        m_VerticalMarginBetweenPages = 0.0f;
                    }
                    else
                    {
                        m_VerticalMarginBetweenPages = value;
                    }

                    if (m_IsLoaded)
                    {
                        ComputePageOffsets();
                        UpdatePagesPlacement();
                        m_Internal.PageContainer.sizeDelta = GetDocumentSize();
                        EnsureValidPageContainerPosition();
                    }
                }
            }
        }

        public float ZoomFactor
        {
            get { return m_ZoomToGo; }
            set
            {
                if (Math.Abs(m_ZoomToGo - Mathf.Clamp(value, MinZoomFactor, MaxZoomFactor)) > float.Epsilon)
                {
                    m_ZoomToGo = Mathf.Clamp(value, MinZoomFactor, MaxZoomFactor);

                    m_ZoomPosition = new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f);

                    NotifyZoomChanged(m_PreviousZoomToGo, m_ZoomToGo);

                    m_PageFitting = PageFittingType.Zoom;
                }
            }
        }

        public float ZoomStep
        {
            get { return m_ZoomStep; }
            set { m_ZoomStep = value; }
        }

        public bool ParagraphZoomingEnable
        {
            get { return m_ParagraphZoomingEnable; }
            set { m_ParagraphZoomingEnable = value; }
        }

        public float ParagraphZoomFactor
        {
            get { return m_ParagraphZoomFactor; }
            set { m_ParagraphZoomFactor = value; }
        }

        public float ParagraphDetectionThreshold
        {
            get { return m_ParagraphDetectionThreshold; }
            set { m_ParagraphDetectionThreshold = value; }
        }

        public void LoadDocument(int pageIndex = 0)
        {
            if (m_IsLoaded)
                CleanUp();

            CommonLoad();
        }

        public void LoadDocument(PDFDocument document, int pageIndex = 0)
        {
            LoadDocument(document, null, pageIndex);
        }

        public void LoadDocument(PDFDocument document, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.DocumentObject;

            m_SuppliedDocument = document;
            m_Password = password;

            CommonLoad();
        }

        public void LoadDocumentFromAsset(PDFAsset pdfAsset, int pageIndex = 0)
        {
            LoadDocumentFromAsset(pdfAsset, null, pageIndex);
        }

        public void LoadDocumentFromAsset(PDFAsset pdfAsset, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
            {
                CleanUp();
            }

            m_FileSource = FileSourceType.Asset;

            m_PDFAsset = pdfAsset;
            m_Password = password;

            CommonLoad(pdfAsset.m_FileContent);
        }

        public void LoadDocumentFromResources(string folder, string fileName, int pageIndex = 0)
        {
            LoadDocumentFromResources(folder, fileName, null, pageIndex);
        }

        public void LoadDocumentFromResources(string folder, string fileName, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.Resources;

            m_Folder = folder;
            m_FileName = fileName;
            m_FilePath = GetFileLocation();

            m_Password = password;

            CommonLoad();
        }

        public void LoadDocumentFromStreamingAssets(string folder, string fileName, int pageIndex = 0)
        {
            LoadDocumentFromStreamingAssets(folder, fileName, null, pageIndex);
        }

        public void LoadDocumentFromStreamingAssets(string folder, string fileName, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.StreamingAssets;

            m_Folder = folder;
            m_FileName = fileName;
            m_FilePath = GetFileLocation();

            m_Password = password;

            CommonLoad();
        }

        public void LoadDocumentFromPersistentData(string folder, string fileName, int pageIndex = 0)
        {
            LoadDocumentFromPersistentData(folder, fileName, null, pageIndex);
        }

        public void LoadDocumentFromPersistentData(string folder, string fileName, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.PersistentData;

            m_Folder = folder;
            m_FileName = fileName;
            m_FilePath = GetFileLocation();

            m_Password = password;

            CommonLoad();
        }

        public void LoadDocumentFromWeb(string url, int pageIndex = 0)
        {
            LoadDocumentFromWeb(url, null, pageIndex);
        }

        public void LoadDocumentFromWeb(string url, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.Web;

            m_FileURL = url;
            m_FilePath = GetFileLocation();

            m_Password = password;

            CommonLoad();
        }

        public void LoadDocumentFromBuffer(byte[] buffer, int pageIndex = 0)
        {
            LoadDocumentFromBuffer(buffer, null, pageIndex);
        }

        public void LoadDocumentFromBuffer(byte[] buffer, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.Bytes;

            m_Password = password;

            CommonLoad(buffer);
        }

        public void LoadDocumentFromFile(string filePath, int pageIndex = 0)
        {
            LoadDocumentFromFile(filePath, null, pageIndex);
        }

        public void LoadDocumentFromFile(string filePath, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.FilePath;

            m_FilePath = filePath;
            m_Password = password;
            m_FilePath = GetFileLocation();

            m_Password = password;

            CommonLoad();
        }

        public void AdjustZoomToPageFitting(PageFittingType pageFitting, Vector2 referencePageSize)
        {
            switch (pageFitting)
            {
                case PageFittingType.ViewerWidth:
                    {
                        float firstPageWidth = referencePageSize.x;
                        float viewportWidth = m_Internal.Viewport.rect.size.x;
                        m_ZoomToGo = viewportWidth / firstPageWidth;

                        break;
                    }
                case PageFittingType.ViewerHeight:
                    {
                        float firstPageHeight = referencePageSize.y;
                        float viewportHeight = m_Internal.Viewport.rect.size.y;
                        m_ZoomToGo = viewportHeight / firstPageHeight;

                        break;
                    }
                case PageFittingType.WholePage:
                    {
                        float firstPageWidth = referencePageSize.x;
                        float firstPageHeight = referencePageSize.y + 2.0f * m_VerticalMarginBetweenPages;
                        float viewportWidth = m_Internal.Viewport.rect.size.x;
                        float viewportHeight = m_Internal.Viewport.rect.size.y;

                        m_ZoomToGo = Mathf.Min(viewportWidth / firstPageWidth, viewportHeight / firstPageHeight);

                        break;
                    }
                case PageFittingType.Zoom:
                    {
                        break;
                    }
            }
        }

        public void CloseDocument()
        {
            if (m_IsLoaded)
            {
                CleanUp();
            }
        }

        public string GetFileLocation()
        {
            switch (m_FileSource)
            {
                case FileSourceType.FilePath:
                    return m_FilePath;

                case FileSourceType.Resources:
                    string folder = m_Folder + "/";
                    if (string.IsNullOrEmpty(m_Folder))
                        folder = "";

                    return (folder + m_FileName).Replace("//", "/").Replace(@"\\", @"/").Replace(@"\", @"/");

                case FileSourceType.StreamingAssets:
                    folder = m_Folder + "/";
                    if (string.IsNullOrEmpty(m_Folder))
                        folder = "";

                    string location = ("/" + folder + m_FileName).Replace("//", "/")
                        .Replace(@"\\", @"/")
                        .Replace(@"\", @"/");
                    return Application.streamingAssetsPath + location;

                case FileSourceType.PersistentData:
                    folder = m_Folder + "/";
                    if (string.IsNullOrEmpty(m_Folder))
                        folder = "";

                    location = ("/" + folder + m_FileName).Replace("//", "/")
                        .Replace(@"\\", @"/")
                        .Replace(@"\", @"/");
                    return Application.persistentDataPath + location;

                case FileSourceType.Web:
                    return m_FileURL;

                default:
                    return "";
            }
        }

        public void GoToNextPage()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            int mostVisiblePage = GetMostVisiblePageIndex();

            if (mostVisiblePage + 1 < m_PageCount)
            {
                GoToPage(mostVisiblePage + 1);
            }
            else
            {
                m_Internal.PageContainer.anchoredPosition = new Vector2(
                    m_Internal.PageContainer.anchoredPosition.x,
                    m_Internal.PageContainer.sizeDelta.y - m_Internal.Viewport.rect.size.y);
            }
        }

        public void GoToNextSearchResult()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            if (m_SearchResults != null && m_SearchResults.Length > 0)
            {
                ++m_CurrentSearchResultIndex;
                ++m_CurrentSearchResultIndexWithinCurrentPage;

                int oldPageIndex = m_CurrentSearchResult.PageIndex;

                if (m_CurrentSearchResultIndexWithinCurrentPage >= m_SearchResults[m_CurrentSearchResult.PageIndex].Count)
                {
                    int nextPage = m_CurrentSearchResult.PageIndex + 1;
                    while (nextPage < m_PageCount - 1 && m_SearchResults[nextPage].Count == 0)
                    {
                        ++nextPage;
                    }

                    if (nextPage <= m_PageCount - 1 && m_SearchResults[nextPage].Count > 0)
                    {
                        m_CurrentSearchResultIndexWithinCurrentPage = 0;

                        m_CurrentSearchResult = m_SearchResults[nextPage][0];

                        if (oldPageIndex != nextPage)
                        {
                            GoToPage(nextPage);
                        }

                    }
                    else
                    {
                        --m_CurrentSearchResultIndexWithinCurrentPage;
                        --m_CurrentSearchResultIndex;

                        if (!m_CurrentPageRange.ContainsPage(m_CurrentSearchResult.PageIndex))
                            GoToPage(m_CurrentSearchResult.PageIndex);
                    }
                }
                else
                {
                    m_CurrentSearchResult =
                        m_SearchResults[m_CurrentSearchResult.PageIndex][m_CurrentSearchResultIndexWithinCurrentPage];

                    if (!m_CurrentPageRange.ContainsPage(m_CurrentSearchResult.PageIndex))
                        GoToPage(m_CurrentSearchResult.PageIndex);
                }
            }
        }

        public void GoToPage(int pageIndex)
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            if (pageIndex < 0)
            {
                pageIndex = 0;
            }
            else if (pageIndex > m_PageCount - 1)
            {
                pageIndex = m_PageCount - 1;
            }

            m_Internal.PageInputField.text = (pageIndex + 1).ToString();
            m_Internal.PageContainer.anchoredPosition = new Vector2(m_Internal.PageContainer.anchoredPosition.x,
                m_PageOffsets[pageIndex] - m_PageSizes[pageIndex].y * 0.5f);
            m_Internal.PageContainer.anchoredPosition -= m_VerticalMarginBetweenPages * Vector2.up;

            SetPageCountLabel(pageIndex, m_PageCount);

            EnsureValidPageContainerPosition();
        }

        public void GoToPreviousPage()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            int mostVisiblePage = GetMostVisiblePageIndex();

            if (mostVisiblePage - 1 >= 0)
            {
                GoToPage(mostVisiblePage - 1);
            }
            else
            {
                m_Internal.PageContainer.anchoredPosition = Vector2.zero;
            }
        }

        public void GoToPreviousSearchResult()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            if (m_SearchResults != null && m_SearchResults.Length > 0 && m_CurrentSearchResultIndex > 0)
            {
                --m_CurrentSearchResultIndex;
                --m_CurrentSearchResultIndexWithinCurrentPage;

                int oldPageIndex = m_CurrentSearchResult.PageIndex;

                if (m_CurrentSearchResultIndexWithinCurrentPage < 0)
                {
                    int prevPage = m_CurrentSearchResult.PageIndex - 1;
                    while (prevPage >= 0 && m_SearchResults[prevPage].Count == 0)
                    {
                        --prevPage;
                    }

                    if (prevPage >= 0 && m_SearchResults[prevPage].Count > 0)
                    {
                        m_CurrentSearchResultIndexWithinCurrentPage = m_SearchResults[prevPage].Count - 1;
                        m_CurrentSearchResult = m_SearchResults[prevPage][m_SearchResults[prevPage].Count - 1];

                        if (oldPageIndex != prevPage)
                        {
                            GoToPage(prevPage);
                        }
                    }
                    else
                    {
                        ++m_CurrentSearchResultIndexWithinCurrentPage;
                        ++m_CurrentSearchResultIndex;

                    }
                }
                else
                {
                    m_CurrentSearchResult =
                        m_SearchResults[m_CurrentSearchResult.PageIndex][m_CurrentSearchResultIndexWithinCurrentPage];

                    if (!m_CurrentPageRange.ContainsPage(m_CurrentSearchResult.PageIndex))
                        GoToPage(m_CurrentSearchResult.PageIndex);
                }
            }
        }

        public void CancelDownload()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            StopCoroutine(DownloadFileFromWWW());

            m_Internal.DownloadDialog.gameObject.SetActive(false);

            m_DownloadCanceled = true;

            NotifyDownloadCancelled();
#endif
        }

        public void OnPageEditEnd()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            if (string.IsNullOrEmpty(m_Internal.PageInputField.text))
                return;

            int pageIndex = int.Parse(m_Internal.PageInputField.text) - 1;

            GoToPage(pageIndex);
        }

        public void OnPasswordDialogCancelButtonClicked()
        {
            m_InvalidPasswordMessageVisisble = false;
            m_Internal.InvalidPasswordImage.gameObject.SetActive(false);
            m_Internal.InvalidPasswordImage.GetComponent<CanvasGroup>().alpha = 1.0f;

            NotifyPasswordCancelled();

            CleanUp();
        }

        public void OnPasswordDialogOkButtonClicked()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            m_Password = m_Internal.PasswordInputField.text;

            if (TryLoadDocumentWithBuffer(m_PendingDocumentBuffer, m_Password))
            {
                m_Internal.PasswordDialog.gameObject.SetActive(false);

                m_InvalidPasswordMessageVisisble = false;
                m_Internal.InvalidPasswordImage.gameObject.SetActive(false);
                m_Internal.InvalidPasswordImage.GetComponent<CanvasGroup>().alpha = 1.0f;

                m_Internal.PasswordInputField.text = "";
            }
            else
            {
                m_InvalidPasswordMessageVisisble = true;
                m_Internal.InvalidPasswordImage.gameObject.SetActive(true);
                m_Internal.InvalidPasswordImage.GetComponent<CanvasGroup>().alpha = 1.0f;
                m_InvalidPasswordMessageDelay = m_InvalidPasswordMessageDelayBeforeFade;

                m_Internal.PasswordInputField.Select();
            }
#endif
        }

        public void ReloadDocument(int pageIndex = 0)
        {
            LoadDocument(pageIndex);
        }

        public bool SaveDocumentAsFile(string path)
        {
            if (m_Document == null || m_Document.DocumentBuffer == null)
            {
                Debug.LogError("Error while saving document: there is no document loaded.");
                return false;
            }

            if (!new Uri(path).IsWellFormedOriginalString())
            {
                Debug.LogError("Error while saving document: the path is not well formed => " + path);
                return false;
            }

            try
            {
                FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                stream.Write(m_Document.DocumentBuffer, 0, m_Document.DocumentBuffer.Length);
                stream.Close();

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception while saving document: " + ex);
            }

            return false;
        }

        public void SetSearchResults(IList<PDFSearchResult>[] searchResults)
        {
            m_SearchResults = searchResults;

            if (m_SearchResults != null && m_SearchResults.Length > 0)
            {
                m_CurrentSearchResultIndex = 0;
                m_CurrentSearchResultIndexWithinCurrentPage = 0;

                for (int i = 0; i < m_PageCount; ++i)
                {
                    if (m_SearchResults[i] != null && m_SearchResults[i].Count > 0)
                    {
                        m_CurrentSearchResult = m_SearchResults[i][0];
                        break;
                    }
                }
            }
            else
            {

                m_CurrentSearchResult = new PDFSearchResult(-1, 0, 0);
                m_CurrentSearchResultIndex = 0;
            }

            AdjustCurrentSearchResultDisplayed();

            m_UpdateChangeDelay = 0.25f;
        }

        public void UnloadDocument()
        {
            if (m_IsLoaded)
                CleanUp();
        }

        public void ZoomIn()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            ZoomCommon(new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f), true);
        }

        public void ZoomOut()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            ZoomCommon(new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f), false);
        }

        private void AdjustCurrentSearchResultDisplayed()
        {
            if (m_SearchResults != null && m_SearchResults.Length > 0)
            {
                if (!m_CurrentPageRange.ContainsPage(m_CurrentSearchResult.PageIndex))
                {
                    int minPage = m_CurrentPageRange.m_From;
                    int maxPage = m_CurrentPageRange.m_To;

                    bool minFound = false;
                    bool maxFound = false;

                    for (int i = minPage; i >= 0; --i)
                    {
                        if (m_SearchResults[i] != null && m_SearchResults[i].Count > 0)
                        {
                            minFound = true;
                            minPage = i;
                            break;
                        }
                    }

                    for (int i = maxPage; i < m_PageCount; ++i)
                    {
                        if (m_SearchResults[i] != null && m_SearchResults[i].Count > 0)
                        {
                            maxFound = true;
                            maxPage = i;
                            break;
                        }
                    }

                    int disMinPage = Math.Abs(m_CurrentPageRange.m_From - minPage);
                    int disMaxPage = Math.Abs(maxPage - m_CurrentPageRange.m_To);

                    int nearestPage = -1;

                    if (disMinPage <= disMaxPage)
                    {
                        if (minFound)
                        {
                            nearestPage = minPage;
                        }
                        else if (maxFound)
                        {
                            nearestPage = maxPage;
                        }
                    }
                    else
                    {
                        if (maxFound)
                        {
                            nearestPage = maxPage;
                        }
                        else if (minFound)
                        {
                            nearestPage = minPage;
                        }
                    }

                    int count = 0;

                    for (int i = 0; i < nearestPage; ++i)
                    {
                        count += m_SearchResults[i].Count;
                    }

                    if (minFound || maxFound)
                    {
                        if (m_CurrentPageRange.ContainsPage(nearestPage)
                            || nearestPage >= m_CurrentPageRange.m_To)
                        {
                            m_CurrentSearchResult = m_SearchResults[nearestPage][0];
                            m_CurrentSearchResultIndex = count;
                            m_CurrentSearchResultIndexWithinCurrentPage = 0;
                        }
                        else
                        {
                            m_CurrentSearchResult = m_SearchResults[nearestPage][m_SearchResults[nearestPage].Count - 1];
                            m_CurrentSearchResultIndex = count + m_SearchResults[nearestPage].Count - 1;
                            m_CurrentSearchResultIndexWithinCurrentPage = m_SearchResults[nearestPage].Count - 1;
                        }
                    }
                }
            }
        }

        private void CleanUp()
        {
            if (m_Document != null)
                NotifyDocumentUnloaded(m_Document);

            m_Document = null;

            if (m_PageTextureHolders != null)
            {
                foreach (PDFPageTextureHolder holder in m_PageTextureHolders)
                {
                    if (holder.Texture != null)
                    {
                        Texture2D tex = holder.Texture;
                        holder.Texture = null;

                        Destroy(tex);
                    }

                    if (holder.Page.name != "Page")
                    {
                        Destroy(holder.Page);
                    }
                    else
                    {
	                    holder.Page.GetComponent<PDFViewerPage>().ClearCache();
                    }
                }
            }

#if !UNITY_WEBGL
            m_Internal.SearchPanel.GetComponent<PDFSearchPanel>().Close();
#endif

            m_IsLoaded = false;

            m_Internal.PageContainer.anchoredPosition = Vector2.zero;
            m_Internal.PageContainer.sizeDelta = Vector2.zero;
            UpdateScrollBarVisibility();
            EnsureValidPageContainerPosition();

            m_ZoomToGo = m_StartZoom;
            m_PageSizes = null;
            m_NormalPageSizes = null;
            m_PageOffsets = null;
            m_PageCount = 0;
            m_PreviousZoom = 0.0f;
            m_PreviousZoomToGo = 0.0f;
            m_PageTextureHolders = null;
            m_CurrentPageRange = null;
            m_PreviousMostVisiblePage = -1;

            OverlayVisible = false;
            m_InvalidPasswordMessageVisisble = false;
            m_Internal.Overlay.gameObject.SetActive(false);
            m_Internal.PasswordDialog.gameObject.SetActive(false);
            m_Internal.DownloadDialog.gameObject.SetActive(false);

            m_LastSetLabelPageIndex = null;
            m_LastSetLabelPageCount = null;
            m_Internal.PageCountLabel.text = "";
            m_Internal.PageZoomLabel.text = "";
            m_Internal.PageInputField.text = "";
        }

        private void CommonLoad(byte[] specifiedBuffer = null)
        {
            UpdateScrollBarVisibility();

            m_IsLoaded = false;

            if (m_FileSource == FileSourceType.None)
            {
                OverlayVisible = true;
                m_Internal.Overlay.gameObject.SetActive(true);
                m_Internal.Overlay.alpha = OverlayAlpha;
                return;
            }

            if (m_FileSource != FileSourceType.DocumentObject)
	            m_SuppliedDocument = null;

#if UNITY_WEBGL && !UNITY_EDITOR
			StartCoroutine(LoadDocument_WebGL(specifiedBuffer));

			return;
#else
            byte[] buffer = specifiedBuffer;

            if (m_FileSource == FileSourceType.DocumentObject)
            {
                TryLoadWithSpecifiedDocument(m_SuppliedDocument);
            }
            else if (m_FileSource == FileSourceType.FilePath)
            {
                buffer = File.ReadAllBytes(GetFileLocation());
                OnLoadingBufferFinished(buffer);
            }
            else if (m_FileSource == FileSourceType.Resources)
            {
                buffer = LoadAssetBytesFromResources(GetFileLocation());
                OnLoadingBufferFinished(buffer);
            }
            else if (m_FileSource == FileSourceType.StreamingAssets)
            {

#if UNITY_ANDROID && !UNITY_EDITOR
                StartCoroutine(DownloadFileFromWWW());
#else
                string location = GetFileLocation();
                if (File.Exists(location))
                    buffer = File.ReadAllBytes(location);
                OnLoadingBufferFinished(buffer);
#endif
            }
            else if (m_FileSource == FileSourceType.PersistentData)
            {
                string location = GetFileLocation();
                if (File.Exists(location))
                    buffer = File.ReadAllBytes(location);
                OnLoadingBufferFinished(buffer);
            }
            else if (m_FileSource == FileSourceType.Web)
            {
                StartCoroutine(DownloadFileFromWWW());
            }
            else if (m_FileSource == FileSourceType.Bytes)
            {
                if (buffer != null)
                {
                    OnLoadingBufferFinished(buffer);
                }
                else if (BytesSupplierComponent != null)
                {
                    MethodInfo methodInfo = BytesSupplierComponent.GetType().GetMethod(BytesSupplierFunctionName);

                    if (methodInfo != null)
                    {
                        buffer = (byte[])methodInfo.Invoke(BytesSupplierComponent, null);
                    }

                    if (buffer != null)
                    {
                        OnLoadingBufferFinished(buffer);
                    }
                }

                if (buffer == null)
                {
                    NotifyDocumentLoadFailed();
                }
            }
            else if (m_FileSource == FileSourceType.Asset)
            {
                if (m_PDFAsset != null && m_PDFAsset.m_FileContent != null && m_PDFAsset.m_FileContent.Length > 0)
                {
                    OnLoadingBufferFinished(m_PDFAsset.m_FileContent);
                }
                else
                {
                    NotifyDocumentLoadFailed();
                }
            }
#endif
        }

        private void ComputePageOffsets()
        {
            float totalOffset = m_VerticalMarginBetweenPages;

            m_PageOffsets = new float[m_PageCount];

            for (int i = 0; i < m_PageCount; ++i)
            {
                m_PageOffsets[i] = totalOffset + m_PageSizes[i].y * 0.5f;

                totalOffset += m_VerticalMarginBetweenPages + m_PageSizes[i].y;
            }
        }

        private void ComputePageSizes()
        {
            m_PageCount = m_Document.GetPageCount();

            m_PageSizes = new Vector2[m_PageCount];

            for (int i = 0; i < m_PageCount; ++i)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                m_PageSizes[i] = m_NormalPageSizes[i] * m_ZoomFactor;
#else
                float w = m_Document.GetPageWidth(i) * m_ZoomFactor;
                float h = m_Document.GetPageHeight(i) * m_ZoomFactor;

                m_PageSizes[i] = new Vector2(w, h);
#endif
            }
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        IEnumerator LoadDocument_WebGL(byte[] specifiedBuffer = null)
        {
            PDFJS_Promise<PDFDocument> documentPromise = null;

            byte[] buffer = specifiedBuffer;

            bool fromUrl = false;
            
            switch (m_FileSource)
            {
                case FileSourceType.DocumentObject:
	                StartCoroutine(LoadWithDocument(m_SuppliedDocument));
                    yield break;
                case FileSourceType.Asset:
                    if (m_PDFAsset.m_FileContent == null || m_PDFAsset.m_FileContent.Length == 0)
                        yield break;

                    documentPromise = PDFDocument.LoadDocumentFromBytesAsync(m_PDFAsset.m_FileContent);
                    break;
                case FileSourceType.Bytes:
                    if (buffer != null)
                    {
                        documentPromise = PDFDocument.LoadDocumentFromBytesAsync(buffer);
                    }
                    else if (BytesSupplierComponent != null)
                    {
                        MethodInfo methodInfo = BytesSupplierComponent.GetType().GetMethod(BytesSupplierFunctionName);
                        
                        if (methodInfo != null)
                            buffer = (byte[])methodInfo.Invoke(BytesSupplierComponent, null);

                        if (buffer != null)
                            documentPromise = PDFDocument.LoadDocumentFromBytesAsync(buffer);
                    }

                    if (buffer == null)
                        yield break;
                    break;
                case FileSourceType.Resources:
                    buffer = LoadAssetBytesFromResources(GetFileLocation());

                    if (buffer != null)
                        documentPromise = PDFDocument.LoadDocumentFromBytesAsync(buffer);
                    else
                        yield break;
                    break;
                case FileSourceType.Web:
                case FileSourceType.FilePath:
                case FileSourceType.StreamingAssets:
                case FileSourceType.PersistentData:
                    documentPromise = PDFDocument.LoadDocumentFromUrlAsync(GetFileLocation());
                    fromUrl = true;
                    
                    break;
            }
             
            if (!fromUrl)
            {
                while (!documentPromise.HasFinished)
                    yield return null;
            }
            else
            {
                OverlayVisible = true;
                m_Internal.Overlay.gameObject.SetActive(true);
                m_Internal.Overlay.GetComponent<CanvasGroup>().alpha = 0.0f;

                m_Internal.DownloadDialog.gameObject.SetActive(true);

                m_Internal.DownloadSourceLabel.text = GetFileLocation();

                m_Internal.ProgressRect.sizeDelta = new Vector2(0.0f, m_Internal.ProgressRect.sizeDelta.y);
                m_Internal.ProgressLabel.text = "0%";

                while (!documentPromise.HasFinished)
                {
                    SetProgress(documentPromise.Progress);

                    yield return null;
                }

                m_Internal.DownloadDialog.gameObject.SetActive(false);
            }

            if (documentPromise.HasSucceeded)
            {
	            StartCoroutine(LoadWithDocument(documentPromise.Result));
            }
            else
            {
                NotifyDocumentLoadFailed();
            }
        }

        private IEnumerator LoadWithDocument(PDFDocument document)
        {
	        m_Document = document;

	        m_NormalPageSizes = new Vector2[m_Document.GetPageCount()];

	        for (int i = 0; i < m_NormalPageSizes.Length; ++i)
	        {
		        PDFJS_Promise<PDFPage> pagePromise = m_Document.GetPageAsync(i);

		        while (!pagePromise.HasFinished)
			        yield return null;

		        if (pagePromise.HasSucceeded)
		        {
			        PDFPage page = pagePromise.Result;

			        m_NormalPageSizes[i] = page.GetPageSize(1.0f);
		        }
		        else
		        {
			        NotifyDocumentLoadFailed();
			        yield break;
		        }
	        }

	        TryLoadWithSpecifiedDocument(m_Document);
        }
#endif

#if !UNITY_WEBGL || UNITY_EDITOR
        private IEnumerator DownloadFileFromWWW()
        {
            OverlayVisible = true;
            m_Internal.Overlay.gameObject.SetActive(true);
            m_Internal.Overlay.alpha = 0.0f;

            m_Internal.DownloadDialog.gameObject.SetActive(true);

            if (m_FileSource == FileSourceType.Web)
            {
                m_Internal.DownloadSourceLabel.text = GetFileLocation();
            }
            else
            {
                m_Internal.DownloadSourceLabel.text = "";
            }

            m_Internal.ProgressRect.sizeDelta = new Vector2(0.0f, m_Internal.ProgressRect.sizeDelta.y);
            m_Internal.ProgressLabel.text = "0%";

            PDFWebRequest www = new PDFWebRequest(GetFileLocation());
            www.SendWebRequest();

            m_DownloadCanceled = false;

            while (!www.isDone && !m_DownloadCanceled)
            {
                SetProgress(www.progress);
                yield return null;
            }
            
            if (!m_DownloadCanceled && string.IsNullOrEmpty(www.error) && www.isDone)
            {
                SetProgress(1.0f);

                OnLoadingBufferFinished(www.bytes);
            }
            else if (m_DownloadCanceled || !string.IsNullOrEmpty(www.error))
            {
                NotifyDocumentLoadFailed();
            }

            www.Dispose();
            www = null;

            m_Internal.DownloadDialog.gameObject.SetActive(false);
        }
#endif

        private void EnsureValidPageContainerPosition()
        {
            if (m_PageSizes == null || GetDocumentSize().x <= m_Internal.Viewport.rect.size.x)
            {
                m_Internal.PageContainer.anchoredPosition = new Vector2(0.0f,
                    m_Internal.PageContainer.anchoredPosition.y);
            }

            if (m_Internal.PageContainer.anchoredPosition.y >
                m_Internal.PageContainer.sizeDelta.y - m_Internal.Viewport.rect.size.y)
            {
                m_Internal.PageContainer.anchoredPosition = new Vector2(
                    m_Internal.PageContainer.anchoredPosition.x,
                    m_Internal.PageContainer.sizeDelta.y - m_Internal.Viewport.rect.size.y);
            }

            if (m_Internal.PageContainer.anchoredPosition.y < 0.0f)
            {
                m_Internal.PageContainer.anchoredPosition = new Vector2(
                    m_Internal.PageContainer.anchoredPosition.x, 0.0f);
            }
        }

        private Vector2 GetDocumentSize()
        {
            Vector2 size = new Vector2(0.0f, 0.0f);

            foreach (Vector2 s in m_PageSizes)
            {
                if (s.x > size.x)
                {
                    size.x = s.x;
                }
            }

            size.y = m_PageOffsets[m_PageCount - 1] + m_PageSizes[m_PageCount - 1].y * 0.5f;

            size.x += 0.0f * m_VerticalMarginBetweenPages;
            size.y += 1.0f * m_VerticalMarginBetweenPages;

            return size;
        }

        private int GetMostVisiblePageIndex()
        {
            int mostVisibleIndex = -1;
            float mostVisibleArea = 0.0f;

            for (int i = m_CurrentPageRange.m_From; i < m_CurrentPageRange.m_To; ++i)
            {
                RectTransform page = (RectTransform)m_Internal.PageContainer.GetChild(i);
                float area = PDFInternalUtils.CalculateRectTransformIntersectArea(page, m_Internal.Viewport);

                if (area > page.sizeDelta.x * page.sizeDelta.y * 0.4f)
                    return i;

                if (area > mostVisibleArea)
                {
                    mostVisibleIndex = i;
                    mostVisibleArea = area;
                }
            }

            return mostVisibleIndex;
        }

        private static bool Intersect(Rect box0, Rect box1)
        {
            if (box0.xMax < box1.xMin || box0.xMin > box1.xMax)
                return false;
            if (box0.yMax < box1.yMin || box0.yMin > box1.yMax)
                return false;

            return true;
        }

        private PDFPageRange GetVisiblePageRange()
        {
            if (m_PageCount == 0)
                throw new Exception("There is no document loaded.");

            PDFPageRange pageRange = new PDFPageRange();

            for (int i = 0; i < m_PageCount; ++i)
            {
                RectTransform rt = (RectTransform)m_Internal.PageContainer.GetChild(i);

                Rect pageRect = new Rect(-m_Internal.PageContainer.anchoredPosition - rt.anchoredPosition, rt.rect.size);
                Rect viewportRect = new Rect(new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f), m_Internal.Viewport.rect.size);

                pageRect.position = Vector2.zero;
                viewportRect.position = Vector2.zero;

                pageRect.center = -m_Internal.PageContainer.anchoredPosition - rt.anchoredPosition;
                viewportRect.center = new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f);

                if (Intersect(pageRect, viewportRect))

                {
                    if (pageRange.m_From == -1)
                    {
                        pageRange.m_From = i;
                    }
                    else
                    {
                        pageRange.m_To = i + 1;
                    }
                }
                else if (pageRange.m_From != -1)
                {
                    break;
                }
            }

            if (pageRange.m_From != -1 && pageRange.m_To == -1)
            {
                pageRange.m_To = pageRange.m_From + 1;
            }

            return pageRange;
        }

        private void InstantiatePageTextureHolders()
        {
            if (m_PageTextureHolders == null)
            {
                m_PageTextureHolders = new PDFPageTextureHolder[m_PageCount];

                for (int i = 0; i < m_PageCount; ++i)
                {
	                GameObject page = i == 0 
	                    ? m_Internal.PageSample.gameObject 
	                    : Instantiate(m_Internal.PageSample.gameObject);

                    page.transform.SetParent(m_Internal.PageSample.transform.parent, false);
                    page.transform.localScale = Vector3.one;
                    page.transform.localRotation = Quaternion.identity;

                    PDFPageTextureHolder textureHolder = new PDFPageTextureHolder
                    {
	                    PageIndex = i, 
	                    Page = page, 
	                    Viewer = this
                    };

                    m_PageTextureHolders[i] = textureHolder;
                }
            }
        }

        private void Update()
        {
            if (m_DelayedOnEnable)
            {
                m_DelayedOnEnable = false;

                if (m_LoadOnEnable && !m_IsLoaded)
                {
                    LoadDocument();
                }
                else
                {
                    OverlayVisible = true;
                    m_Internal.Overlay.gameObject.SetActive(true);
                    m_Internal.Overlay.alpha = OverlayAlpha;
                }
            }

            ProcessPinchZoom();
        }

        private void ProcessPinchZoom()
        {
            if (m_GraphicRaycaster == null)
            {
                if (m_Canvas == null)
                    CacheCanvas();

                if (m_GraphicRaycaster == null)
                    return;
            }
            
            int validTouchCount = 0;

            if (Input.touchCount >= 1)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        PointerEventData ped = new PointerEventData(null);
                        ped.position = touch.position;
                        List<RaycastResult> results = new List<RaycastResult>();
                        m_GraphicRaycaster.Raycast(ped, results);

                        foreach (RaycastResult result in results)
                        {
                            if (result.gameObject.GetComponentInParent<PDFViewer>() == this)
                            {
                                ++validTouchCount;

                                break;
                            }
                        }
                    }
                }
            }

            if (validTouchCount >= 2)
            {
                if (m_PreviousTouchCount < 2)
                {
                    m_PinchZoomStartZoomFactor = ZoomFactor;

                    ScrollRect scrollRect = m_Internal.Viewport.GetComponent<ScrollRect>();

                    scrollRect.inertia = false;
                    scrollRect.horizontal = false;
                    scrollRect.vertical = false;

                    StopCoroutine(DelayedUnlockScrollRect());
                }

                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                if (m_PreviousTouchCount < 2)
                    m_PinchZoomStartDeltaMag = touchDeltaMag;
                else
                    ZoomFactor = m_PinchZoomStartZoomFactor / (m_PinchZoomStartDeltaMag / touchDeltaMag);
            }
            else if (m_PreviousTouchCount >= 2)
                StartCoroutine(DelayedUnlockScrollRect());

            m_PreviousTouchCount = validTouchCount;
        }

        private IEnumerator DelayedUnlockScrollRect()
        {
            while (Input.touchCount != 0)
                yield return null;

            ScrollRect scrollRect = m_Internal.Viewport.GetComponent<ScrollRect>();

            scrollRect.inertia = true;
            scrollRect.horizontal = true;
            scrollRect.vertical = true;
        }

        private byte[] LoadAssetBytesFromResources(string path)
        {
            string fixedPath = path.Replace(".bytes", "");
            if (fixedPath.StartsWith("./"))
                fixedPath = fixedPath.Substring(2);

            TextAsset pdfAsset = Resources.Load(fixedPath, typeof(TextAsset)) as TextAsset;

            if (pdfAsset != null && pdfAsset.bytes != null && pdfAsset.bytes.Length > 0)
                return pdfAsset.bytes;

            return null;
        }

        private void NotifyCurrentPageChanged(int oldPageIndex, int newPageIndex)
        {
            if (OnCurrentPageChanged != null)
                OnCurrentPageChanged(this, oldPageIndex, newPageIndex);

            m_ThumbnailsViewer.OnCurrentPageChanged(newPageIndex);
        }

        private void NotifyDisabled()
        {
            if (OnDisabled != null)
                OnDisabled(this);
        }

        private void NotifyDocumentLoaded(PDFDocument document)
        {
            EnsureValidPageContainerPosition();

            if (OnDocumentLoaded != null)
                OnDocumentLoaded(this, document);

            m_ThumbnailsViewer.OnDocumentLoaded(document);
            m_BookmarksViewer.OnDocumentLoaded(document);
        }

        private void NotifyDocumentLoadFailed()
        {
            if (OnDocumentLoadFailed != null)
                OnDocumentLoadFailed(this);
        }

        private void NotifyDocumentUnloaded(PDFDocument document)
        {
            if (OnDocumentUnloaded != null)
                OnDocumentUnloaded(this, document);

            m_ThumbnailsViewer.OnDocumentUnloaded();
            m_BookmarksViewer.OnDocumentUnloaded();
        }

        private void NotifyDownloadCancelled()
        {
            if (OnDownloadCancelled != null)
                OnDownloadCancelled(this);
        }

        private void NotifyPasswordCancelled()
        {
            if (OnPasswordCancelled != null)
                OnPasswordCancelled(this);
        }

        private void NotifyZoomChanged(float oldZoom, float newZoom)
        {
            if (OnZoomChanged != null)
                OnZoomChanged(this, oldZoom, newZoom);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (m_UnloadOnDisable && m_IsLoaded)
            {
                if (m_Renderer != null)
                    m_Renderer.Dispose();
                m_Renderer = null;

                CleanUp();
            }

            NotifyDisabled();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_DelayedOnEnable = true;

            if (m_ThumbnailsViewer == null)
                m_ThumbnailsViewer = m_Internal.LeftPanel.ThumbnailsViewer;
            if (m_BookmarksViewer == null)
                m_BookmarksViewer = m_Internal.LeftPanel.Bookmarks.GetComponent<PDFBookmarksViewer>();

            if (!m_ShowBookmarksViewer && m_ShowThumbnailsViewer)
                m_Internal.LeftPanel.OnThumbnailsTabClicked();
            else if (m_ShowBookmarksViewer && !m_ShowThumbnailsViewer)
                m_Internal.LeftPanel.OnBookmarksTabClicked();
            else
                m_Internal.LeftPanel.OnBookmarksTabClicked();

            m_ThumbnailsViewer.DoOnEnable();
            m_BookmarksViewer.DoOnEnable();

#if UNITY_WEBGL
            m_Internal.SearchPanel.gameObject.SetActive(false);

            int c = m_Internal.TopPanel.childCount;
            for (int i = 0; i < c; ++i)
            {
                Transform t = m_Internal.TopPanel.GetChild(i);
                if (t.name == "SearchButton")
                    t.gameObject.SetActive(false);
            }
#endif
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        private void OnLoadingBufferFinished(byte[] buffer)
        {
            m_PendingDocumentBuffer = buffer;

            if (m_PendingDocumentBuffer != null && m_PendingDocumentBuffer.Length > 0)
            {
                if (!TryLoadDocumentWithBuffer(m_PendingDocumentBuffer, m_Password))
                {
                    if (m_FileSource == FileSourceType.Asset)
                    {
                        if (!TryLoadDocumentWithBuffer(m_PendingDocumentBuffer, m_PDFAsset.m_Password))
                        {
                            ShowPasswordDialog();
                        }
                    }
                    else
                        ShowPasswordDialog();
                }
            }
            else
            {
                OverlayVisible = true;
                m_Internal.Overlay.gameObject.SetActive(true);
                m_Internal.Overlay.alpha = OverlayAlpha;
            }
        }
#endif

        private void SetPageCountLabel(int pageIndex, int pageCount)
        {
            if (m_LastSetLabelPageIndex == null || m_LastSetLabelPageIndex.Value != pageIndex
                || m_LastSetLabelPageCount == null || m_LastSetLabelPageCount.Value != pageCount)
			{
                m_Internal.PageCountLabel.text = "(" + pageIndex + " of " + pageCount + ")";

                m_LastSetLabelPageIndex = pageIndex;
                m_LastSetLabelPageCount = pageCount;
            }
        }

        private void SetProgress(float progress)
        {
            RectTransform rectTransform = (RectTransform)m_Internal.ProgressRect.parent.transform;
            if (rectTransform != null)
            {
                m_Internal.ProgressRect.sizeDelta = new Vector2(Mathf.Clamp01(progress) * rectTransform.sizeDelta.x,
                    m_Internal.ProgressRect.sizeDelta.y);
            }
            m_Internal.ProgressLabel.text = (int)(Mathf.Clamp01(progress) * 100) + "%";
        }

        private void SetZoomLabel()
        {
            m_Internal.PageZoomLabel.text = "(" + (int)Mathf.Round(m_ZoomFactor * 100.0f) + "%)";
        }

        private void ShowPasswordDialog()
        {
            OverlayVisible = true;
            m_Internal.Overlay.gameObject.SetActive(true);
            m_Internal.Overlay.alpha = 0.0f;

            m_Internal.PasswordDialog.gameObject.SetActive(true);
        }

        protected override void Start()
        {
            m_StartZoom = m_ZoomToGo;

            m_LinksActionHandler = m_LinksActionHandler ?? new PDFViewerDefaultActionHandler();
            m_BookmarksActionHandler = m_BookmarksActionHandler ?? new PDFViewerDefaultActionHandler();
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        private bool TryLoadDocumentWithBuffer(byte[] buffer, string password)
        {
            m_Document = new PDFDocument(buffer, password);

            return TryLoadWithSpecifiedDocument(m_Document);
        }
#endif

        private bool TryLoadWithSpecifiedDocument(PDFDocument document)
        {
            m_Document = document;

            if (m_Document != null && m_Document.IsValid)
            {
                m_CurrentPageRange = new PDFPageRange();

                m_PageCount = m_Document.GetPageCount();

#if !UNITY_WEBGL || UNITY_EDITOR
                m_NormalPageSizes = new Vector2[m_PageCount];

                for (int i = 0; i < m_NormalPageSizes.Length; ++i)
                    m_NormalPageSizes[i] = m_Document.GetPageSize(i);
#endif

                m_Internal.ScrollRect.scrollSensitivity = m_ScrollSensitivity;

                m_PreviousPageFitting = m_PageFitting;
                AdjustZoomToPageFitting(m_PageFitting, m_NormalPageSizes[0]);
                m_ZoomFactor = m_ZoomToGo;

                ComputePageSizes();
                ComputePageOffsets();

                InstantiatePageTextureHolders();

                m_Internal.PageContainer.sizeDelta = GetDocumentSize();

                SetPageCountLabel(1, m_PageCount);
                SetZoomLabel();

                m_Internal.PageContainer.anchoredPosition = Vector2.zero;

                m_IsLoaded = true;

                UpdatePagesPlacement();

                UpdateScrollBarVisibility();
                EnsureValidPageContainerPosition();

                GoToPage(m_LoadAtPageIndex);

                m_LoadAtPageIndex = 0;

                NotifyDocumentLoaded(m_Document);

                UpdateBookmarksViewerVisibility(m_ShowBookmarksViewer);

                return true;
            }

            OverlayVisible = true;
            m_Internal.Overlay.gameObject.SetActive(true);
            m_Internal.Overlay.alpha = OverlayAlpha;

            m_IsLoaded = false;

            NotifyDocumentLoadFailed();

            return false;
        }

        private void LateUpdate()
        {
            if (m_InvalidPasswordMessageVisisble && m_InvalidPasswordMessageDelay >= 0.0f)
            {
                m_InvalidPasswordMessageDelay = m_InvalidPasswordMessageDelay - Time.deltaTime;

                if (m_InvalidPasswordMessageDelay < 0.0f)
                    m_InvalidPasswordMessageDelay = 0.0f;

                CanvasGroup messageCanvas = m_Internal.InvalidPasswordImage.GetComponent<CanvasGroup>();

                if (m_InvalidPasswordMessageDelay <= 0.0f)
                    messageCanvas.alpha = Mathf.Clamp01(messageCanvas.alpha - Time.deltaTime);

                if (messageCanvas.alpha <= 0.0f)
                {
                    m_InvalidPasswordMessageVisisble = false;
                    messageCanvas.alpha = 1.0f;
                    m_Internal.InvalidPasswordImage.gameObject.SetActive(false);
                }
            }

            if (OverlayVisible && !m_IsLoaded)
            {
                CanvasGroup overlayCanvas = m_Internal.Overlay;

                overlayCanvas.alpha = Mathf.Clamp01(overlayCanvas.alpha + Time.deltaTime * 2.0f);

                if (overlayCanvas.alpha > OverlayAlpha)
                    overlayCanvas.alpha = OverlayAlpha;
            }
            else if (OverlayVisible && m_IsLoaded)
            {
                CanvasGroup overlayCanvas = m_Internal.Overlay;

                overlayCanvas.alpha = Mathf.Clamp01(overlayCanvas.alpha - Time.deltaTime * 2.0f);

                if (overlayCanvas.alpha <= 0.0f)
                {
                    OverlayVisible = false;
                    m_Internal.Overlay.gameObject.SetActive(false);
                }
            }

            if (m_Internal.PasswordDialog.gameObject.activeInHierarchy
                && m_Internal.PasswordInputField.text != ""
                && Input.GetKeyDown("enter"))
            {
                OnPasswordDialogOkButtonClicked();
            }

            if (m_Document == null || !m_Document.IsValid || !m_IsLoaded)
                return;

            UpdateScrollBarVisibility();
            EnsureValidPageContainerPosition();

            if (m_PageFitting != m_PreviousPageFitting)
                AdjustZoomToPageFitting(m_PageFitting, m_NormalPageSizes[0]);

            if (Math.Abs(m_ZoomFactor - m_ZoomToGo) > 0.001f)
            {
	            m_ZoomToGo = Mathf.Clamp(m_ZoomToGo, m_MinZoomFactor, m_MaxZoomFactor);
	            m_ZoomFactor = Mathf.Lerp(m_ZoomFactor, m_ZoomToGo, Time.deltaTime * 15.0f);

                m_UpdateChangeDelay = m_DelayAfterZoomingBeforeUpdate;
            }
            else
            {
	            m_ZoomFactor = m_ZoomToGo;
            }

            bool zoomHasChanged = m_PreviousZoom != 0.0f && Math.Abs(m_PreviousZoom - m_ZoomFactor) > float.Epsilon;

            if (m_PreviousZoom == 0.0f)
            {
                UpdateScrollBarVisibility();
                EnsureValidPageContainerPosition();
            }

            if (zoomHasChanged)
            {
                Vector2 oldDocumentSize = GetDocumentSize();

                ComputePageSizes();
                ComputePageOffsets();
                UpdatePagesPlacement();

                m_Internal.PageContainer.sizeDelta = GetDocumentSize();

                float newDocumentWidthRatio = m_Internal.PageContainer.sizeDelta.x / oldDocumentSize.x;
                float newDocumentHeightRatio = m_Internal.PageContainer.sizeDelta.y / oldDocumentSize.y;

                float deltaOffsetY = (m_Internal.PageContainer.anchoredPosition.y + m_ZoomPosition.y) *
                                     newDocumentHeightRatio - m_Internal.PageContainer.anchoredPosition.y - m_ZoomPosition.y;

                float deltaOffsetX = (m_Internal.PageContainer.anchoredPosition.x + m_ZoomPosition.x) *
                                     newDocumentWidthRatio - m_Internal.PageContainer.anchoredPosition.x - m_ZoomPosition.x;

                m_Internal.PageContainer.anchoredPosition += Vector2.up * deltaOffsetY + Vector2.right * deltaOffsetX;

                UpdateScrollBarVisibility();

                SetZoomLabel();
            }
            else if (Input.touchCount < 2)
            {
                EnsureValidPageContainerPosition();
            }

            PDFPageRange oldPageRange = m_CurrentPageRange;

            m_CurrentPageRange = GetVisiblePageRange();

            if (!m_Internal.PageInputField.isFocused)
            {
                int p = GetMostVisiblePageIndex() + 1;
                m_Internal.PageInputField.text = p.ToString();

                SetPageCountLabel(p, m_PageCount);
            }

            if (!oldPageRange.Equals(m_CurrentPageRange) && m_CurrentPageRange.IsValid
                || zoomHasChanged && m_CurrentPageRange.IsValid)
            {
                float scale = Mathf.Min(m_ZoomToGo, m_MaxZoomFactorTextureQuality);

                PDFPageRange.UpdatePageAgainstRanges(oldPageRange, m_CurrentPageRange, m_Document, m_PageTextureHolders, m_RenderSettings, scale, this, m_NormalPageSizes);
            }

            int mostVisible = GetMostVisiblePageIndex();

            if (m_PreviousMostVisiblePage != mostVisible)
            {
                NotifyCurrentPageChanged(m_PreviousMostVisiblePage, mostVisible);

                m_PreviousMostVisiblePage = GetMostVisiblePageIndex();
            }

            if (!oldPageRange.Equals(m_CurrentPageRange))
            {
                AdjustCurrentSearchResultDisplayed();
            }

            if (Math.Abs(m_PreviousZoomToGo - m_ZoomToGo) > float.Epsilon)
            {
                NotifyZoomChanged(m_PreviousZoomToGo, m_ZoomToGo);
            }

            if (m_UpdateChangeDelay != 0.0f && !zoomHasChanged)
            {
                m_UpdateChangeDelay -= Time.deltaTime;

                if (m_UpdateChangeDelay <= 0.0f)
                {
                    m_UpdateChangeDelay = 0.0f;

                    for (int i = m_CurrentPageRange.m_From; i < m_CurrentPageRange.m_To; ++i)
                    {
#if UNITY_WEBGL
                        m_PageTextureHolders[i].Visible = true;

                        if (m_PageTextureHolders[i].RenderingStarted)
                            continue;

                        int w = (int)(m_NormalPageSizes[i].x * Mathf.Min(m_ZoomToGo, m_MaxZoomFactorTextureQuality));
                        int h = (int)(m_NormalPageSizes[i].y * Mathf.Min(m_ZoomToGo, m_MaxZoomFactorTextureQuality));

                        m_PageTextureHolders[i].RenderingStarted = true;

                        StartCoroutine(UpdatePageRangeTextures(i, w, h));
#else
                        if (m_PageTextureHolders[i].Texture != null)
                        {
                            Texture2D tex = m_PageTextureHolders[i].Texture;
                            m_PageTextureHolders[i].Texture = null;

                            Destroy(tex);
                        }

                        int w = (int)(m_Document.GetPageWidth(i) * Mathf.Min(m_ZoomToGo, m_MaxZoomFactorTextureQuality));
                        int h = (int)(m_Document.GetPageHeight(i) * Mathf.Min(m_ZoomToGo, m_MaxZoomFactorTextureQuality));

                        if (m_Renderer == null)
                            m_Renderer = new PDFRenderer();

                        using (PDFPage page = m_Document.GetPage(i))
                        {
	                        Texture2D newTex = m_Renderer.RenderPageToTexture(page, w, h, this, m_RenderSettings);

	                        m_PageTextureHolders[i].Texture = newTex;
                        }
#endif
                    }
                }
            }

            m_PreviousZoom = m_ZoomFactor;
            m_PreviousZoomToGo = m_ZoomToGo;
            m_PreviousPageFitting = m_PageFitting;


            if (m_ThumbnailsViewer.gameObject.activeInHierarchy)
                m_ThumbnailsViewer.DoUpdate();
            if (m_BookmarksViewer.gameObject.activeInHierarchy)
                m_BookmarksViewer.DoUpdate();
        }

#if UNITY_WEBGL
        private IEnumerator UpdatePageRangeTextures(int pageIndex, int w, int h)
        {
            PDFJS_Promise<PDFPage> pagePromise = m_Document.GetPageAsync(pageIndex);

            while (!pagePromise.HasFinished)
                yield return null;

            if (pagePromise.HasSucceeded)
            {
                PDFJS_Promise<Texture2D> renderPromise = PDFRenderer.RenderPageToTextureAsync(pagePromise.Result, w, h);

                m_PageTextureHolders[pageIndex].RenderingPromise = renderPromise;

                while (!renderPromise.HasFinished)
                    yield return null;

                m_PageTextureHolders[pageIndex].RenderingPromise = null;
                m_PageTextureHolders[pageIndex].RenderingStarted = false;

                if (renderPromise.HasSucceeded)
                {
                    if (m_PageTextureHolders[pageIndex].Texture != null && m_PageTextureHolders[pageIndex].Texture != renderPromise.Result)
                    {
                        Destroy(m_PageTextureHolders[pageIndex].Texture);
                        m_PageTextureHolders[pageIndex].Texture = null;
                    }

                    if (m_PageTextureHolders[pageIndex].Visible)
                    {
                        m_PageTextureHolders[pageIndex].Texture = renderPromise.Result;
                    }
                    else
                    {
                        Destroy(renderPromise.Result);
                        renderPromise.Result = null;
                    }
                }
            }
            else
            {
                m_PageTextureHolders[pageIndex].RenderingPromise = null;
                m_PageTextureHolders[pageIndex].RenderingStarted = false;
            }
        }
#endif

        public Vector2[] GetCachedNormalPageSizes()
        {
            return m_NormalPageSizes;
        }

        private void UpdatePagesPlacement()
        {
            if (m_PageTextureHolders == null || m_PageSizes == null)
                return;

            foreach (PDFPageTextureHolder holder in m_PageTextureHolders)
            {
                RectTransform holderRectTransform = (RectTransform)holder.Page.transform;

                holderRectTransform.sizeDelta = m_PageSizes[holder.PageIndex];
                holder.RefreshTexture();

                Vector3 newPosition = holderRectTransform.anchoredPosition3D;
                newPosition.x = 0;
                newPosition.y = -m_PageOffsets[holder.PageIndex];
                newPosition.z = 0;
                holderRectTransform.anchoredPosition3D = newPosition;
            }
        }

        private void UpdateScrollBarVisibility()
        {
            bool vScrollVisible = true;
            bool hScrollVisible = true;

            if (Application.isPlaying)
            {
                Vector2 pageContainerSizeDelta = m_Internal.PageContainer.sizeDelta;
                Vector2 viewportRectSize = m_Internal.Viewport.rect.size;

                vScrollVisible = pageContainerSizeDelta.y > viewportRectSize.y;
                hScrollVisible = pageContainerSizeDelta.x > viewportRectSize.x;
            }

            vScrollVisible = vScrollVisible && m_ShowVerticalScrollBar;
            hScrollVisible = hScrollVisible && m_ShowHorizontalScrollBar;

            if (m_ViewportScrollRect == null)
                m_ViewportScrollRect = m_Internal.Viewport.GetComponent<ScrollRect>();

            if (m_HorizontalScrollBar == null)
                m_HorizontalScrollBar = m_Internal.HorizontalScrollBar.GetComponent<Scrollbar>();

            if (m_VerticalScrollBar == null)
                m_VerticalScrollBar = m_Internal.VerticalScrollBar.GetComponent<Scrollbar>();

            if (!hScrollVisible)
            {
                m_Internal.Viewport.offsetMin = new Vector2(m_Internal.Viewport.offsetMin.x, 0.0f);

                if (m_ViewportScrollRect.horizontalScrollbar != null)
				{
                    m_ViewportScrollRect.horizontalScrollbar = null;
                    m_Internal.HorizontalScrollBar.gameObject.SetActive(false);
                }
            }
            else
            {
                m_Internal.Viewport.offsetMin = new Vector2(m_Internal.Viewport.offsetMin.x, 20.0f);

                if (m_ViewportScrollRect.horizontalScrollbar != m_HorizontalScrollBar)
				{
                    m_ViewportScrollRect.horizontalScrollbar = m_HorizontalScrollBar;
                    m_Internal.HorizontalScrollBar.gameObject.SetActive(true);
                }
            }

            if (!vScrollVisible)
            {
                m_Internal.Viewport.offsetMax = new Vector2(0.0f, m_Internal.Viewport.offsetMax.y);

                if (m_ViewportScrollRect.verticalScrollbar != null)
				{
                    m_ViewportScrollRect.verticalScrollbar = null;
                    m_Internal.VerticalScrollBar.gameObject.SetActive(false);
                }
            }
            else
            {
                m_Internal.Viewport.offsetMax = new Vector2(-20.0f, m_Internal.Viewport.offsetMax.y);

                if (m_ViewportScrollRect.verticalScrollbar != m_VerticalScrollBar)
				{
                    m_ViewportScrollRect.verticalScrollbar = m_VerticalScrollBar;
                    m_Internal.VerticalScrollBar.gameObject.SetActive(true);
                }
            }

            if (hScrollVisible && vScrollVisible)
            {
                m_Internal.VerticalScrollBar.offsetMin = new Vector2(m_Internal.VerticalScrollBar.offsetMin.x, 19.0f);
                m_Internal.HorizontalScrollBar.offsetMax = new Vector2(-19.0f, m_Internal.HorizontalScrollBar.offsetMax.y);

                if (!m_Internal.ScrollCorner.gameObject.activeInHierarchy)
                    m_Internal.ScrollCorner.gameObject.SetActive(true);
            }
            else if (!hScrollVisible)
            {
                m_Internal.VerticalScrollBar.offsetMin = new Vector2(m_Internal.VerticalScrollBar.offsetMin.x, 0.0f);

                if (m_Internal.ScrollCorner.gameObject.activeInHierarchy)
                    m_Internal.ScrollCorner.gameObject.SetActive(false);
            }
            else
            {
                m_Internal.HorizontalScrollBar.offsetMax = new Vector2(0.0f, m_Internal.HorizontalScrollBar.offsetMax.y);

                if (m_Internal.ScrollCorner.gameObject.activeInHierarchy)
                    m_Internal.ScrollCorner.gameObject.SetActive(false);
            }
        }

        private void ZoomCommon(Vector2 zoomPosition, bool zoomIn, bool useSpecificZoom = false, float specificZoom = 1.0f)
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            m_ZoomPosition = zoomPosition;

            if (useSpecificZoom)
            {
                m_ZoomToGo = specificZoom;
            }
            else
            {
                float step = m_ZoomStep;

                if (m_ZoomToGo >= 2.0f)
                    step *= 2.0f;

                if (m_ZoomToGo >= 4.0f)
                    step *= 2.0f;

                float epsilon = m_ZoomStep * 0.125f;

                if (zoomIn)
                {
                    if (!Mathf.Approximately(Mathf.Floor(m_ZoomToGo * (1 / step)), m_ZoomToGo * (1 / step))
                        && m_ZoomToGo * (1 / step) <= Mathf.Floor(m_ZoomToGo * (1 / step)))
                    {
                        m_ZoomToGo = Mathf.Floor(m_ZoomToGo * (1 / step)) * step;
                    }

                    m_ZoomToGo = Mathf.Clamp(Mathf.Floor((m_ZoomToGo + step) * (1 / step) + epsilon) * step, m_MinZoomFactor, m_MaxZoomFactor);
                }
                else
                {
                    if (!Mathf.Approximately(Mathf.Floor(m_ZoomToGo * (1 / step)), m_ZoomToGo * (1 / step))
                        && m_ZoomToGo * (1 / step) >= Mathf.Floor(m_ZoomToGo * (1 / step)))
                    {
                        m_ZoomToGo = Mathf.Floor(m_ZoomToGo * (1 / step)) * step;
                    }

                    m_ZoomToGo = Mathf.Clamp(Mathf.Floor((m_ZoomToGo - step) * (1 / step) + epsilon) * step, m_MinZoomFactor, m_MaxZoomFactor);
                }
            }

            m_PageFitting = PageFittingType.Zoom;
        }

        public Vector2 GetDevicePageSize(int pageIndex)
        {
            return m_PageSizes[pageIndex];
        }

        public IPDFDeviceActionHandler GetBookmarksActionHandler()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return null;
#else
            return m_BookmarksActionHandler;
#endif
        }

        public IPDFDeviceActionHandler GetLinksActionHandler()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return null;
#else
            return m_LinksActionHandler;
#endif
        }

        public IList<PDFColoredRect> GetBackgroundColoredRectList(PDFPage page)
        {
#if !UNITY_WEBGL
            if (m_SearchResults != null && m_SearchResults[page.PageIndex] != null &&
                m_SearchResults[page.PageIndex].Count > 0)
            {
                using (PDFTextPage textPage = page.GetTextPage())
                {
                    List<PDFColoredRect> coloredRectList = new List<PDFColoredRect>();

                    foreach (PDFSearchResult result in m_SearchResults[page.PageIndex])
                    {
                        int pageRectCount = textPage.CountRects(result.StartIndex, result.Count);

                        for (int j = 0; j < pageRectCount; ++j)
                        {
                            Rect rect = textPage.GetRect(j);
                            rect = new Rect(
                                rect.xMin - m_SearchResultPadding.x,
                                rect.yMin + m_SearchResultPadding.y,
                                rect.width + 2 * m_SearchResultPadding.x,
                                rect.height + 2 * m_SearchResultPadding.y);
                            coloredRectList.Add(new PDFColoredRect(rect, m_SearchResultColor));
                        }
                    }

                    return coloredRectList;
                }
            }
#endif

            return null;
        }

        public void ZoomOnParagraph(PDFViewerPage viewerPage, Rect pageRect)
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

#if !UNITY_WEBGL
            Vector3[] pageCorners = new Vector3[4];

            RectTransform viewerPageTransform = (RectTransform)viewerPage.transform;

            viewerPageTransform.GetWorldCorners(pageCorners);
            Vector2 min = pageCorners[0];
            Vector2 max = pageCorners[0];
            for (int i = 1; i < 4; ++i)
            {
                if (pageCorners[i].x < min.x)
                    min.x = pageCorners[i].x;
                if (pageCorners[i].y < min.y)
                    min.y = pageCorners[i].y;
                if (pageCorners[i].x > max.x)
                    max.x = pageCorners[i].x;
                if (pageCorners[i].y > max.y)
                    max.y = pageCorners[i].y;
            }

            Vector2 devicePageSize = viewerPageTransform.sizeDelta;

            using (PDFPage page  = m_Document.GetPage(viewerPage.PageIndex))
			{
                Rect deviceRect = page.ConvertPageRectToDeviceRect(pageRect, devicePageSize);

                float deviceRectCenterPosition = deviceRect.max.y + (deviceRect.min - deviceRect.max).y * 0.5f;

                m_Internal.PageContainer.anchoredPosition = new Vector2(
                    m_Internal.PageContainer.anchoredPosition.x,
                    m_PageOffsets[page.PageIndex]
                    - m_PageSizes[page.PageIndex].y * 0.5f
                    + (m_PageSizes[page.PageIndex].y - deviceRectCenterPosition)
                    - m_Internal.Viewport.rect.size.y * 0.5f);
            }

            if (m_ZoomToGo < m_ParagraphZoomFactor)
                ZoomCommon(new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f), true, true, m_ParagraphZoomFactor);
#endif
        }

        protected override void OnTransformParentChanged()
        {
            base.OnTransformParentChanged();

            if (gameObject != null)
                m_Canvas = null;
        }

        private void CacheCanvas()
        {
            if (gameObject == null)
                return;

            gameObject.GetComponentsInParent(false, m_CanvasList);

            if (m_CanvasList.Count > 0)
            {
                // Find the first active and enabled canvas.
                for (int i = 0; i < m_CanvasList.Count; ++i)
                {
                    if (m_CanvasList[i].isActiveAndEnabled)
                    {
                        m_Canvas = m_CanvasList[i];
                        m_GraphicRaycaster = m_Canvas.GetComponent<GraphicRaycaster>();
                        break;
                    }
                }
            }
            else
            {
                m_Canvas = null;
                m_GraphicRaycaster = null;
            }

            m_CanvasList.Clear();
        }
    }
}