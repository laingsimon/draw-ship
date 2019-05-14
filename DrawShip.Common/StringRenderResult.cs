using System.IO;

namespace DrawShip.Common
{
    public class StringRenderResult : IRenderResult
    {
        private readonly string _output;

        public StringRenderResult(string output)
        {
            _output = output;
        }

        public void WriteResult(Stream output)
        {
            using (var writer = new StreamWriter(output))
                writer.Write(_output);
        }
    }
}
