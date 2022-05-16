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
    public partial class PickerSettingsDlg : Window
    {
        private int mLoaderIndex;
        private LoaderPaddleConfViewModel mViewModel;
        private bool initWindows;

        public PickerSettingsDlg()
        {
            InitializeComponent();

            mViewModel = LoaderViewModelFactory.Instance.GetLoaderPaddleConfViewModel();
            DataContext = mViewModel;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (!initWindows)
            {

                UpdateConfigData();
                initWindows = true;
            }
            
        }

        public void SetLoaderIndex(int loaderIndex) {
            mLoaderIndex = loaderIndex;
        }




        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            DownloadConfig();
            this.Close();
        }

        private void DownloadConfig()
        {
            if (MessageBox.Show("Would you want to download config?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LoaderService.Instance.PutPaddleConfig(mLoaderIndex, mViewModel.PaddleConfig);

                MessageBox.Show("The config has been downloaded sucessfully.");
                UpdateConfigData();
            }
        }

        private void UpdateConfigData()
        {
            List<bool> paddleConfig = LoaderService.Instance.GetPaddleConfig(mLoaderIndex);
            for (int i = 0; i < 20; ++i)
            {
                mViewModel.PaddleConfig[i] = paddleConfig[i];
            }
            mViewModel.NotifyPaddleConfig();
        }
    }
}
