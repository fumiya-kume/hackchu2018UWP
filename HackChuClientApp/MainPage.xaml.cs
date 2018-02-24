using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.FaceAnalysis;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace HackChuClientApp
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public DispatcherTimer timer { get; set; } = new DispatcherTimer();
        public MediaCapture mediaCapture { get; set; } = new MediaCapture();
        public IFaceServiceClient FaceServiceClient { get; set; } = new FaceServiceClient("e32173e705514cc2ac2ef2b615ed7131", "https://southcentralus.api.cognitive.microsoft.com/face/v1.0");

        public MainPage()
        {
            this.InitializeComponent();

            InitAsync();

            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += async (sender, o) =>
            {

                var properties = mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview) as VideoEncodingProperties;
                if (properties == null) return;

                //Jpeg形式でガメラの最大解像度で取得する。
                var property = ImageEncodingProperties.CreateJpeg();
                property.Width = properties.Width;
                property.Height = properties.Height;


                using (var RandomStream = new InMemoryRandomAccessStream())
                {
                    await mediaCapture.CapturePhotoToStreamAsync(property, RandomStream);
                    RandomStream.Seek(0);

                    IEnumerable<FaceAttributeType> faceAttributes = new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Emotion, FaceAttributeType.Glasses, FaceAttributeType.Hair };

                    var detectAsync = await FaceServiceClient.DetectAsync(RandomStream.AsStreamForRead(),returnFaceAttributes:faceAttributes);
                    if(detectAsync.Count() == 0)
                    {
                        return;
                    }

                    if(detectAsync.Count() == 1)
                    {
                        timer.Stop();
                        Frame.Navigate(typeof(BrowserPage));
                    }
                    var userEmotion = detectAsync[0].FaceAttributes.Emotion;
                    FaceResultText.Text = $"{detectAsync.Count()}人, {userEmotion.Happiness}ハピネス度, {userEmotion.Neutral}虚無おじさん度";
                }

            };
        }

        public async Task InitAsync()
        {
            await mediaCapture.InitializeAsync();
            captureElement.Source = mediaCapture;
            await mediaCapture.StartPreviewAsync();
            timer.Start();
        }
    }
}
