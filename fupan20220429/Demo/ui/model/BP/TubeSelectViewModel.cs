using System.Windows;

namespace Demo.ui.model
{
    public class TubeSelectViewModel : ViewModel
    {
        private int mTubeIndex;
        private Visibility mAlarm1Visible;
        private Visibility mAlarm2Visible;
        private Visibility mAlarm3Visible;
        private Visibility mAlarm4Visible;
        private Visibility mAlarm6Visible;

        private uint[] mProcessTotalTime = new uint[4];
        private uint[] mProcessEscapedTime = new uint[4];
        private int[] mRunStatus = new int[4];

        public TubeSelectViewModel() {
            mTubeIndex = 1;
        }


        public int TubeIndex
        {
            get { return mTubeIndex; }
            set{
                mTubeIndex = value;
                Notify("TubeIndex");
            }
        }

        public Visibility Alarm1Visible
        {
            get { return mAlarm1Visible; }
            set {
                mAlarm1Visible = value;
                Notify("Alarm1Visible");
            }
        }

        public Visibility Alarm2Visible
        {
            get { return mAlarm2Visible; }
            set
            {
                mAlarm2Visible = value;
                Notify("Alarm2Visible");
            }
        }

        public Visibility Alarm3Visible
        {
            get { return mAlarm3Visible; }
            set
            {
                mAlarm3Visible = value;
                Notify("Alarm3Visible");
            }
        }

        public Visibility Alarm4Visible
        {
            get { return mAlarm4Visible; }
            set
            {
                mAlarm4Visible = value;
                Notify("Alarm4Visible");
            }
        }

        public Visibility Alarm6Visible
        {
            get { return mAlarm6Visible; }
            set
            {
                mAlarm6Visible = value;
                Notify("Alarm6Visible");
            }
        }

        public int[] RunStatus
        {
            get { return mRunStatus; }
        }

        public uint[] ProcessTotalTime
        {
            get { return mProcessTotalTime; }
        }

        public uint[] ProcessEscapedTime
        {
            get { return mProcessEscapedTime; }
        }

        public void NotifyProcess() {
            Notify("RunStatus");
            Notify("ProcessTotalTime");
            Notify("ProcessEscapedTime");
        }
    }
}
