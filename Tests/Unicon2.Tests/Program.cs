using Unicon2.Shell;

namespace Unicon2.Tests
{
    public static class Program
    {

        private static App _app;

        static void Main(string[] args)
        {
            _app = new App();
            _app.InitializePublic();
        }

        public static App GetApp()
        {
            if (_app == null)
            {
                _app = new App();
                _app.InitializePublic();
            }
            return _app;
        }
    }
}