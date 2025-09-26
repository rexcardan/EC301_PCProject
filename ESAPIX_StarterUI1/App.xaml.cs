using CommonServiceLocator;
using ESAPIX.AppKit.Overlay;
using ESAPIX.Common;
using ESAPIX.Common.Args;
using ESAPIX.Interfaces;
using ESAPIX.Services;
using ESAPIX_WPF_Example.Helpers;
using ESAPX_StarterUI.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Events;
using Prism.Ioc;
using Serilog.Extensions.Logging;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ESAPX_StarterUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private IEventAggregator _ea;
        private IESAPIService _esapiServ;

        //Disable if you don't want patient selection
        public bool IsPatientSelectionEnabled { get; } = true;

        private string[] _args;

        protected override void OnStartup(StartupEventArgs e)
        {
            _args = e.Args;
            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            var win = ServiceLocator.Current.GetInstance<MainView>();

            EventHandler addSelection = null;

            addSelection = new EventHandler((o, args) =>
            {
                // Place the patient selection UI below the TitleBar using the host defined in MainView.xaml
                var selectPat = new SelectPatient();
                var selectPatContent = (FrameworkElement)selectPat.Content;
                selectPatContent.DataContext = selectPat;
                selectPat.Content = null;

                var host = (ContentControl)win.FindName("PatientSelectorHost");
                if (host != null)
                {
                    host.Content = selectPatContent;
                }
                win.ContentRendered -= addSelection;
            });

            if (IsPatientSelectionEnabled)
            {
                win.ContentRendered += addSelection;
            }
            return win;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _esapiServ = new ESAPIService(() => VMS.TPS.Common.Model.API.Application.CreateApplication());
            //args = ContextIO.ReadArgsFromFile(@"context.txt");

            if (_args != null)
            {
                _esapiServ.Execute(sac =>
                {
                    ArgContextSetter.Set(sac, _args);
                });
            }

            _ea = Container.Resolve<IEventAggregator>();
            containerRegistry.RegisterInstance(this._esapiServ);
            containerRegistry.RegisterInstance(this.Container);
            containerRegistry.RegisterInstance(LogHelper.GetLogger(toConsole: true, toFile: true));
            containerRegistry.RegisterInstance(new SerilogLoggerFactory(Serilog.Log.Logger));
            containerRegistry.RegisterInstance(typeof(ILogger<>), typeof(Logger<>));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                AppComThread.Instance.Dispose();
            }
            catch (Exception ex)
            {

            }
            base.OnExit(e);
        }
    }
}
