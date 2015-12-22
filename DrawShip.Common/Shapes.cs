using System;
using System.Configuration;

namespace DrawShip.Common
{
	public static class Shapes
	{
		static Shapes()
		{
			var shapeNames = ConfigurationManager.AppSettings["ShapeNames"];

			if (!string.IsNullOrEmpty(shapeNames))
				Names = shapeNames.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>
		/// The names of the shapes that should be included in the previews
		/// </summary>
		public static string[] Names { get; set; }

		public static string GetNames()
		{
			if (Names == null)
				return string.Empty;

			return string.Join(";", Names);
		}
	}
}
