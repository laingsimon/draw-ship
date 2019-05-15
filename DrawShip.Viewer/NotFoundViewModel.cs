namespace DrawShip.Viewer
{
    public class NotFoundViewModel : IndexViewModel
    {
        public NotFoundViewModel(HostingContext hostingContext, int directoryKey, string requestedFile) 
            : base(hostingContext, directoryKey)
        {
            RequestedFile = requestedFile;
        }

        public string RequestedFile { get; }
    }
}
