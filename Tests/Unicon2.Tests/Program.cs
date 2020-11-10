using Prism.Ioc;
using Unicon2.Shell;
using Unicon2.Shell.ViewModels;

namespace Unicon2.Tests
{
    public static class Program
    {

        private static App _app;

        static void Main(string[] args)
        {
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