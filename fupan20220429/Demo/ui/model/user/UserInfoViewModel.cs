using System.ComponentModel;


namespace Demo.ui.model
{
    public class UserInfoViewModel:ViewModel
    {

        private int mID;
        private string mUserName;
        private string mNickName;
        private string mPassword;
        private string mPasswordConf;
        private int mUserRole;

        public UserInfoViewModel() {
        }

        public int ID
        {
            get { return mID; }
            set
            {
                mID = value;
                Notify("ID");
            }
        }

        public string UserName
        {
            get { return mUserName; }
            set
            {
                mUserName = value;
                Notify("UserName");
            }
        }

        public string NickName
        {
            get { return mNickName; }
            set
            {
                mNickName = value;
                Notify("NickName");
            }
        }

        public string Password
        {
            get { return mPassword; }
            set
            {
                mPassword = value;
                Notify("Password");
            }
        }

        public string PasswordConf
        {
            get { return mPasswordConf; }
            set
            {
                mPasswordConf = value;
                Notify("PasswordConf");
            }
        }

        public int UserRole
        {
            get { return mUserRole; }
            set
            {
                mUserRole = value;
                Notify("UserRole");
            }
        }
    }
}
