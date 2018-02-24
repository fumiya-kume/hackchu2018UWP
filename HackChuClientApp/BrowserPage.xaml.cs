using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace HackChuClientApp
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class BrowserPage : Page
    {
        public BrowserPage()
        {
            this.InitializeComponent();

            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            Browser.Navigate(new Uri("https://yahoo.co.jp/"));
        }

        private void Browser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if(args.Uri.AbsoluteUri != "https://www.yahoo.co.jp/")
                {
                new Windows.ApplicationModel.DataTransfer.DataPackage().SetText(args.Uri.ToString());
                new MessageDialog(args.Uri.ToString()).ShowAsync();
            }
        }
    }
}
