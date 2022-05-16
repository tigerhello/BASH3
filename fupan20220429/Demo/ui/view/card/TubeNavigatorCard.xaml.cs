using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;
using Demo.ui.view.dialog;
using log4net;
using Demo.ui.view.converter;
using Demo.ui.model;

using Demo.utilities;
using System.Windows.Data;

namespace Demo.ui.view.card
{
    /// <summary>
    /// Interaction logic for TubeMaintToolbarView.xaml
    /// </summary>
    public partial class TubeNavigatorCard : UserControl
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public event MainFrame.OnExitWorkspace ExitWorkspaceEvent;
        public event MainFrame.OnEnterWorkspace LoadWorkspacePageEvent;

        public TubeNavigatorViewModel mViewModel;

        
        public TubeNavigatorCard()
        {
            InitializeComponent();

            mViewModel = new TubeNavigatorViewModel();
            DataContext = mViewModel;
        }

        public void SetWorkspaceIndex(int index) {
            mViewModel.WorkspaceIndex = index;
        }
       
        private void btnMonitor_Click(object sender, RoutedEventArgs e)
        {

            mViewModel.WorkspaceIndex = 0;
            if (LoadWorkspacePageEvent != null) {
                LoadWorkspacePageEvent(0);
            }
        }
        //点击手动页面请求
        private void btnMaint_Click(object sender, RoutedEventArgs e)
        {
            LoginDlg loginDlg = new LoginDlg();
            loginDlg.LoginBool = false;
            loginDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            loginDlg.ShowDialog();

            if (loginDlg.LoginBool == false) return;

            mViewModel.WorkspaceIndex = 1;

            if (LoadWorkspacePageEvent != null)
            {
                LoadWorkspacePageEvent(1);
            }
        }
        //点击配置页面请求
        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            //if (!UserService.Instance.IsConfigAllowed())
            //{
            //    MessageBox.Show("The user has no access permission for config function");
            //    return;
            //}

            mViewModel.WorkspaceIndex = 3;

            if (LoadWorkspacePageEvent != null)
            {
                LoadWorkspacePageEvent(3);
            }
        }
        //点击工艺页面请求
        private void btnRecipe_Click(object sender, RoutedEventArgs e)
        {


            mViewModel.WorkspaceIndex = 4;

            if (LoadWorkspacePageEvent != null)
            {
                LoadWorkspacePageEvent(4);
            }
        }
        //点击曲线页面请求
        private void btnTrend_Click(object sender, RoutedEventArgs e)
        {
            mViewModel.WorkspaceIndex = 5;

            if (LoadWorkspacePageEvent != null)
            {
                LoadWorkspacePageEvent(5);
            }
        }
        //点击历史页面请求
        private void btnEvent_Click(object sender, RoutedEventArgs e)
        {
            mViewModel.WorkspaceIndex = 6;

            if (LoadWorkspacePageEvent != null)
            {
                LoadWorkspacePageEvent(6);
            }
        }
        //点击报警页面请求
        private void btnAlarm_Click(object sender, RoutedEventArgs e)
        {
            mViewModel.WorkspaceIndex = 7;

            if (LoadWorkspacePageEvent != null)
            {
                LoadWorkspacePageEvent(7);
            }
        }
        //点击工艺历史信息页面请求
        private void btnStatistic_Click(object sender, RoutedEventArgs e)
        {
            mViewModel.WorkspaceIndex = 8;

            if (LoadWorkspacePageEvent != null)
            {
                LoadWorkspacePageEvent(8);
            }
        }

        public delegate void IniImage_Record();
        public event IniImage_Record IniImageRecord;
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            IniImageRecord();
            mViewModel.LoaderIndex = 9;
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            //if (ServiceFactory.control.bIsGameOver == true) return; 

            //mViewModel.LoaderIndex = 10;
            //ServiceFactory.control.bIsPuase = true;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //if (ServiceFactory.control.bIsPuase == true) return;

            //mViewModel.LoaderIndex = 11;
            //ServiceFactory.control.bIsGameOver = true;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserUpdateDlg userUpdateDlg = new UserUpdateDlg();
            userUpdateDlg.userUpdateDlgBool = false;
            userUpdateDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            userUpdateDlg.ShowDialog();

            if (userUpdateDlg.userUpdateDlgBool == false) return;

            mViewModel.LogIndex = 1;
        }
    }
}
