using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Paroxe.PdfRenderer.Examples
{
    public class API_Usage : MonoBehaviour
    {
#if !UNITY_WEBGL
        private IEnumerator Start()
        {
	        // UnityWebRequest or WWW can be use instead of PDFWebRequest.
	        using (PDFWebRequest www = new PDFWebRequest("https://www.dropbox.com/s/tssavtnvaym2t6b/DocumentationEN.pdf?raw=1"))
	        {
		        www.SendWebRequest();

		        Debug.Log("Downloading document...");

		        yield return www;

		        if (!string.IsNullOrEmpty(www.error))
		        {
					Debug.LogError(www.error);

			        yield break;
				}

		        // You can use using or call .Dispose manually on the document
		        // when you don't need it anymore. Or you can let the garbage collector
		        // free the memory for you.
		        using (PDFDocument document = new PDFDocument(www.bytes, ""))
		        {
			        TestAPIs(document);
		        }
	        }
        }

        private void TestAPIs(PDFDocument document)
        {
	        Debug.Log("Page count: " + document.GetPageCount());

	        TextPageAPI(document);
	        SearchAPI(document);
	        BookmarkAPI(document);
	        WebLinksAPI(document);
        }

        private void TextPageAPI(PDFDocument document)
        {
            Debug.Log("TextPage API-----------------------");

            using (PDFPage page = document.GetPage(1))
            {
	            Debug.Log("Page size: " + page.GetPageSize());

	            using (PDFTextPage textPage = page.GetTextPage())
	            {
		            int countChars = textPage.CountChars();
		            Debug.Log("Page chars count: " + countChars);

		            string str = textPage.GetText(0, countChars);
		            Debug.Log("Page text: " + str);

		            int rectCount = textPage.CountRects(0, countChars);
		            Debug.Log("Rect count: " + rectCount);

		            string boundedText = textPage.GetBoundedText(0, 0, page.GetPageSize().x, page.GetPageSize().y * 0.5f, 2000);
		            Debug.Log("Bounded text: " + boundedText);
                }
            }
        }

        private void SearchAPI(PDFDocument document)
        {
            Debug.Log("Search API-------------------------");

            using (PDFPage page = document.GetPage(1))
            {
	            using (PDFTextPage textPage = page.GetTextPage())
	            {
		            IList<PDFSearchResult> results = textPage.Search("pdf");

		            Debug.Log("Search results count: " + results.Count);

		            if (results.Count > 0)
		            {
			            Debug.Log("First result char index: " + results[0].StartIndex + " and chars count: " + results[0].Count);

			            // Get all rects corresponding to the first search result
			            int rectsCount = document.GetPage(1).GetTextPage().CountRects(results[0].StartIndex, results[0].Count);
			            Debug.Log("Search result rects count: " + rectsCount);
					}
	            }
            }
        }

        private void BookmarkAPI(PDFDocument document)
        {
            Debug.Log("Bookmark API-----------------------");

            PDFBookmark rootBookmark = document.GetRootBookmark();
            OutputBookmarks(rootBookmark, 0);
        }

        private void OutputBookmarks(PDFBookmark bookmark, int indent)
        {
            string line = "";
            for (int i = 0; i < indent; ++i)
                line += "    ";
            line += bookmark.GetTitle();
            Debug.Log(line);

            foreach (PDFBookmark child in bookmark.EnumerateChildrenBookmarks())
            {
                OutputBookmarks(child, indent + 1);
            }
        }

        private void WebLinksAPI(PDFDocument document)
        {
	        Debug.Log("WebLinks API-----------------------");

            using (PDFPage page = document.GetPage(0))
	        {
		        using (PDFTextPage textPage = page.GetTextPage())
		        {
			        using (PDFPageWebLinks webLinks = textPage.GetPageWebLinks())
			        {
				        int linksCount = webLinks.LinksCount;

                        Debug.Log("WebLinks count on first page: " + linksCount);

                        for (int i = 0; i < linksCount; ++i)
                        {
	                        string url = webLinks.GetLinkUrl(i);

                            Debug.Log("Link #" + i + " url: " + url);

                            int rectsCount = webLinks.CountLinkRects(i);

                            Debug.Log("Link #" + i + " rects count: " + rectsCount);

                            for (int r = 0; r < rectsCount; ++r)
                            {
	                            Rect linkRect = webLinks.GetLinkRect(i, r);

	                            Debug.Log("Link #" + i + " rect: " + linkRect);

	                            Rect linkRectOnDeviceWithZoom = page.ConvertPageRectToDeviceRect(linkRect, page.GetPageSize(1.5f));

	                            Debug.Log("Link #" + i + " rect on device at zoom 1.5x: " + linkRectOnDeviceWithZoom);
                            }
                        }
			        }
		        }
	        }
        }
#endif
    }
}
