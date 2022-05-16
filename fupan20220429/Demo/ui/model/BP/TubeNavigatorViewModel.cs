using System.Collections.Generic;

namespace Demo.ui.model
{
    public class TubeNavigatorViewModel : ViewModel
    {
        private int mWorkspaceIndex;
        private int mLoaderIndex;
        private int mLogIndex;
        private bool[] mDeStatus=new bool[38];
        public bool[] DeStatus
        {
            get { return mDeStatus; }
            set
            {
                mDeStatus = value;
               
            }
        }

        public void NotifyDeStatus()
        {
            Notify("DeStatus");
        }

        public TubeNavigatorViewModel() {
            mWorkspaceIndex = 0;
            mLoaderIndex = 0;
        }

        public int LoaderIndex
        {
            get { return mLoaderIndex; }
            set
            {
                mLoaderIndex = value;
                Notify("LoaderIndex");
            }
        }

        public int LogIndex
        {
            get { return mLogIndex; }
            set{
                mLogIndex = value;
                Notify("LogIndex");
            }
        }

        public int WorkspaceIndex
        {
            get { return mWorkspaceIndex; }
            set
            {
                mWorkspaceIndex = value;
                Notify("WorkspaceIndex");
            }
        }
    }
}
