using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DrawShip.Viewer
{
    public static class RegAsm
    {
        private static readonly string[] _paths =
        {
            @"c:\Windows\Microsoft.NET\Framework64\v4.0.30319\",
            @"c:\Windows\Microsoft.NET\Framework\v4.0.30319\"
        };

        public static bool Execute(params string[] arguments)
        {
            foreach (var path in _paths)
            {
                var regasmPath = Path.Combine(path, "regasm.exe");
                if (!File.Exists(regasmPath))
                    continue;

                return _ExecuteRegAsm(regasmPath, arguments);
            }

            return false;
        }

        private static bool _ExecuteRegAsm(string regasmPath, string[] arguments)
        {
            var applicationExePath = Environment.GetCommandLineArgs().First();
            var regAsm = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = "\"" + applicationExePath + "\" " + string.Join(" ", arguments),
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = regasmPath,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                }
            };

            regAsm.ErrorDataReceived += _ErrorDataReceived;
            regAsm.OutputDataReceived += _OutputDataReceived;

            regAsm.Start();
            regAsm.BeginOutputReadLine();
            regAsm.BeginErrorReadLine();

            regAsm.WaitForExit();

            return regAsm.ExitCode == 0;
        }

        private static void _OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Trace.TraceInformation(e.Data);
                Console.Out.WriteLine(e.Data);
            }
        }

        private static void _ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Trace.TraceError(e.Data);
                Console.Error.WriteLine(e.Data);
            }
        }
    }
}
