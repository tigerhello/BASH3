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
using Demo.driver.cam;
using System.Linq;

namespace Demo.ui.view.card
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


        public System.Drawing.Image DrawStr(System.Drawing.Image tableChartImage, string signature)
        {

            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(tableChartImage);

            graph.DrawImage(tableChartImage, 0, 0);

            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
            graph.DrawString(signature, new System.Drawing.Font("Arial", 48), drawBrush, 0, 0);
            drawBrush.Dispose();

            graph.Dispose();
            return tableChartImage;
        }

        private System.Drawing.Image JoinImageC0(List<System.Drawing.Image> imageList, int maxX, int maxY)
        {
            //图片列表
            if (imageList.Count <= 0)
                return null;
            //构造最终的图片白板
            System.Drawing.Bitmap tableChartImage = new System.Drawing.Bitmap(maxX, maxY);
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(tableChartImage);
            //初始化这个大图
            graph.DrawImage(tableChartImage, 0, 0);
            //初始化当前宽
            int currentWidth = 0;
            int currentHeight = 0;
            int maxHeightLine = 0;//一行中最大高度
            foreach (System.Drawing.Image ImageTemp in imageList)
            {

                if (maxHeightLine < ImageTemp.Height)
                {
                    maxHeightLine = ImageTemp.Height;
                }
                if (currentWidth + ImageTemp.Width > maxX)
                {
                    currentWidth = 0;
                    currentHeight += maxHeightLine;
                    if (currentHeight >= maxY)
                    {
                        break;
                    }
                }
                if (currentHeight + maxHeightLine >= maxY)
                {
                    break;
                }

                //拼图
                graph.DrawImage(ImageTemp, currentWidth, currentHeight);
                //拼接改图后，当前宽度
                currentWidth += ImageTemp.Width;

            }

            graph.Dispose();
            GC.Collect();
         
            return tableChartImage;
        }


      

        private System.Drawing.Image JoinImageR0(List<System.Drawing.Image> imageList, int maxX, int maxY)
        {
            //图片列表
            if (imageList.Count <= 0)
                return null;
            //构造最终的图片白板
            System.Drawing.Bitmap tableChartImage = new System.Drawing.Bitmap(maxX, maxY);
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(tableChartImage);
            //初始化这个大图
            graph.DrawImage(tableChartImage, 0, 0);
            //初始化当前宽
            int currentWidth = 0;
            int currentHeight = 0;
            int maxWidthLine = 0;//一行中最大高度
            foreach (System.Drawing.Image i in imageList)
            {
                if (maxWidthLine < i.Width)
                {
                    maxWidthLine = i.Width;
                }
                if (currentHeight + i.Height > maxY)
                {
                    currentHeight = 0;
                    currentWidth += maxWidthLine;
                    if (currentWidth >= maxX)
                    {
                        break;
                    }
                }
                if (currentWidth + maxWidthLine >= maxX)
                {
                    break;
                }

                //拼图
                graph.DrawImage(i, currentWidth, currentHeight);
                //拼接改图后，当前宽度
                currentHeight += i.Height;

            }

            graph.Dispose();
            GC.Collect();

            return tableChartImage;
        }




       

        //public void JoinImage(Dictionary<string, ImageInfo1> imageGr, ref System.Drawing.Image ImageTempNG)
        //{
        //    int ALLmaxX = 2172 * 4;
        //    int ALLmaxY = 0;
        //    int NGmaxX = ALLmaxX;
        //    int NGmaxY = 0;


        //    List<System.Drawing.Image> imageListNG = new List<System.Drawing.Image>();
        //    //图片列表

        //    if (imageGr.Count <= 0)
        //        return;

        //    int height = 0;
        //    foreach (string i in imageGr.Keys)
        //    {
        //        //imageGr[i].ListGroupAll.AddRange(imageGr[i].ListGroupNG);
        //        //imageGr[i].ListGroupAll.AddRange(imageGr[i].ListGroupOK);
        //        if (i.Contains("A"))
        //        {
        //            height = 1568;
        //        }

        //        if (i.Contains("P"))
        //        {
        //            height = 1500;
        //        }

        //        if (i.Contains("D"))
        //        {
        //            height = 1568;
        //        }

        //        if (i.Contains("CS"))
        //        {
        //            height = 1416;
        //        }

        //        if (i.Contains("CR"))
        //        {
        //            height = 322;
        //        }


        //        if (imageGr[i].ListGroupNG.Count > 0)
        //        {
        //            imageGr[i].MoveToXNG = 0;
        //            imageGr[i].MoveToYNG = NGmaxY;
        //            NGmaxY += height;

        //            imageListNG.Add(JoinImageR(imageGr[i].ListGroupNG, ALLmaxX, height));
        //        }
        //    }


        //    ImageTempNG = JoinImageC(imageListNG, NGmaxX, NGmaxY + 100);

        //}


        //public void JoinImage(Dictionary<string, ImageInfo1> imageGr, ref System.Drawing.Image ImageTempNG)
        //{
        //    int ALLmaxX = 2172 * 4;
        //    int ALLmaxY = 0;
        //    int NGmaxX = ALLmaxX;
        //    int NGmaxY = 0;


        //    List<System.Drawing.Image> imageListNG = new List<System.Drawing.Image>();
        //    //图片列表

        //    if (imageGr.Count <= 0)
        //        return;

        //    int height = 0;
        //    foreach (string i in imageGr.Keys)
        //    {
        //        //imageGr[i].ListGroupAll.AddRange(imageGr[i].ListGroupNG);
        //        //imageGr[i].ListGroupAll.AddRange(imageGr[i].ListGroupOK);
        //        if (i.Contains("A"))
        //        {
        //            height = 1568;
        //        }

        //        if (i.Contains("P"))
        //        {
        //            height = 1500;
        //        }

        //        if (i.Contains("D"))
        //        {
        //            height = 1568;
        //        }

        //        if (i.Contains("CS"))
        //        {
        //            height = 1416;
        //        }

        //        if (i.Contains("CR"))
        //        {
        //            height = 322;
        //        }


        //        if (imageGr[i].ListGroupNG.Count > 0)
        //        {
        //            imageGr[i].MoveToXNG = 0;
        //            imageGr[i].MoveToYNG = NGmaxY;
        //            NGmaxY += height;

        //            imageListNG.Add(JoinImageR(imageGr[i].ListGroupNG, ALLmaxX, height));
        //        }
        //    }


        //    ImageTempNG = JoinImageC(imageListNG, NGmaxX, NGmaxY + 100);

        //}




        //public void JoinImage(Dictionary<string, ImageInfo1> imageGr, ref System.Drawing.Image ImageTempNG)
        //{
        //    int ALLmaxX = 2172 * 4;
        //    int ALLmaxY = 0;
        //    int NGmaxX = ALLmaxX;
        //    int NGmaxY = 0;


        //    List<System.Drawing.Image> imageListNG = new List<System.Drawing.Image>();
        //    //图片列表

        //    if (imageGr.Count <= 0)
        //        return;

        //    int height = 0;
        //    foreach (string i in imageGr.Keys)
        //    {
        //        //imageGr[i].ListGroupAll.AddRange(imageGr[i].ListGroupNG);
        //        //imageGr[i].ListGroupAll.AddRange(imageGr[i].ListGroupOK);
        //        if (i.Contains("A"))
        //        {
        //            height = 1568;
        //        }

        //        if (i.Contains("P"))
        //        {
        //            height = 1500;
        //        }

        //        if (i.Contains("D"))
        //        {
        //            height = 1568;
        //        }

        //        if (i.Contains("CS"))
        //        {
        //            height = 1416;
        //        }

        //        if (i.Contains("CR"))
        //        {
        //            height = 322;
        //        }


        //        if (imageGr[i].ListGroupNG.Count > 0)
        //        {
        //            imageGr[i].MoveToXNG = 0;
        //            imageGr[i].MoveToYNG = NGmaxY;
        //            NGmaxY += height;

        //            imageListNG.Add(JoinImageR(imageGr[i].ListGroupNG, ALLmaxX, height));
        //        }
        //    }


        //    ImageTempNG = JoinImageC(imageListNG, NGmaxX, NGmaxY + 100);

        //}


        //public void JoinImage(Dictionary<string, ImageInfo1> imageGr, ref System.Drawing.Image ImageTempNG)
        //{
        //    int ALLmaxX = 2172 * 4;
        //    int ALLmaxY = 0;
        //    int NGmaxX = ALLmaxX;
        //    int NGmaxY = 0;


        //    List<System.Drawing.Image> imageListNG = new List<System.Drawing.Image>();
        //    //图片列表

        //    if (imageGr.Count <= 0)
        //        return;

        //    int height = 0;
        //    foreach (string i in imageGr.Keys)
        //    {
        //        //imageGr[i].ListGroupAll.AddRange(imageGr[i].ListGroupNG);
        //        //imageGr[i].ListGroupAll.AddRange(imageGr[i].ListGroupOK);
        //        if (i.Contains("A"))
        //        {
        //            height = 1568;
        //        }

        //        if (i.Contains("P"))
        //        {
        //            height = 1500;
        //        }

        //        if (i.Contains("D"))
        //        {
        //            height = 1568;
        //        }

        //        if (i.Contains("CS"))
        //        {
        //            height = 1416;
        //        }

        //        if (i.Contains("CR"))
        //        {
        //            height = 322;
        //        }


        //        if (imageGr[i].ListGroupNG.Count > 0)
        //        {
        //            imageGr[i].MoveToXNG = 0;
        //            imageGr[i].MoveToYNG = NGmaxY;
        //            NGmaxY += height;

        //            imageListNG.Add(JoinImageR(imageGr[i].ListGroupNG, ALLmaxX, height));
        //        }
        //    }


        //    ImageTempNG = JoinImageC(imageListNG, NGmaxX, NGmaxY + 100);

        //}
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


        public void DoImageMove0(Canvas image, Point position)
        {
            TransformGroup group = image.FindResource("ImageCompareResources") as TransformGroup;
            TransformGroup group1 = this.mainPanel.FindResource("ImageCompareResources") as TransformGroup;
            Debug.Assert(group != null, "Can't find transform group from image resource");
            TranslateTransform transform = group.Children[1] as TranslateTransform;
            ScaleTransform transform1 = group1.Children[0] as ScaleTransform;
            double actualHeight = this.gridofimage.ActualHeight;
            double actualWidth = this.gridofimage.ActualWidth;
            double num3 = actualWidth * this.xscale;
            double num4 = actualHeight * this.yscale;
            double num5 = actualWidth - num3;
            double num6 = actualHeight - num4;
            double num7 = position.X;
            double num8 = position.Y;
            //if (((((transform.X + num7) > (num5 + num3)) || ((transform.Y + num8) > (num6 + num4))) || ((transform.X + num7) < (0.0 - num3))) || ((transform.Y + num8) < (0.0 - num4)))
            //{
            //    transform.X = 0.0;
            //    transform.Y = 0.0;
            //}
            //else
            //{
            transform.X = num7;
            transform.Y = num8;

            transform1.ScaleX = 1;
            transform1.ScaleY = 1;
            //}

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
            System.Threading.Thread.Sleep(100);
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


    public class ComPic
    {
        public  int ALLmaxX = 2300 * 2;
        public  int ALLmaxY = 0;

        public  double NGmaxX = 0;
        public double NGmaxY = 0;

        public System.Drawing.Image JoinImageC_BUILDING(List<System.Drawing.Image> imageList, int maxX, int maxY)
        {
            //图片列表
            if (imageList.Count <= 0 || maxY<=0 || maxX <= 0)
                return null;
            //构造最终的图片白板
            System.Drawing.Bitmap tableChartImage = new System.Drawing.Bitmap(maxX, maxY);
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(tableChartImage);
            //初始化这个大图
            graph.DrawImage(tableChartImage, 0, 0);
            //初始化当前宽
            int currentWidth = 0;
            int currentHeight = 0;

            foreach (System.Drawing.Image ImageTemp in imageList)
            {

                if (currentWidth > maxX)
                {

                    break;

                }
                if (currentHeight + ImageTemp.Height >= maxY)
                {
                    break;
                }

                //拼图
                graph.DrawImage(ImageTemp, currentWidth, currentHeight);
                //拼接改图后，当前宽度
                currentHeight += ImageTemp.Height;

            }

            graph.Dispose();
            GC.Collect();

            //Tools t1 = new Tools();
            //System.Drawing.Bitmap tableChartImageCP = t1.RGB2Gray(tableChartImage);
            ////System.Drawing.Bitmap tableChartImageCP = tableChartImage.Clone(new System.Drawing.Rectangle(0, 0, tableChartImage.Width, tableChartImage.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            //tableChartImage = null;
            //GC.Collect();

            return tableChartImage;
        }

        static  object SO = new object();
        public System.Drawing.Image JoinImageR_BACK(List<System.Drawing.Image> imageList, int maxX, int maxY)
        {
            //图片列表
            if (imageList.Count <= 0 || maxY <= 0 || maxX <= 0)
                return null;
            //构造最终的图片白板
            System.Drawing.Image tableChartImage = new System.Drawing.Bitmap(maxX, maxY);
            System.Drawing.Graphics graph = null;

            lock (SO)
            {
                graph = System.Drawing.Graphics.FromImage(tableChartImage);
            }
            //初始化这个大图
            graph.DrawImage(tableChartImage, 0, 0);
            //初始化当前宽
            int currentWidth = 0;
            int currentHeight = 0;

            foreach (System.Drawing.Image i in imageList)
            {

                if (i.Height > maxY)
                {
                    break;

                }
                if (currentWidth + i.Width >= maxX)
                {
                    break;
                }

                //拼图
                graph.DrawImage(i, currentWidth, currentHeight);
                //拼接改图后，当前宽度
                currentWidth = currentWidth + i.Width;
                currentHeight = 0;
            }

            graph.Dispose();
            GC.Collect();



            return tableChartImage;
        }

        public void CAL_all_MoveTo(Dictionary<string, ImageInfo1> imageGr,int offset,double Factor=1)
        {
            if (imageGr.Count <= 0)
                return;

            double height = 0;
            ALLmaxY = 0;


            foreach (string i in imageGr.Keys)
            {
                if (i.Contains("A"))
                {
                    height = imageGr["A"].HeightCollect.Max();

                    imageGr[i].MoveToX = 0;
                    imageGr[i].MoveToY = (int)(ALLmaxY* Factor);

                    ALLmaxY += (int)(height * Factor);
                }
            }

            foreach (string i in imageGr.Keys)
            {
                if (i.Contains("B"))
                {
                    height = imageGr["B"].HeightCollect.Max();

                    imageGr[i].MoveToX = 0;
                    imageGr[i].MoveToY = (int)((ALLmaxY - offset) * Factor);

                    ALLmaxY += (int)(height * Factor);
                }
            }

            foreach (string i in imageGr.Keys)
            {
                if (i.Contains("C1C2"))
                {
                    height = imageGr["C1C2"].HeightCollect.Max();

                    imageGr[i].MoveToX = 0;
                    imageGr[i].MoveToY = (int)((ALLmaxY - offset) * Factor);

                    ALLmaxY += (int)(height * Factor);
                }
            }


            foreach (string i in imageGr.Keys)
            {
                if (i.Contains("CR1"))
                {
                    height = imageGr["CR1"].HeightCollect.Max();

                    imageGr[i].MoveToX = 0;
                    imageGr[i].MoveToY = (int)((ALLmaxY - offset) * Factor);

                    ALLmaxY += (int)(height * Factor);
                }
            }

            foreach (string i in imageGr.Keys)
            {
                if (i.Contains("CR3"))
                {
                    height = imageGr["CR3"].HeightCollect.Max();

                    imageGr[i].MoveToX = 0;
                    imageGr[i].MoveToY = (int)((ALLmaxY - offset) * Factor);

                    ALLmaxY += (int)(height * Factor);
                }
            }


        }

        public void CAL_NGpic_MoveTo(Dictionary<string, ImageInfo1> imageGr, int offset, double Factor = 1)
        {
            if (imageGr.Count <= 0)
                return;

            double height = 0;
            NGmaxY = 0;

            double width_A = 0;
            double width_C1C2 = 0;
            double width_B = 0;
            double width_CR3 = 0;
            double width_CR1 = 0;


            foreach (string i in imageGr.Keys)
            {
                if (i.Contains("A"))
                {
                    height = imageGr["A"].HeightCollect.Max();

                    imageGr[i].MoveToXNG = 0;
                    
                    imageGr[i].MoveToYNG = (int)(NGmaxY * Factor);

                    width_A = imageGr["A"].WidthCollect.Sum();

                    NGmaxY += (int)(height * Factor);
                 
                }
            }

   

            foreach (string i in imageGr.Keys)
            {
                if (i.Contains("B"))
                {
                    height = imageGr["B"].HeightCollect.Max();

                    imageGr[i].MoveToXNG = 0;

                    
                    imageGr[i].MoveToYNG = (int)((NGmaxY - offset) * Factor);

                    width_B = imageGr["B"].WidthCollect.Sum();

                    NGmaxY += (int)(height * Factor);
                }
            }

            foreach (string i in imageGr.Keys)
            {
                if (i.Contains("C1C2"))
                {
                    height = imageGr["C1C2"].HeightCollect.Max();

                    imageGr[i].MoveToXNG = 0;
                    imageGr[i].MoveToYNG = (int)((NGmaxY - offset) * Factor);

                    width_C1C2 = imageGr["C1C2"].WidthCollect.Sum();

                    NGmaxY += (int)(height * Factor);
                }
            }

            foreach (string i in imageGr.Keys)
            {
                if (i.Contains("CR1"))
                {
                    height = imageGr["CR1"].HeightCollect.Max();

                    imageGr[i].MoveToXNG = 0;
                    imageGr[i].MoveToYNG = (int)((NGmaxY - offset) * Factor);

                    width_CR1 = imageGr["CR1"].WidthCollect.Sum();

                    NGmaxY += (int)(height * Factor);
                }
            }

            foreach (string i in imageGr.Keys)
            {
                if (i.Contains("CR3"))
                {
                    height = imageGr["CR3"].HeightCollect.Max();

                    imageGr[i].MoveToXNG = 0;
                    imageGr[i].MoveToYNG = (int)((NGmaxY - offset) * Factor);

                    width_CR3 = imageGr["CR3"].WidthCollect.Sum();

                    NGmaxY += (int)(height * Factor);
                }
            }


            NGmaxX = Math.Max(width_CR3, width_CR1);
            NGmaxX = Math.Max(NGmaxX, width_A);
            NGmaxX = Math.Max(NGmaxX, width_B);
            NGmaxX = Math.Max(NGmaxX, width_C1C2);
        }
    }


    public class ImageInfo1
    {
        public string GroupName = "";

        public int SingleIm_L = 0;
        public int SingleIm_W = 0;

        public int GroupIm_LAll = 0;
        public int GroupIm_WAll = 0;

        public int GroupIm_LNG = 0;
        public int GroupIm_WNG = 0;

        public int MoveToX = 0;
        public int MoveToY = 0;

        public int MoveToXNG = 0;
        public int MoveToYNG = 0;

        public List<System.Drawing.Image> ListGroupAll = new List<System.Drawing.Image>();
        //public List<System.Drawing.Image> ListGroupNG = new List<System.Drawing.Image>();
        public List<System.Drawing.Image> ListGroupOK = new List<System.Drawing.Image>();

        public System.Drawing.Image ImageJoinRes_ALL = null;
        public System.Drawing.Image ImageJoinRes_NG = null;

        public List<double> WidthCollect = new List<double>();
        public List<double> HeightCollect = new List<double>();

        public ImageInfo1()
        {
            HeightCollect.Add(0);
            WidthCollect.Add(0);
        }

    }

}
