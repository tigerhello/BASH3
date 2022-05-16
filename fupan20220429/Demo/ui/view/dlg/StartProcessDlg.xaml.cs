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
    public partial class StartProcessDlg : Window
    {
        private UserInfoViewModel mViewModel;
        private List<string> mStepList;
        private int mSelectedStep;

        private bool initWindows;

        public StartProcessDlg()
        {
            InitializeComponent();

            mViewModel = new UserInfoViewModel();
            DataContext = mViewModel;

            initWindows = false;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (!initWindows)
            {
                mSelectedStep = 0;
                mStepList = new List<string>();
                mStepList.Add("");
                TubeProcessViewModel procModel = TubeViewModelFactory.Instance.GetProcViewModel("TubeMaintPage");
                for (int i = 0; i < 64; ++i)
                {
                    string stepName = ((procModel.StepNames != null) ? procModel.StepNames[i] : "");
                    mStepList.Add("Step" + (i + 1) + ":" + stepName);
                }
                comboSteps.ItemsSource = mStepList;

                initWindows = true;
            }
        }

        public int SelectedStep {
            get { return mSelectedStep; }
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            mSelectedStep = 0;
            this.Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            mSelectedStep = comboSteps.SelectedIndex;
            if (mSelectedStep == 0)
            {
                MessageBox.Show("Please select a step first.");
            }
            else
            {
                this.Close();
            }
        }
    }
}
