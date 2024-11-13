using System;
using System.IO;

namespace Paroxe.PdfRenderer
{
#if !UNITY_WEBGL
    /// <summary>
    /// Provides default action handling implementation.
    /// </summary>
    public static class PDFActionHandlerHelper
    {
        public static void ExecuteAction(IPDFDeviceActionHandler actionHandler, IPDFDevice device, PDFAction action)
        {
            if (actionHandler == null)
                throw new ArgumentNullException("actionHandler");
            if (device == null)
                throw new ArgumentNullException("device");
            if (action == null)
                throw new ArgumentNullException("action");

            PDFAction.ActionType type = action.GetActionType();

            switch (type)
            {
                case PDFAction.ActionType.Unsupported:
                    actionHandler.HandleUnsupportedAction(device);
                    break;

                case PDFAction.ActionType.GoTo:
                    PDFDest dest = action.GetDest();
                    actionHandler.HandleGotoAction(device, dest.PageIndex);
                    break;

                case PDFAction.ActionType.RemoteGoTo:
                    string resolvedFilePath = actionHandler.HandleRemoteGotoActionPathResolving(device, action.GetFilePath());

                    if (File.Exists(resolvedFilePath))
                    {
                        string password = actionHandler.HandleRemoteGotoActionPasswordResolving(device, resolvedFilePath);

                        PDFDocument newDocument = new PDFDocument(resolvedFilePath, password);

                        if (newDocument.IsValid)
                        {
                            actionHandler.HandleRemoteGotoActionResolved(device, newDocument, action.GetDest().PageIndex);
                        }
                        else
                        {
                            actionHandler.HandleRemoteGotoActionUnresolved(device, resolvedFilePath);
                        }
                    }
                    else
                    {
                        actionHandler.HandleRemoteGotoActionUnresolved(device, resolvedFilePath);
                    }

                    break;

                case PDFAction.ActionType.Uri:
                    actionHandler.HandleUriAction(device, action.GetURIPath());
                    break;

                case PDFAction.ActionType.Launch:
                    actionHandler.HandleLaunchAction(device, action.GetFilePath());
                    break;
            }
        }

        public static void ExecuteBookmarkAction(PDFBookmark bookmark, IPDFDevice device)
        {
            if (bookmark == null)
                throw new ArgumentNullException("bookmark");
            if (device == null)
                throw new ArgumentNullException("device");

            if (device.BookmarksActionHandler != null)
            {
                PDFDest dest = bookmark.GetDest();

                if (dest != null)
                {
                    device.BookmarksActionHandler.HandleGotoAction(device, dest.PageIndex);
                }
                else
                {
                    PDFAction action = bookmark.GetAction();

                    if (action != null)
                        ExecuteAction(device.BookmarksActionHandler, device, action);
                }
            }
        }

        public static void ExecuteLinkAction(PDFLink link, IPDFDevice device)
        {
	        if (link == null)
		        throw new ArgumentNullException("link");
	        if (device == null)
		        throw new ArgumentNullException("device");

            if (device.LinksActionHandler != null)
            {
                PDFDest dest = link.GetDest();

                if (dest != null)
                {
                    device.LinksActionHandler.HandleGotoAction(device, dest.PageIndex);
                }
                else
                {
                    PDFAction action = link.GetAction();

                    if (action != null)
                        ExecuteAction(device.LinksActionHandler, device, action);
                }
            }
        }
    }
#endif
}