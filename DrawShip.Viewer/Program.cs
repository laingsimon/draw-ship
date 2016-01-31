using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using System.Threading;
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
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var applicationContext = new ApplicationContext();
			var singleInstanceMutex = _EnsureSingleInstanceForWorkingDirectory(applicationContext);

			if (singleInstanceMutex == null)
			{
				//send a message to the other instance
				_SendMessageToOtherInstance(applicationContext);
				return; //another instance already running for this directory
			}

			try
			{
				using (var owinHost = new OwinHost())
				{
					var hostingContext = new HostingContext(applicationContext, owinHost);
					var form = new HostingDetail(hostingContext);

					Application.Run(form);
				}
			}
			finally
			{
				singleInstanceMutex.ReleaseMutex();
			}
		}

		private static void _SendMessageToOtherInstance(ApplicationContext applicationContext)
		{
			var title = HostingDetail.GetWindowTitle(applicationContext);
			var hwnd = NativeMethods.FindWindow(null, title);

			if (hwnd == IntPtr.Zero)
				throw new InvalidOperationException("Could not find window to send message to");

			var command = new ShowDiagramStructure
			{
				FileName = applicationContext.FileName,
				Directory = applicationContext.WorkingDirectory
			};
			var commandJson = JsonConvert.SerializeObject(command);

			var copyData = new NativeMethods.COPYDATASTRUCT
			{
				dwData = IntPtr.Zero,
				lpData = Marshal.StringToHGlobalAnsi(commandJson),
				cbData = commandJson.Length + 1
			};
			IntPtr copyDataBuff = IntPtrAlloc(copyData);
			NativeMethods.SendMessage(hwnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, copyDataBuff);
			IntPtrFree(ref copyDataBuff);
		}

		// Allocate a pointer to an arbitrary structure on the global heap.
		public static IntPtr IntPtrAlloc<T>(T param)
		{
			IntPtr retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
			Marshal.StructureToPtr(param, retval, false);
			return retval;
		}

		// Free a pointer to an arbitrary structure from the global heap.
		public static void IntPtrFree(ref IntPtr preAllocated)
		{
			if (IntPtr.Zero == preAllocated)
				throw (new NullReferenceException("Go Home"));
			Marshal.FreeHGlobal(preAllocated);
			preAllocated = IntPtr.Zero;
		}

		private static Mutex _EnsureSingleInstanceForWorkingDirectory(ApplicationContext applicationContext)
		{
			var singleInstanceHash = applicationContext.WorkingDirectory.ToLower().GetHashCode();
			var mutex = new Mutex(true, string.Format("drawShip.Viewer@{0}", singleInstanceHash));
			var singleInstance = mutex.WaitOne(TimeSpan.FromSeconds(1));
			if (singleInstance)
				return mutex;

			return null;
		}
	}
}
