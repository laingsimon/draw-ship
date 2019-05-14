using System;
using System.Net;
using System.Windows.Forms;

namespace DrawShip.Viewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var applicationContext = new ApplicationContext();
                var runMode = applicationContext.GetRunMode();

                runMode.Run(applicationContext);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error running command:\r\n" + exc.Message, "DrawShip", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.ExitCode = -1;
            }
        }
    }
}
