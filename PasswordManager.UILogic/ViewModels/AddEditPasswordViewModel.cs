using PasswordManager.Infrastructure;
using PasswordManager.Data;
using PasswordManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordManager.UILogic.Helpers;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Input;
using Windows.System;

namespace PasswordManager.UILogic.ViewModels
{
    public class AddEditPasswordViewModel : ViewModel, INavigationAware
    {
        private INavigationService _navigationService;
        private IPasswordRepository _passwordRepository;
        private int _id;
        private string _accountName;
        private string _userName;
        private string _password;

        public AddEditPasswordViewModel(INavigationService navigationService, IPasswordRepository passwordRepository)
        {
            _navigationService = navigationService;
            _passwordRepository = passwordRepository;

            SaveCommand = new DelegateCommand(() => Save(), () => CanSave);
            CopyToClipboardCommand = new DelegateCommand(() => CopyToClipboard());
            GoBackCommand = new DelegateCommand(() => navigationService.GoBack(), () => navigationService.CanGoBack());

            KeyPressAction = (eventArgs) =>
                {
                    switch (eventArgs.Key)
                    {
                        case VirtualKey.Enter:
                            {
                                if(CanSave)
                                    SaveCommand.Execute();
                            }
                            break;
                        default:
                            break;
                    }
                };
        }

        private void Save()
        {
            PasswordItem password = new PasswordItem()
            {
                Id = this.Id,
                AccountName = this.AccountName,
                UserName = this.UserName,
                Password = CryptographyHelper.Encrypt(this.Password, CryptographyHelper.Key)
            };
            _passwordRepository.SavePassword(password);

            _navigationService.GoBack();
        }

        private void CopyToClipboard()
        {
            DataPackage clipboardPackage = new DataPackage();
            clipboardPackage.SetText(_password);

            Clipboard.SetContent(clipboardPackage);
        }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                this.SetProperty(ref _id, value);
            }
        }

        public String AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                SetProperty(ref _accountName, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                this.SetProperty(ref _userName, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                this.SetProperty(ref _password, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public Action<KeyRoutedEventArgs> KeyPressAction { get; set; }

        public DelegateCommand SaveCommand { get; set; }

        public DelegateCommand GoBackCommand { get; set; }

        public DelegateCommand CopyToClipboardCommand { get; set; }

        void INavigationAware.OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            var viewmodel = navigationParameter as PasswordItemViewModel;

            if (viewmodel != null)
            {
                _id = viewmodel.Id;
                _accountName = viewmodel.AccountName;
                _userName = viewmodel.UserName;
                _password = viewmodel.Password;
            }
        }

        void INavigationAware.OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {

        }

        public bool CanSave
        {
            get
            {
                return !(String.IsNullOrWhiteSpace(AccountName) || 
                            String.IsNullOrWhiteSpace(UserName) || 
                            String.IsNullOrWhiteSpace(Password));
            }
        }
    }
}
