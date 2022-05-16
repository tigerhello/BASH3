using System.Windows;
using System.Windows.Input;

using log4net;

using Demo.ui.model;
using System.Collections.Generic;

using System.Windows.Controls;
using Demo.utilities;
using System.Windows.Media;
using System;

namespace Demo.ui.view.snippet
{
    /// <summary>
    /// Interaction logic for TubePressureCard.xaml
    /// </summary>
    public partial class LoaderDoSnippet : UserControl
    {
        //private LoaderMaintControlViewModel mViewModel;

        public LoaderDoSnippet()
        {
            InitializeComponent();

        }

        private void Btn_Switch_Click(object sender, MouseButtonEventArgs e)
        {
            CamNub = this.Name.ToString();
        }


        private void OnValuePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Value)
            {

                lblValue6.Foreground = new SolidColorBrush(Colors.Red);

                    lblValue6.Content = this.Name.ToString();
            }
            else
            {
                lblValue6.Foreground = new SolidColorBrush(Colors.Black);

                    lblValue6.Content = this.Name.ToString();
            }
               
        }

        public static string CamNub = "";

   
        /// <summary>
        /// Gets or sets the Value which is being displayed
        /// </summary>
        public bool Value
        {
            get { return (bool)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(bool),
                typeof(LoaderDoSnippet), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSamplePropertyChanged));

        static void OnSamplePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as LoaderDoSnippet).OnValuePropertyChanged(e);
        }

        private void ld(object sender, RoutedEventArgs e)
        {
            try
            {

                lblValue6.Content = this.Name.ToString();

            }
            catch (Exception EX)
            {
                System.Windows.Forms.MessageBox.Show(EX.ToString());
            }
        }
    }



}


