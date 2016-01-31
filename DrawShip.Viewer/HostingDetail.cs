using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DrawShip.Viewer
{
	public partial class HostingDetail : Form
	{
		private HostingContext _hostingContext;

		public HostingDetail(HostingContext detail)
		{
			_hostingContext = detail;
			InitializeComponent();
		}

		internal static string GetWindowTitle(ApplicationContext applicationContext)
		{
			return string.Format("DrawShip:{0}", applicationContext.WorkingDirectory);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			lblHostingDetail.Text = string.Format(
				"http://localhost:{0}",
				_hostingContext.Port);

			_hostingContext.ApplicationStarted();
			Text = GetWindowTitle(_hostingContext.ApplicationContext);
			icoSystemTray.Text = string.Format(
				"DrawShip: {0} ({1})",
				_hostingContext.ApplicationContext.WorkingDirectory,
				_hostingContext.Port);
		}

		private void HostingDetail_Shown(object sender, EventArgs e)
		{
			Hide();
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void lblHostingDetail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_hostingContext.DisplayIndex();
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == NativeMethods.WM_COPYDATA)
				AcceptData(m);

			base.WndProc(ref m);
		}

		private void AcceptData(Message m)
		{
			var copyStruct = (NativeMethods.COPYDATASTRUCT)m.GetLParam(typeof(NativeMethods.COPYDATASTRUCT));
			var commandJson = Marshal.PtrToStringAnsi(copyStruct.lpData);
			var command = JsonConvert.DeserializeObject<ShowDiagramStructure>(commandJson);

			_hostingContext.DisplayDrawing(command.FileName, command.Version);
		}

		private void icoSystemTray_DoubleClick(object sender, EventArgs e)
		{
			Show();
		}

		private void HostingDetail_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
			}
		}
	}
}
