using Demo.ui.model;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Demo.model.vo;
using Demo.service;
using System.Windows.Input;
using System.Windows.Controls;
using Demo.ui.view.card;

namespace Demo.ui.view.dialog
{
    /// <summary>
    /// Interaction logic for JigEditDlg.xaml
    /// </summary>
    public partial class UserListDlg : Window
    {

        private ObservableCollection<UserInfoViewModel> mItemModels;
        private string mUserName;
        private int mUserRole;

        private bool initWindows;

        public UserListDlg()
        {
            InitializeComponent();

            mItemModels = new ObservableCollection<UserInfoViewModel>();
            DataContext = mItemModels;

            initWindows = false;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (!initWindows) {
                LoadItems();

                initWindows = true;
            }
        }

        public void SetUserInfo(UserInfoVO userInfoVO) {
            mUserName = userInfoVO.UserName;
            mUserRole = userInfoVO.UserRole;
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            UserInfoVO userInfoVO = UserService.Instance.CurUserInfo;
            RegisterDlg newUserDlg = new RegisterDlg();
            newUserDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            newUserDlg.ShowDialog();
            LoadItems();
        }

        private void LoadItems()
        {
            mItemModels.Clear();
            List<UserInfoVO> userInfoVOs = UserService.Instance.GetUserInfoList(mUserName);
            for (int i = 0; i < userInfoVOs.Count; ++i) {
                UserInfoViewModel userInfoModel = new UserInfoViewModel();
                userInfoModel.ID = (i + 1);
                userInfoModel.UserName = userInfoVOs[i].UserName;
                userInfoModel.NickName = userInfoVOs[i].NickName;
                userInfoModel.UserRole = userInfoVOs[i].UserRole;
                mItemModels.Add(userInfoModel);
            }
        }

        private void DeleteRow(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<UserInfoViewModel> viewModels = (ObservableCollection<UserInfoViewModel>)userDataGrid.DataContext;
            DataGridRow dgr = DataGridExtensions.FindParent<DataGridRow>(sender as DependencyObject);
            string userName = viewModels[dgr.GetIndex()].UserName;
            if (MessageBox.Show("Would you want to delete user " + userName + "?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                UserService.Instance.DelUserInfo(userName);
            }
            LoadItems();
        }

    }
}
