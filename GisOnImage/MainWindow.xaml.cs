using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Brushes = System.Windows.Media.Brushes;

namespace GisOnImage
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public void AddEdge(object sender, MouseButtonEventArgs e)
        {
            Path pathCross = new Path();
            float CrossLengt = Convert.ToByte(SliderCrossLength.Value);
            UIElement Click = new UIElement();
            if (myCanvas.Children.Contains((UIElement)e.OriginalSource) != e.OriginalSource.Equals(myImage))
            {
                myCanvas.Children.Remove((UIElement)e.OriginalSource);
            }
            else
            {
                    Random rnd = new Random();
                    double alpha = 0.785398 - 0.005 * rnd.Next(-50, 50),
                           beta = 0.785398 - 0.005 * rnd.Next(-50, 50);

                    // длина стрелок крестика
                    float L = CrossLengt,
                          l1 = L + rnd.Next(-2, 2),
                          l2 = L + rnd.Next(-2, 2),
                          l3 = L + rnd.Next(-2, 2),
                          l4 = L + rnd.Next(-2, 2);

                    // координаты вершин крестика
                    float x1 = (float)(l1 * Math.Sin(alpha)),
                          x2 = (float)(l2 * Math.Sin(beta)),
                          x3 = (float)(l3 * Math.Sin(beta)),
                          x4 = (float)(l4 * Math.Sin(alpha)),
                          y1 = (float)(l1 * Math.Cos(alpha)),
                          y2 = (float)(l2 * Math.Cos(beta)),
                          y3 = (float)(l3 * Math.Cos(beta)),
                          y4 = (float)(l4 * Math.Cos(alpha));

                    Point crossPosition = new Point(e.GetPosition(myCanvas).X, e.GetPosition(myCanvas).Y);
                    Point crossLU = new Point(crossPosition.X - x1, crossPosition.Y - y1);
                    Point crossRU = new Point(crossPosition.X + x2, crossPosition.Y - y2);
                    Point crossLD = new Point(crossPosition.X - x3, crossPosition.Y + y3);
                    Point crossRD = new Point(crossPosition.X + x4, crossPosition.Y + y4);


                    LineGeometry line1 = new LineGeometry(crossPosition, crossLU),
                                 line2 = new LineGeometry(crossPosition, crossRD),
                                 line3 = new LineGeometry(crossPosition, crossLD),
                                 line4 = new LineGeometry(crossPosition, crossRU);


                    GeometryGroup gg = new GeometryGroup();
                    gg.Children.Add(line1);
                    gg.Children.Add(line2);
                    gg.Children.Add(line3);
                    gg.Children.Add(line4);

                    pathCross.Stroke = Brushes.Red;
                    pathCross.StrokeThickness = SliderCrossThick.Value;
                    pathCross.Data = gg;
                    myCanvas.Children.Add(pathCross);

            }
        }
        private void LoadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter =
                "Image files|*.bmp;*.jpg;*.gif;*.png;*.tiff|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                myImage.Source = new BitmapImage(new Uri(ofdPicture.FileName));
                myCanvas.Width = myImage.Source.Width;
                myCanvas.Height = myImage.Source.Height;
            }
        }

        private void SaveImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveimg = new Microsoft.Win32.SaveFileDialog();
            saveimg.DefaultExt = ".PNG";
            saveimg.Filter = "Image (.PNG)|*.PNG";
            if (saveimg.ShowDialog() == true)
            {
                ToImageSource(myCanvas, saveimg.FileName); 
            }
        }
        public static void ToImageSource(Canvas canvas, string filename)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            canvas.Measure(new Size((int)canvas.ActualWidth, (int)canvas.ActualHeight));
            canvas.Arrange(new Rect(new Size((int)canvas.ActualWidth, (int)canvas.ActualHeight)));
            bmp.Render(canvas);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (System.IO.FileStream file = System.IO.File.Create(filename))
            {
                encoder.Save(file);
            }
        }
    }
}
