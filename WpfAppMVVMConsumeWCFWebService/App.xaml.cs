using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace WpfAppMVVMConsumeWCFWebService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IUnityContainer container = new UnityContainer();

            Init(container);

            RegisterType(container);
        }

        private void Init(IUnityContainer container)
        {
            var mainViewModel = container.Resolve<MainViewModel>();
            var mainView = new MainWindow
            {
                DataContext = mainViewModel
            };
            mainView.Show();
        }

        private void RegisterType(IUnityContainer container)
        {
            //container.RegisterType<IUser, User>();
        }
    }
}
