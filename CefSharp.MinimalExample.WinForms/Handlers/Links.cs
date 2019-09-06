using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.WinForms.Handlers
{
    public class Links
    {
        public event EventHandler<List<LinkInfo>> FetchListComplete;

        private readonly List<LinkInfo> linkInfos = new List<LinkInfo>();
        public void AddLink(string innerHtml, string innerText, string link)
        {
            LinkInfo linkInfo = new LinkInfo(innerHtml, innerText, link);
            linkInfos.Add(linkInfo);
        }

        public void OnFetchLinksComplete()
        {
            FetchListComplete?.Invoke(this, linkInfos);
        }

        public class LinkInfo
        {
            public string innerHtml;
            public string innerText;
            public string href;

            public LinkInfo(string innerHtml, string innerText, string href)
            {
                this.innerHtml = innerHtml;
                this.innerText = innerText;
                this.href = href;
            }
        }
    }
}
