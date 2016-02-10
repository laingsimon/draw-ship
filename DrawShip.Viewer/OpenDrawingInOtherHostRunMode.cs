using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;

namespace DrawShip.Viewer
{
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
			IntPtr copyDataBuff = _IntPtrAlloc(copyData);
			NativeMethods.SendMessage(hwnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, copyDataBuff);
			_IntPtrFree(ref copyDataBuff);

			return true;
		}

		// Allocate a pointer to an arbitrary structure on the global heap.
		private static IntPtr _IntPtrAlloc<T>(T param)
		{
			IntPtr retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
			Marshal.StructureToPtr(param, retval, false);
			return retval;
		}

		// Free a pointer to an arbitrary structure from the global heap.
		private static void _IntPtrFree(ref IntPtr preAllocated)
		{
			if (IntPtr.Zero == preAllocated)
				return;

			Marshal.FreeHGlobal(preAllocated);
			preAllocated = IntPtr.Zero;
		}
	}
}
