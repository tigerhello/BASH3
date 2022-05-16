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
    public partial class JigEditDlg : Window
    {
        private int mLoaderIndex;
        private int mJigIndex;

        private Thread mUpdateAskRunThread;
        private int updateTimeout;
        private bool mUpdateUI;

        public JigEditDlg()
        {
            InitializeComponent();

            LoaderJigViewModel jigModel = LoaderViewModelFactory.Instance.GetLoaderJigViewModel();
            DataContext = jigModel;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            LoaderService.Instance.AskJigInfoEdit(mLoaderIndex, mJigIndex);
            StartUpdateAskUIServer();
        }

        public void SetLoaderIndex(int loaderIndex) {
            mLoaderIndex = loaderIndex;
        }

        public void SetJigIndex(int jigIndex) {
            mJigIndex = jigIndex;
        }

        public void SetJigIDs(List<int> jigIDs) {
            LoaderJigViewModel jigModel = LoaderViewModelFactory.Instance.GetLoaderJigViewModel();
            jigModel.JigIDs[0] = jigIDs[0];
            jigModel.JigIDs[1] = jigIDs[1];
        }

        public void SetJigStatus(int jigStatus) {
            LoaderJigViewModel jigModel = LoaderViewModelFactory.Instance.GetLoaderJigViewModel();
            jigModel.JigStatus = jigStatus;
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            LoaderJigViewModel jigModel = LoaderViewModelFactory.Instance.GetLoaderJigViewModel();
            JigInfoVO jigInfo = new JigInfoVO();
            jigInfo.JigIndex = mJigIndex;
            jigInfo.JigStatus = jigModel.JigStatus;
            jigInfo.JigIDs[0] = jigModel.JigIDs[0];
            jigInfo.JigIDs[1] = jigModel.JigIDs[1];
            for (int i = 0; i < 10; ++i) {
                jigInfo.BoatIDs[i] = jigModel.BoatIDs[i];
            }
            LoaderService.Instance.PutJigInfoEdit(mLoaderIndex, jigInfo);
            this.Close();
        }

        private void StartUpdateAskUIServer()
        {
            updateTimeout = 0;
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                //waitPopup.Visibility = Visibility.Visible;
            });
            mUpdateAskRunThread = new Thread(() =>
            {
                while (!UpdateJigEditAskAck() && (updateTimeout < 10))
                {
                    if (!mUpdateUI)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                    updateTimeout++;
                }
                if (updateTimeout == 10)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        MessageBox.Show("Update Timeout.");
                        //waitPopup.Visibility = Visibility.Hidden;
                    });
                }
                else if (mLoaderIndex != 0 && mUpdateUI)
                {
                    LoaderJigViewModel jigModel = LoaderViewModelFactory.Instance.GetLoaderJigViewModel();
                    jigModel.NotifyJigIDs();
                    JigInfoVO jigInfoVO = LoaderService.Instance.GetJigInfoEdit(mLoaderIndex, mJigIndex);
                    if (jigInfoVO==null)
                    {
                        MessageBox.Show("对象为空");
                    }
                    jigModel.BoatIDs[0] = jigInfoVO.BoatIDs[0];
                    jigModel.BoatIDs[1] = jigInfoVO.BoatIDs[1];
                    jigModel.BoatIDs[2] = jigInfoVO.BoatIDs[2];
                    jigModel.BoatIDs[3] = jigInfoVO.BoatIDs[3];
                    jigModel.BoatIDs[4] = jigInfoVO.BoatIDs[4];
                    jigModel.BoatIDs[5] = jigInfoVO.BoatIDs[5];
                    jigModel.BoatIDs[6] = jigInfoVO.BoatIDs[6];
                    jigModel.BoatIDs[7] = jigInfoVO.BoatIDs[7];
                    jigModel.BoatIDs[8] = jigInfoVO.BoatIDs[8];
                    jigModel.BoatIDs[9] = jigInfoVO.BoatIDs[9];
                    jigModel.NotifyBoatIDs();

                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        
                        //MessageBox.Show("The config has been updated sucessfully.");
                        //waitPopup.Visibility = Visibility.Hidden;
                    });
                }
            });
            mUpdateUI = true;
            mUpdateAskRunThread.IsBackground = true;
            mUpdateAskRunThread.Start();
        }

        private bool UpdateJigEditAskAck()
        {
            JigInfoVO jigInfoVO = LoaderService.Instance.GetJigInfoEdit(mLoaderIndex, mJigIndex);
            if (jigInfoVO != null) {
                return (jigInfoVO.JigStatus == 3);
            }
            return false;
        }
    }
}
