using System;
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
			var applicationContext = new ApplicationContext();
			var runMode = applicationContext.GetRunMode();

			runMode.Run(applicationContext);
		}
	}
}
