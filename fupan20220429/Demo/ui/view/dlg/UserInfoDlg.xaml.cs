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
    public partial class UserInfoDlg : Window
    {
        private UserInfoViewModel mViewModel;

        public UserInfoDlg()
        {
            InitializeComponent();

            mViewModel = new UserInfoViewModel();
            DataContext = mViewModel;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            //LoaderService.Instance.AskJigInfoEdit(mLoaderIndex, mJigIndex);
            //StartUpdateAskUIServer();
        }

        public void SetUserInfo(UserInfoVO userInfoVO) {
            mViewModel.UserName = userInfoVO.UserName;
            mViewModel.NickName = userInfoVO.NickName;
            mViewModel.Password = userInfoVO.Password;
            mViewModel.UserRole = userInfoVO.UserRole;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UserInfoVO userInfoVO = new UserInfoVO();
            userInfoVO.UserName = mViewModel.UserName;
            userInfoVO.Password = mViewModel.Password;
            userInfoVO.NickName = mViewModel.NickName;
            userInfoVO.UserRole = mViewModel.UserRole;

            UserUpdateDlg userUpdateDlg = new UserUpdateDlg();
            userUpdateDlg.SetUserInfo(userInfoVO);
            userUpdateDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            userUpdateDlg.ShowDialog();

            userInfoVO = UserService.Instance.CurUserInfo;
            mViewModel.Password = userInfoVO.Password;
            mViewModel.NickName = userInfoVO.NickName;
        }
    }
}
