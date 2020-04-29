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
using System.Runtime.InteropServices;

namespace PythonScheduler
{
    public partial class ScriptLauncher : ServiceBase
    {
        private Timer timer = null;
        private int startingIntreval = 3600000;//* 24; //godziny

        public ScriptLauncher()
        {
            InitializeComponent();            
        }
        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            ServiceHelper.WriteLog($"Nested methods launched at {DateTime.Now}");            

            string pathToPython = IniFile.ReadValue("Settings", "pathToPython");
            //int interval = Convert.ToInt32(IniFile.ReadValue("Settings", "interval"));       
            
            var script1 = IniFile.ReadValue("Settings", "script1");
            var script2 = IniFile.ReadValue("Settings", "script2");
            var script3 = IniFile.ReadValue("Settings", "script3");

            ServiceHelper.WriteLog(((String.IsNullOrEmpty(pathToPython) ? $"path to Python empty.{Environment.NewLine}" : $"path to Python :{pathToPython}")));
            //ServiceHelper.WriteLog(((interval == 0) ? $"Intreval equals 0.{Environment.NewLine}" : $"Interval ={interval}."));
            ServiceHelper.WriteLog(((String.IsNullOrEmpty(script1) ? $"path script 1 empty.{Environment.NewLine}" : $"path to Python script1:{script1}")));
            ServiceHelper.WriteLog(((String.IsNullOrEmpty(script2) ? $"path script 2 empty.{Environment.NewLine}" : $"path to Python script2:{script2}")));
            ServiceHelper.WriteLog(((String.IsNullOrEmpty(script2) ? $"path script 3 empty.{Environment.NewLine}" : $"path to Python script3:{script3}")));

            PythonLauncher.RunPythonScript(script1, pathToPython);
            ServiceHelper.WriteLog($"Script{script1} executed at {DateTime.Now}");

            PythonLauncher.RunPythonScript(script2, pathToPython);
            ServiceHelper.WriteLog($"Script{script2} executed at {DateTime.Now}");

            PythonLauncher.RunPythonScript(script3, pathToPython);
            ServiceHelper.WriteLog($"Script{script3} executed at {DateTime.Now}");

        }
        protected override void OnStart(string[] args)
        {   
            timer = new Timer();
            timer.Interval = startingIntreval;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.Timer_Tick);            
            timer.Start();
            ServiceHelper.WriteLog($"Service started on {DateTime.Now}");
        }

        protected override void OnStop()
        {
            timer.Stop();
            timer.Enabled = false;
            ServiceHelper.WriteLog($"Service stopped on {DateTime.Now}");
        }
        
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };
    }
}
