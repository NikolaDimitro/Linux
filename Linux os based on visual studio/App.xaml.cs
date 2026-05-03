using System.Windows;

namespace LinuxMintOS
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // SplashScreen is set in App.xaml as StartupUri
        }
    }
}