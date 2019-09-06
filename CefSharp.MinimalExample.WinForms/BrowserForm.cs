// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.MinimalExample.WinForms.Controls;
using CefSharp.MinimalExample.WinForms.Handlers;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CefSharp.MinimalExample.WinForms
{
    public partial class BrowserForm : Form
    {
        private readonly ChromiumWebBrowser browser;

        private List<Links.LinkInfo> links;

        public BrowserForm()
        {
            InitializeComponent();

            Text = "CefSharp";
            WindowState = FormWindowState.Maximized;

            browser = new ChromiumWebBrowser("https://wixlabs---dropbox-folder.appspot.com/index?instance=lp5CbqBbK6JUFzCW2hXENEgT4Jn0Q-U1-lIAgEbjeio.eyJpbnN0YW5jZUlkIjoiYjNiNzk5YjktNjE5MS00ZDM0LTg3ZGQtYjY2MzI1NWEwMDNhIiwiYXBwRGVmSWQiOiIxNDkyNDg2NC01NmQ1LWI5NGItMDYwZi1jZDU3YmQxNmNjMjYiLCJzaWduRGF0ZSI6IjIwMTgtMDEtMjJUMTg6Mzk6MjkuNjAwWiIsInVpZCI6bnVsbCwidmVuZG9yUHJvZHVjdElkIjpudWxsLCJkZW1vTW9kZSI6ZmFsc2V9&target=_top&width=728&compId=comp-j6bjhny1&viewMode=viewer-seo")
            {
                Dock = DockStyle.Fill,
            };

            DownloadHandler downloadHandler = new DownloadHandler();
            browser.DownloadHandler = downloadHandler;
            downloadHandler.OnBeforeDownloadFired += DownloadHandler_OnBeforeDownloadFired;
            downloadHandler.OnDownloadUpdatedFired += DownloadHandler_OnDownloadUpdatedFired;

            Links links = new Links();
            //browser.JavascriptObjectRepository.Register("links", links, true);
            browser.RegisterJsObject("links", links);
            links.FetchListComplete += Links_FetchListComplete; ;

            browser.RenderProcessMessageHandler = new RenderProcessMessageHandler();

            toolStripContainer.ContentPanel.Controls.Add(browser);

            //browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            //browser.LoadingStateChanged += OnLoadingStateChanged;
            //browser.ConsoleMessage += OnBrowserConsoleMessage;
            //browser.StatusMessage += OnBrowserStatusMessage;
            //browser.TitleChanged += OnBrowserTitleChanged;
            //browser.AddressChanged += OnBrowserAddressChanged;

            //var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            //var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}, Environment: {3}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion, bitness);
            //DisplayOutput(version);
        }

        private void Links_FetchListComplete(object sender, List<Links.LinkInfo> e)
        {
            links = e;//This far it is working well.
        }

        private void DownloadHandler_OnDownloadUpdatedFired(object sender, DownloadItem e)
        {
            
        }

        private void DownloadHandler_OnBeforeDownloadFired(object sender, DownloadItem e)
        {
            e.FullPath = Path.Combine("D:\\Tests", e.SuggestedFileName);//This doesn't get triggered.
        }

        //Following commented code seems to interfere with the execution of JavaScript when uncommented.
        //private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
        //{
        //    var b = ((ChromiumWebBrowser)sender);

        //    this.InvokeOnUiThreadIfRequired(() => b.Focus());
        //}

        //private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
        //{
        //    DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        //}

        //private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args)
        //{
        //    this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
        //}

        //private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        //{
        //    SetCanGoBack(args.CanGoBack);
        //    SetCanGoForward(args.CanGoForward);

        //    this.InvokeOnUiThreadIfRequired(() => SetIsLoading(!args.CanReload));
        //}

        //private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
        //{
        //    this.InvokeOnUiThreadIfRequired(() => Text = args.Title);
        //}

        //private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        //{
        //    this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.Address);
        //}

        //private void SetCanGoBack(bool canGoBack)
        //{
        //    this.InvokeOnUiThreadIfRequired(() => backButton.Enabled = canGoBack);
        //}

        //private void SetCanGoForward(bool canGoForward)
        //{
        //    this.InvokeOnUiThreadIfRequired(() => forwardButton.Enabled = canGoForward);
        //}

        //private void SetIsLoading(bool isLoading)
        //{
        //    goButton.Text = isLoading ?
        //        "Stop" :
        //        "Go";
        //    goButton.Image = isLoading ?
        //        Properties.Resources.nav_plain_red :
        //        Properties.Resources.nav_plain_green;

        //    HandleToolStripLayout();
        //}

        //public void DisplayOutput(string output)
        //{
        //    this.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
        //}

        private void HandleToolStripLayout(object sender, LayoutEventArgs e)
        {
            HandleToolStripLayout();
        }

        private void HandleToolStripLayout()
        {
            var width = toolStrip1.Width;
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (item != urlTextBox)
                {
                    width -= item.Width - item.Margin.Horizontal;
                }
            }
            urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal - 18);
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            browser.Dispose();
            Cef.Shutdown();
            Close();
        }

        private void GoButtonClick(object sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            browser.Back();
        }

        private void ForwardButtonClick(object sender, EventArgs e)
        {
            browser.Forward();
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            LoadUrl(urlTextBox.Text);
        }

        private void LoadUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                browser.Load(url);
            }
        }

        private void ShowDevToolsMenuItemClick(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }
    }
}
