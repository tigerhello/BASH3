using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using System.Threading;
using System.Diagnostics;
using System.Text;
using Demo.utilities;
using System.Windows.Data;
using Lierda.WPFHelper;

namespace Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static System.Threading.Mutex _mutex = null;
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Mutex Mutex;
        private static string _currentLang = string.Empty;
        LierdaCracker crr = new LierdaCracker();

        public App()
        {
            SingleInstanceCheck();
        }


        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)

        {

            Exception exp = e.Exception;

            //todo:记录异常    

        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)

        {

            Exception exp = (Exception)e.ExceptionObject;

            //todo:记录异常       

        }

        public void SingleInstanceCheck()
        {
            //if (mmApp.Configuration.UseSingleWindow)
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);


                bool isOnlyInstance = false;
                Mutex = new Mutex(true, @"MengKeApp_TGV4S", out isOnlyInstance);
                if (!isOnlyInstance)
                {
                    string filesToOpen = " ";
                    var args = Environment.GetCommandLineArgs();
                    if (args != null && args.Length > 1)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 1; i < args.Length; i++)
                        {
                            sb.AppendLine(args[i]);
                        }
                        filesToOpen = sb.ToString();
                    }
                    MessageBox.Show("Instance already running");                 
                    Environment.Exit(0);
                }
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            crr.Cracker();
            base.OnStartup(e);
            log.Info("==Startup=====================>>>");
           // MessageBoxResult result = MessageBox.Show("Would?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
           // if (result == MessageBoxResult.Yes)
           
                Demo.Properties.Resource1.Culture = new System.Globalization.CultureInfo("sv-SE");
              //  Demo.Properties.Resource1.Culture = new System.Globalization.CultureInfo("zh");
         
            SetupExceptionHandling();

        }
        protected virtual void CloseMutexHandler(object sender, EventArgs e)
        {
            _mutex?.Close();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            log.Info("<<<========================End==");
            base.OnExit(e);
        }



   

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");

            TaskScheduler.UnobservedTaskException += (s, e) =>
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";
            try
            {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                log.Error(exception);
            }
        }
    }

   
}
