
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


namespace Demo.ui.view.dialog
{
    /// <summary>
    /// Interaction logic for JigEditDlg.xaml
    /// </summary>
    public partial class UserUpdateDlg : Window
    {
        private UserInfoViewModel mViewModel;

        private bool initWindows;

        public UserUpdateDlg()
        {
            InitializeComponent();

            mViewModel = new UserInfoViewModel();
            DataContext = mViewModel;

            initWindows = false;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

        }

      public  bool userUpdateDlgBool=false;
 

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            userUpdateDlgBool = false;
            this.Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {

            //if (ServiceFactory.machine.g_PSW != passwordOld.Password)
            //{
            //    MessageBox.Show("Old password is not correct");
            //}
            //else if (passwordConf.Password != passwordNew.Password)
            //{
            //    MessageBox.Show("New password is not matched the confirmed password");
            //}
            //else
            //{
            //    userUpdateDlgBool = true;
            //    ServiceFactory.Instance.Inifile.WriteObjToIni("config", "PSW", passwordConf.Password, ServiceFactory.machineDataPath);
            //    this.Close();
            //}
        }
    }
}
