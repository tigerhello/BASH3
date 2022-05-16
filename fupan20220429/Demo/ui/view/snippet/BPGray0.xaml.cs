using System;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.ui.model;



namespace Demo.ui.view.snippet
{
    /// <summary>
    /// Interaction logic for GasSnippet.xaml
    /// </summary>
    public partial class BPGray : UserControl
    {
        public TubeNavigatorViewModel mViewModel;
        public BPGray()
        {
            InitializeComponent();
            mViewModel = new TubeNavigatorViewModel();
            DataContext = mViewModel;
            mViewModel.WorkspaceIndex = 2;
        }

        public string MyProperty0
        {
            get { return (string)GetValue(MyPropertyProperty0); }
            set { SetValue(MyPropertyProperty0, value); }
        }



        public static DependencyProperty MyPropertyProperty0 =
            DependencyProperty.Register("MyProperty0", typeof(string), typeof(BPGray), new PropertyMetadata(""));

        public string MyProperty1
        {
            get { return (string)GetValue(MyPropertyProperty1); }
            set { SetValue(MyPropertyProperty1, value); }
        }


        public static DependencyProperty MyPropertyProperty1 =
            DependencyProperty.Register("MyProperty1", typeof(string), typeof(BPGray), new PropertyMetadata(""));

    }

}
