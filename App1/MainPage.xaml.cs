using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Microsoft.ProjectOxford.Face;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace App1
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public DispatcherTimer timer { get; set; } = new DispatcherTimer();
        public MediaCapture mediaCapture { get; set; } = new MediaCapture();
        public IFaceServiceClient FaceServiceClient { get; set; } = new FaceServiceClient("");

        public MainPage()
        {
            this.InitializeComponent();

            InitAsync();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();

            timer.Tick += async (sender, o) =>
            {
                using (var imageStream = new InMemoryRandomAccessStream())
                {
                    await mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreatePng(), imageStream);

                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(imageStream);

                    var detectAsync = await FaceServiceClient.DetectAsync(imageStream.AsStream());

                    detectAsync.Count();
                }
            };
        }

        public async Task InitAsync()
        {
            await mediaCapture.InitializeAsync();
            CaptureElement.Source = mediaCapture;
            await mediaCapture.StartPreviewAsync();

        }
    }
}
