using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharp.MinimalExample.WinForms.Handlers
{
    public class RenderProcessMessageHandler : IRenderProcessMessageHandler
    {
        public void OnContextCreated(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            try
            {
                const string script =
                        @"function sleep (time) {
                            return new Promise((resolve) => setTimeout(resolve, time));
                        }  
                        document.addEventListener('DOMContentLoaded', function(){
                            sleep(5000).then(() => {
                                var fileLinks=document.getElementsByClassName('file-link');
                                for (var i = 0; i < fileLinks.length; i++){
                                    links.addLink(String(fileLinks[i].innerHTML),String(fileLinks[i].innerText),String(fileLinks[i].href));
                                }
                                links.onFetchLinksComplete();
                            })
                        });";
                frame.ExecuteJavaScriptAsync(script);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void OnContextReleased(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {

        }

        public void OnFocusedNodeChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IDomNode node)
        {

        }

        public void OnUncaughtException(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, JavascriptException exception)
        {

        }
    }
}
