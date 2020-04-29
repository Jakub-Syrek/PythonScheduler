using System.Diagnostics;
using System.IO;

namespace PythonScheduler
{
    public static class PythonLauncher
    {  
        public static void RunPythonScript(string script, string pathToInterpreter)
        {
            try
            {
                ServiceHelper.WriteLog($"Inner Python Script=>{script} {pathToInterpreter}");
                ProcessStartInfo processStartInfo = new ProcessStartInfo()
                {
                    Arguments = script ,
                    FileName = pathToInterpreter,
                    CreateNoWindow = false,
                    // can only redirect STDIO when UseShellExecute=false
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                };

                using (Process process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    if (!process.Start())
                    {
                        ServiceHelper.WriteLog($"Inner Python Script executed UNuccessfully.");
                        return;
                    }

                    process.WaitForExit();
                    process.Close();
                    ServiceHelper.WriteLog($"Inner Python Script executed successfully.");
                }
            }
            catch (System.Exception e)
            {
                ServiceHelper.WriteLog($"Inner Script error{e.Message}");
            }
        }
    }
}
