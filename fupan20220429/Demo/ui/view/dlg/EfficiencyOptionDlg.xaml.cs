using Demo.service;
using Demo.ui.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Demo.model.vo;

namespace Demo.ui.view.dialog
{
    /// <summary>
    /// Interaction logic for JigEditDlg.xaml
    /// </summary>
    public partial class EfficiencyOptionDlg : Window
    {
        private EfficiencyOptionViewModel mViewModel;

        private bool initWindows;

        public EfficiencyOptionDlg()
        {
            InitializeComponent();

            mViewModel = new EfficiencyOptionViewModel();
            DataContext = mViewModel;

            initWindows = false;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (!initWindows)
            {

                mViewModel.Baseline = TubeStatisticService.Instance.GetEfficiencyBaseline();
                initWindows = true;
            }
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            TubeStatisticService.Instance.PutEfficiencyBaseline(mViewModel.Baseline);
            this.Close();
        }
    }
}
