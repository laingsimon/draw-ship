using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DrawShip.Viewer
{
	public partial class HostingDetail : Form
	{
		private readonly HostingContext _hostingContext;

		public HostingDetail(HostingContext detail)
		{
			_hostingContext = detail;
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			lblHostingDetail.Text = string.Format(
				"http://localhost:{0}",
				_hostingContext.Port);

			_hostingContext.OnApplicationStarted();
			Text = "DrawShip";
			icoSystemTray.Text = string.Format(
				"DrawShip: {0}",
				_hostingContext.Port);
		}

		private void HostingDetail_Shown(object sender, EventArgs e)
		{
			Hide();
		}

		private void lblHostingDetail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_hostingContext.DisplayIndex();
		}

		/// <summary>
		/// Accept windows messages from another process, if the message is WM_COPYDATA, then process it
		/// WM_COPYDATA messages will contain a ShowDiagramCommand structure with the information on what should be performed.
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == NativeMethods.WM_COPYDATA)
				_AcceptData(m);

			base.WndProc(ref m);
		}

		/// <summary>
		/// Unwrap the ShowDiagramCommand from the windows message and ask the HostingContext to execute it
		/// </summary>
		/// <param name="m"></param>
		private void _AcceptData(Message m)
		{
			var copyStruct = (NativeMethods.COPYDATASTRUCT)m.GetLParam(typeof(NativeMethods.COPYDATASTRUCT));
			var commandJson = Marshal.PtrToStringAnsi(copyStruct.lpData);
			var command = JsonConvert.DeserializeObject<ShowDiagramStructure>(commandJson);

			_hostingContext.ExecuteCommand(command);
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

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void itmExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
