
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
using Demo.utilities;

namespace Demo.ui.view.dialog
{
    /// <summary>
    /// Interaction logic for JigEditDlg.xaml
    /// </summary>
    public partial class LoginDlg : Window
    {
        private UserInfoViewModel mViewModel;
        public bool LoginBool = false;

        public LoginDlg()
        {
            InitializeComponent();

            mViewModel = new UserInfoViewModel();
            DataContext = mViewModel;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);


            passwordBox.Focus();
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            LoginBool = false;
            this.Close();
        }


        private void btnApply_Click(object sender, RoutedEventArgs e)
        {


            //if (ServiceFactory.machine.g_PSW== passwordBox.Password)
            //{
            //    LoginBool = true;
            //    this.Close();
            //}
            //else
            //{
            //    LoginBool = false;
            //    MessageBox.Show("Login failed.");
            //}
        }
    }
}
