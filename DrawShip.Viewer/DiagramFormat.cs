namespace DrawShip.Viewer
{
	/// <summary>
	/// The format/renderer to use to render a drawing
	/// </summary>
	public enum DiagramFormat
	{
		/// <summary>
		/// Render the drawing in an html/interactive preview
		/// </summary>
		Html,

		/// <summary>
		/// Render the drawing as an image (png)
		/// </summary>
		Image,

		/// <summary>
		/// Render the drawing in a compatible method for print
		/// </summary>
		Print
	}
}
