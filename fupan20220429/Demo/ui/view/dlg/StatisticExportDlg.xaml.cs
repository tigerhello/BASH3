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
using Demo.utilities;

namespace Demo.ui.view.dialog
{
    /// <summary>
    /// Interaction logic for JigEditDlg.xaml
    /// </summary>
    public partial class StatisticExportDlg : Window
    {
        private EfficiencyOptionViewModel mViewModel;

        private bool initWindows;
        private int mRunID;
        private int mTubeIndex;

        public StatisticExportDlg()
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

                txtFileName.Text = "Tube" + mTubeIndex + "_Run" + mRunID;
                initWindows = true;
            }
        }

        public void SetRunID(int runID) {
            mRunID = runID;
        }

        public void SetTubeIndex(int tubeIndex) {
            mTubeIndex = tubeIndex;
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            string fileName = txtFileName.Text;
            int fileType = cbbFileType.SelectedIndex;
            if (fileType == 0) {
                MessageBox.Show("fileName:" + fileName + ",file type:csv");

                int[][] iTemperRawData = TubeStatisticService.Instance.GetTemperRawData(mTubeIndex, mRunID);
                if (iTemperRawData != null) {
                    string[][] sTemperRawData = convertTemperRawData(iTemperRawData);
                    ExportUtility.ExportCSV(sTemperRawData, fileName);
                }


            }
            else if (fileType == 1)
            {
                MessageBox.Show("fileName:" + fileName + ",file type:xls");
            }
            this.Close();
        }

        private string[][] convertTemperRawData(int[][] iTemperRawData) {
            string[][] sTemperRawData = new string[iTemperRawData.Length][];
            return sTemperRawData;
        }
    }
}
