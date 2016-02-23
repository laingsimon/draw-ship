using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;

namespace DrawShip.Viewer
{
	/// <summary>
	/// Run mode for opening a drawing using another process
	/// </summary>
	public class OpenDrawingInOtherHostRunMode : IRunMode
	{
		public bool Run(ApplicationContext applicationContext)
		{
			var hwnd = NativeMethods.FindWindow(null, "DrawShip");

			if (hwnd == IntPtr.Zero)
				return false;

			var command = new ShowDiagramStructure
			{
				FileName = applicationContext.FileName,
				Directory = applicationContext.WorkingDirectory,
				Format = applicationContext.Format
			};
			var commandJson = JsonConvert.SerializeObject(command);

			var copyData = new NativeMethods.COPYDATASTRUCT
			{
				dwData = IntPtr.Zero,
				lpData = Marshal.StringToHGlobalAnsi(commandJson),
				cbData = commandJson.Length + 1
			};
			var copyDataBuff = copyData.AllocatePointer();
			NativeMethods.SendMessage(hwnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, copyDataBuff);
			copyDataBuff.FreePointer();
			copyDataBuff = IntPtr.Zero;

			return true;
		}
	}
}
