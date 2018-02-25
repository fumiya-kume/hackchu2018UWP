using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var userName = e.Parameter as string;
            Debug.WriteLine(userName);
            base.OnNavigatedTo(e);

            if (!userName.Contains("Ku"))
            {
                Browser.Navigate(new Uri("https://hackchu2018.azurewebsites.net/chukyo.html"));
            }
            else
            {
                Browser.Navigate(new Uri("https://hackchu2018.azurewebsites.net/charatest2.html"));
            }
        }

        public BrowserPage()
        {
            this.InitializeComponent();

            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            
        }

        private void Browser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.Uri.AbsoluteUri.Contains("End"))
            {
                //Task.Delay(TimeSpan.FromSeconds(3));
                Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
