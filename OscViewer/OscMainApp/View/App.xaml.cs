using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Win32;

namespace Oscilloscope.View
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string BasePath {  get; private set; }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                //TODO нужно для отладки просмотрщика
                //OpenFileDialog op = new OpenFileDialog();
                //if (op.ShowDialog().GetValueOrDefault())
                //{
                //    BasePath = op.FileName;
                //}
                string res = e.Args.Aggregate(string.Empty, (current, arg) => current + " " + arg);
                BasePath = res;
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
            }
        }


        private void App_OnExit(object sender, ExitEventArgs e)
        {
            if (!string.IsNullOrEmpty(BasePath))
            {
                if (string.Compare(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "TempOsc"),Path.GetDirectoryName(BasePath),true)==0  )
                {
                    File.Delete(Path.ChangeExtension(BasePath, "hdr"));
                    File.Delete(Path.ChangeExtension(BasePath, "cfg"));
                    File.Delete(Path.ChangeExtension(BasePath, "dat"));
                }
            }
        }
    }
}
