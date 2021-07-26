using iviwater.Controller;
using SyncPrimayerToMongoDB.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WF_iPMAC.Controller;

namespace iviwater
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        Log_Controller log = new Log_Controller();
        MainController mainController = new MainController();
        ReadCelloController readCelloController = new ReadCelloController();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.WriteLog("Service is  started at " + DateTime.Now, "Start", false);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            log.WriteLog("Service is stopped at " + DateTime.Now, "Stop", false);
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            //log.WriteLog("Service is recall at " + DateTime.Now, "ReCall", false);
            //readCelloController.SyncData();
            mainController.Main();

        }
    }
}
