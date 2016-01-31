using System;

namespace DrawShip.Viewer
{
	public interface IOwinHost : IDisposable
	{
		int Port { get; }
	}
}
