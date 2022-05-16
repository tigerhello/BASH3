using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text;
using log4net;
using Demo.ui.model;
using System.Windows.Threading;
using System;
using System.Threading;
using System.Collections.ObjectModel;
using System.Data;

namespace Demo.ui.view.card
{
    /// <summary>
    /// Interaction logic for TubePressureCard.xaml
    /// </summary>
    public partial class TubeAlarmHistoryCard 
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int mSelectedTube;
        private ObservableCollection<TubeAlarmItemViewModel> mItemModels;

        private Thread mUpdateAlarmThread;
        private bool mUpdateAlarm;
        private bool mHoldUpdateAlarm;

        public TubeAlarmHistoryCard()
        {
            InitializeComponent();

            mItemModels = new ObservableCollection<TubeAlarmItemViewModel>();
            alarmDataGrid.DataContext = mItemModels;

            //StartUpdateAlarmServer();
            mHoldUpdateAlarm = true;

            //TubeAlarmItemViewModel itemModel = new TubeAlarmItemViewModel();
            //itemModel.AlarmID = true;
            //itemModel.AlarmName = "1";
            //itemModel.AlarmDesc = "电气正常！";
    
            //mItemModels.Add(itemModel);
            List<string> config = driver.cam.Tools.ReadTxt(@"data\checklist.txt");

            for (int i = 0; i < config.Count; i++)
            {
                TubeAlarmItemViewModel itemModel0 = new TubeAlarmItemViewModel();
                itemModel0.AlarmID = false;
                itemModel0.AlarmName = config[i].Split(',')[0]; 
                itemModel0.AlarmDesc = config[i].Split(',')[1]; 
                mItemModels.Add(itemModel0);
            }
        }

        public void LoadTubeView(int tubeIndex)
        {
            mSelectedTube = tubeIndex;

            UpdateItems();
            mHoldUpdateAlarm = false;
        }

        public void UnloadTubeView(int tubeIndex)
        {
            mHoldUpdateAlarm = true;
            //StopUpdatePlotServer();
        }

        private void StartUpdateAlarmServer()
        {
            mUpdateAlarmThread = new Thread(() =>
            {
                while (mUpdateAlarm)
                {
                    if (mSelectedTube != 0)
                    {
                        if (!mHoldUpdateAlarm)
                        {
                            UpdateItems();
                        }

                    }
                    Thread.Sleep(10000);
                }
            });
            mUpdateAlarm = true;
            mUpdateAlarmThread.IsBackground = true;
            mUpdateAlarmThread.Start();
        }

        private void StopUpdateAlarmServer()
        {
            mUpdateAlarm = false;
        }

        private void UpdateItems() {
            
            //List<TubeAlarmTO> alarms = TubeAlarmService.Instance.GetAlarmHistoryList(mSelectedTube);
            //Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            //{
            //    mItemModels.Clear();
            //    for (int i = 0; i < alarms.Count; ++i)
            //    {
            //        TubeAlarmItemViewModel itemModel = new TubeAlarmItemViewModel();
            //        itemModel.AlarmID = (i + 1);
            //        itemModel.AlarmName = alarms[i].AlarmName;
            //        itemModel.AlarmDesc = alarms[i].AlarmDesc;
            //        itemModel.AlarmTime = alarms[i].AlarmTime;
            //        mItemModels.Add(itemModel);
            //    }
            //});
        }

        private void alarmDataGrid_LayoutUpdated(object sender, EventArgs e)
        {
            return;

                foreach (DataRowView drv in this.alarmDataGrid.Items)
                {
                    
                    foreach (DataGridColumn dgc in this.alarmDataGrid.Columns)
                    {
                        var content = dgc.GetCellContent(drv);
                        if (content != null
                            && content.GetType() == typeof(CheckBox))
                        {
                            CheckBox cb = content as CheckBox;
                            cb.Content = "点我";
                        }
                    }
                }

        }

        private void FieldDataGridChecked(object sender, RoutedEventArgs e)
        {

        }

        private void FieldDataGridUnchecked(object sender, RoutedEventArgs e)
        {

        }
    }

    public class TubeAlarmItemViewModel : ViewModel
    {

        private int mTubeIndex;

        private bool mAlarmID;
        private string mAlarmName;
        private string mAlarmDesc;
        private string mAlarmTime;

        public TubeAlarmItemViewModel()
        {

        }

        public int TubeIndex
        {
            get { return mTubeIndex; }
            set
            {
                mTubeIndex = value;
                Notify("TubeIndex");
            }
        }

        public bool AlarmID
        {
            get { return mAlarmID; }
            set
            {
                mAlarmID = value;
                Notify("AlarmID");
            }
        }

        public string AlarmName
        {
            get { return mAlarmName; }
            set
            {
                mAlarmName = value;
                Notify("AlarmName");
            }
        }


        public string AlarmDesc
        {
            get { return mAlarmDesc; }
            set
            {
                mAlarmDesc = value;
                Notify("AlarmDesc");
            }
        }

        public string AlarmTime
        {
            get { return mAlarmTime; }
            set
            {
                mAlarmTime = value;
                Notify("AlarmTime");
            }
        }
    }

}
