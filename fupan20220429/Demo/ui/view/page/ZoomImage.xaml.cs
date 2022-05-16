using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Markup;


namespace DoubleC
{

    public partial class ZoomImage : UserControl, IComponentConnector
    {
        private static ImageBrush ib = new ImageBrush();
        private double xscale = 0.0;
        private double yscale = 0.0;
        private Point PreviousMousePosition;
        public static readonly DependencyProperty PathPropertyProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ZoomImage), new PropertyMetadata(new PropertyChangedCallback(PathChanged)));

        public ZoomImage()
        {
            this.InitializeComponent();

            this.mainPanel.MouseMove += new MouseEventHandler(this.mainPanel_MouseMove);
            this.mainPanel.MouseLeftButtonDown += new MouseButtonEventHandler(this.mainPanel_MouseLeftButtonDown);
            this.mainPanel.MouseLeftButtonUp += new MouseButtonEventHandler(this.mainPanel_MouseLeftButtonUp);
            this.mainPanel.MouseWheel += new MouseWheelEventHandler(this.mainPanel_MouseWheel);
        }

        private void DoImageMove(Canvas image, Point position)
        {
            TransformGroup group = image.FindResource("ImageCompareResources") as TransformGroup;
            Debug.Assert(group != null, "Can't find transform group from image resource");
            TranslateTransform transform = group.Children[1] as TranslateTransform;
            double actualHeight = this.gridofimage.ActualHeight;
            double actualWidth = this.gridofimage.ActualWidth;
            double num3 = actualWidth * this.xscale;
            double num4 = actualHeight * this.yscale;
            double num5 = actualWidth - num3;
            double num6 = actualHeight - num4;
            double num7 = position.X - this.PreviousMousePosition.X;
            double num8 = position.Y - this.PreviousMousePosition.Y;
            if (((((transform.X + num7) > (num5 + num3)) || ((transform.Y + num8) > (num6 + num4))) || ((transform.X + num7) < (0.0 - num3))) || ((transform.Y + num8) < (0.0 - num4)))
            {
                transform.X = 0.0;
                transform.Y = 0.0;
            }
            else
            {
                transform.X += num7;
                transform.Y += num8;
            }
            this.PreviousMousePosition = position;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void DoZoomImage(TransformGroup group, Point point, double scale)
        {
            if (ib.ImageSource != null)
            {
                Debug.Assert(group != null, "Image is removed from current control's resource");
                Point point2 = group.Inverse.Transform(point);
                ScaleTransform transform = group.Children[0] as ScaleTransform;
                if ((transform.ScaleX + scale) >= 0.0)
                {
                    if (scale < 0.0)
                    {
                        transform.ScaleX /= 1.0 - scale;
                        transform.ScaleY /= 1.0 - scale;
                    }
                    else
                    {
                        transform.ScaleX *= 1.0 + scale;
                        transform.ScaleY *= 1.0 + scale;
                    }
                    this.xscale = transform.ScaleX;
                    this.yscale = transform.ScaleY;
                    TranslateTransform transform2 = group.Children[1] as TranslateTransform;
                    transform2.X = -1.0 * ((point2.X * transform.ScaleX) - point.X);
                    transform2.Y = -1.0 * ((point2.Y * transform.ScaleY) - point.Y);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }


        private void mainPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.mainPanel.CaptureMouse();
            this.PreviousMousePosition = e.GetPosition(this.gridofimage);
        }

        private void mainPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.mainPanel.ReleaseMouseCapture();
        }

        private void mainPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DoImageMove(this.mainPanel, e.GetPosition(this.gridofimage));
            }
        }

        private void mainPanel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            TransformGroup group = this.mainPanel.FindResource("ImageCompareResources") as TransformGroup;
            Debug.Assert(group != null, "Can't find transform group from image resource");
            Point position = e.GetPosition(this.gridofimage);
            double scale = e.Delta * 0.001;
            this.DoZoomImage(group, position, scale);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private static void PathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZoomImage image = d as ZoomImage;
            if (image != null)
            {
                ib.ImageSource = (ImageSource)e.NewValue;
                ib.Stretch = Stretch.Uniform;
                image.mainPanel.Background = ib;
            }
        }


        public ImageSource Source
        {
            get
            {
                return (ImageSource)base.GetValue(PathPropertyProperty);
            }
            set
            {
                base.SetValue(PathPropertyProperty, value);
            }
        }
    }

}
