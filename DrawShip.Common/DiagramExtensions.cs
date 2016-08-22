using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DrawShip.Common
{
	/// <summary>
	/// Helper methods for Diagrams
	/// </summary>
	public static class DiagramExtensions
	{
		/// <summary>
		/// Get the names of the shapes that are used within a diagram
		/// </summary>
		/// <param name="drawing"></param>
		/// <param name="fileSystem"></param>
		/// <param name="version"></param>
		/// <returns></returns>
		public static string[] GetContainedShapeNames(this Drawing drawing, IFileSystem fileSystem, string version)
		{
			using (var rawDrawingStream = fileSystem.OpenRead(drawing, version))
			using (var drawingStream = CompressedXmlStream.Read(rawDrawingStream))
			{
				if (drawingStream == null)
					return new string[0];

				var xml = XDocument.Load(drawingStream);
				var cellsWithStyle = xml.XPathSelectElements("//mxCell");
				var cellShapes = (from cell in cellsWithStyle
								  let style = cell.Attribute("style")?.Value
								  let shape = _GetShape(style)
								  where !string.IsNullOrEmpty(shape)
								  select shape).Distinct().ToArray();

				return cellShapes;
			}
		}

		private static string _GetShape(string style)
		{
			if (string.IsNullOrEmpty(style))
				return null;

			var styleDictionary = (from pair in style.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
								   let match = Regex.Match(pair, @"^(?<key>.+?)=(?<value>.+?)$")
								   where match.Success && match.Groups["key"].Value == "shape"
								   select new { key = match.Groups["key"].Value, value = match.Groups["value"].Value })
								  .ToDictionary(a => a.key, a => a.value);

			return styleDictionary.ContainsKey("shape")
				? _GetShapeName(styleDictionary["shape"])
				: null;
		}

		private static string _GetShapeName(string fullShapeName)
		{
			var match = Regex.Match(fullShapeName, @"^mxgraph\.(?<shape>.+?)\.(?<category>[^.]+)$");
			return match.Success
				? match.Groups["shape"].Value.Replace(".", "/")
				: null;
		}
	}
}
