using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace DrawShip.Viewer
{
    /// <summary>
    /// Run mode for hosting owin in the current process/application
    /// If another process is running, then a command will be sent to the other process to execute the hosting
    /// </summary>
    public class SelfHostRunMode : IRunMode
    {
        private static readonly Mutex _mutex = new Mutex(true, "A727D06E-77C3-4760-AC74-C1D76DD11B91");
        private readonly IRunMode _openInOtherInstance;

        public SelfHostRunMode(IRunMode openInOtherInstance)
        {
            _openInOtherInstance = openInOtherInstance;
        }

        public bool Run(ApplicationContext applicationContext)
        {
            if (!_mutex.WaitOne(TimeSpan.FromSeconds(1)))
            {
                var openedInOtherInstance = _openInOtherInstance.Run(applicationContext);
                if (openedInOtherInstance)
                    return true;
            }

            try
            {
                if (applicationContext.Format == DiagramFormat.Print)
                {
                    applicationContext.PrintDrawing(new ShowDiagramStructure
                    {
                        Format = applicationContext.Format,
                        Directory = applicationContext.WorkingDirectory,
                        FileName = applicationContext.FileName
                    });
                    return true;
                }

                using (var owinHost = WebApiStartup.Start(GetUrls()))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    var form = new HostingDetail(
                        new HostingContext(
                            applicationContext,
                            5142));
                    Application.Run(form);
                }

                return true;
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        private static IEnumerable<string> GetUrls()
        {
            var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
                yield return string.Format("http://{0}:{1}", Environment.MachineName, 5142);

            yield return string.Format("http://localhost:{0}", 5142);
        }
    }
}
