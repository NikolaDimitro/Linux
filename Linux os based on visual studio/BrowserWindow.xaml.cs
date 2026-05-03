using Microsoft.Web.WebView2.Wpf;
using System;
using System.Windows;
using System.Windows.Input;

namespace LinuxMintOS
{
    public partial class BrowserWindow : Window
    {
        private WebView2 webView;

        public BrowserWindow()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            try
            {
                webView = new WebView2();
                WebViewContainer.Children.Add(webView);

                await webView.EnsureCoreWebView2Async(null);
                webView.CoreWebView2.Navigate("https://www.google.com");
                AddressBar.Text = "https://www.google.com";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"WebView2 initialization failed: {ex.Message}\n\nPlease ensure WebView2 Runtime is installed.",
                    "Browser Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Navigate_Click(object sender, RoutedEventArgs e)
        {
            NavigateToUrl(AddressBar.Text);
        }

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                NavigateToUrl(AddressBar.Text);
            }
        }

        private void NavigateToUrl(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return;

                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "https://" + url;
                }
                webView.CoreWebView2.Navigate(url);
                AddressBar.Text = url;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Navigation failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (webView.CanGoBack)
                webView.GoBack();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (webView.CanGoForward)
                webView.GoForward();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            webView.Reload();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            webView.CoreWebView2.Navigate("https://www.google.com");
            AddressBar.Text = "https://www.google.com";
        }
    }
}