namespace DrawShip.Viewer
{
    internal class NullRunMode : IRunMode
    {
        public bool Run(ApplicationContext applicationContext)
        {
            return true;
        }
    }
}
