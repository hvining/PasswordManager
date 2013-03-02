using PasswordManager.Infrastructure;
using PasswordManager.Data;
using PasswordManager.UILogic.ViewModels;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PasswordManager
{
    public partial class App : Application
    {
        private static FrameNavigationService CreateNavigationService(Frame frame)
        {
            var sessionStateWrapper = new FrameSessionStateWrapper();

            Func<string, Type> navigationResolver = (string pageToken) =>
            {
                // We set a custom namespace for the View
                var viewNamespace = "PasswordManager.Views";

                var viewFullName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}Page", viewNamespace, pageToken);
                var viewType = Type.GetType(viewFullName);

                return viewType;
            };

            var navigationService = new FrameNavigationService(frame, sessionStateWrapper, navigationResolver);
            return navigationService;
        }

        private static IPasswordRepository CreatePasswordDataRepository()
        {
            return new PasswordRepository();
        }
        public void BootstrapApplication(INavigationService navigationService)
        {
            ViewModelLocator.Register(typeof(HubPage), () => new HubPageViewModel(navigationService, _passwordRepository));
            ViewModelLocator.Register(typeof(AddEditPasswordPage), () => new AddEditPasswordViewModel(navigationService, _passwordRepository));
        }
    }
}
