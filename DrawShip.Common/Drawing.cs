namespace DrawShip.Common
{
	public class Drawing
	{
		public Drawing(string fileName, string filePath)
		{
			FileName = fileName;
			FilePath = filePath;
		}

		public string FileName { get; }
		public string FilePath { get; }
	}
}
