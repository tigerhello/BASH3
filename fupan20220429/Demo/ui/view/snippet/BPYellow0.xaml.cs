using System;
using System.Windows;
using System.Windows.Controls;


namespace Demo.ui.view.snippet
{
    /// <summary>
    /// Interaction logic for GasSnippet.xaml
    /// </summary>
    public partial class BPYellow : UserControl
    {
        public BPYellow()
        {
            InitializeComponent();

        }

        public string MyProperty0
        {
            get { return (string)GetValue(MyPropertyProperty0); }
            set { SetValue(MyPropertyProperty0, value); }
        }



        public static DependencyProperty MyPropertyProperty0 =
            DependencyProperty.Register("MyProperty0", typeof(string), typeof(BPYellow), new PropertyMetadata(""));

        public string MyProperty1
        {
            get { return (string)GetValue(MyPropertyProperty1); }
            set { SetValue(MyPropertyProperty1, value); }
        }


        public static DependencyProperty MyPropertyProperty1 =
            DependencyProperty.Register("MyProperty1", typeof(string), typeof(BPYellow), new PropertyMetadata(""));

    }

}
