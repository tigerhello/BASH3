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
    public partial class RegisterDlg : Window
    {
        private UserInfoViewModel mViewModel;

        private bool initWindows;

        public RegisterDlg()
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
                

                initWindows = true;
            }
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (mViewModel.UserName == null || mViewModel.UserName.Trim().Equals(""))
            {
                MessageBox.Show("Password cannot be empty");
            }
            else if (mViewModel.UserName.ToLower().Equals("mengke"))
            {
                MessageBox.Show("mengke is not allowed to be a new user");
            }
            else if (password.Password == null || password.Password.Trim().Equals(""))
            {
                MessageBox.Show("Password cannot be empty");
            }
            else if (!password.Password.Equals(passwordConf.Password))
            {
                MessageBox.Show("New password is not matched the confirmed password");
            }
            else if (mViewModel.UserRole == 0)
            {
                MessageBox.Show("Please select a user role");
            }
            else if (UserService.Instance.IsUserExist(mViewModel.UserName)) {
                MessageBox.Show("User name " + mViewModel.UserName + " has been existed");
            }
            else
            {
                UserInfoVO userInfoVO = new UserInfoVO();
                userInfoVO.UserName = mViewModel.UserName;
                userInfoVO.NickName = mViewModel.NickName;
                userInfoVO.Password = password.Password;
                userInfoVO.UserRole = mViewModel.UserRole;
                UserService.Instance.AddUserInfo(userInfoVO);
                this.Close();
            }
        }
    }
}
