using PasswordManager.Infrastructure;
using PasswordManager.Data;
using PasswordManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordManager.UILogic.Helpers;
using Windows.ApplicationModel.DataTransfer;

namespace PasswordManager.UILogic.ViewModels
{
    public class HubPageViewModel : ViewModel, INavigationAware
    {
        private ObservableCollection<PasswordGroup> _passwordGroups;
        private INavigationService _navigationService;
        private IPasswordRepository _passwordRepository;
        private PasswordItem _selectedPasswordItem;

        public HubPageViewModel(INavigationService navigationService, IPasswordRepository passwordRepository)
        {
            _navigationService = navigationService;
            _passwordRepository = passwordRepository;

            _passwordGroups = new ObservableCollection<PasswordGroup>();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                PasswordGroups.Add(new PasswordGroup()
                {
                    GroupKey = "A"
                });
                PasswordGroups.Add(new PasswordGroup()
                {
                    GroupKey = "C"
                });
                PasswordGroups.Add(new PasswordGroup()
                {
                    GroupKey = "D"
                });
            }
            else
            {
                GetData();
            }

            AddNewPasswordCommand = new DelegateCommand(() => AddNewPassword());
            DeleteCommand = new DelegateCommand(() => Delete());
            CopyToClipboardCommand = new DelegateCommand(() => CopyToClipboard());
            GoBackCommand = new DelegateCommand(() => _navigationService.GoBack(), () => _navigationService.CanGoBack());
        }

        #region Properties
        public ObservableCollection<PasswordGroup> PasswordGroups
        {
            get { return _passwordGroups; }
            set { SetProperty(ref this._passwordGroups, value); }
        }

        public PasswordItem SelectedPasswordItem
        {
            get
            {
                return _selectedPasswordItem;
            }
            set
            {
                SetProperty(ref _selectedPasswordItem, value);
            }
        } 
        #endregion

        #region Commands
        public DelegateCommand AddNewPasswordCommand { get; set; }

        private void AddNewPassword()
        {
            //Show new password flyout
            _navigationService.Navigate("AddEditPassword", true);
        }

        public DelegateCommand DeleteCommand { get; set; }

        private void Delete()
        {
            _passwordRepository.DeletePassword(_selectedPasswordItem);

            GetData();
        }

        public DelegateCommand CopyToClipboardCommand { get; set; }

        private void CopyToClipboard()
        {
            DataPackage clipboardPackage = new DataPackage();
            clipboardPackage.SetText(SelectedPasswordItem.Password);
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(clipboardPackage);
        }
        #endregion

        public Action<object> EditPasswordAction
        {
            get
            {
                return (parameter) =>
                    {
                        var password = parameter as PasswordItem;

                        PasswordItemViewModel viewModel = new PasswordItemViewModel(password);

                        _navigationService.Navigate("AddEditPassword", viewModel);
                    };
            }
        }

        public DelegateCommand GoBackCommand { get; set; }

        private void GetData()
        {
            PasswordGroups.Clear();

            foreach (var passwordGroup in _passwordRepository.GetAllPasswords())
            {
                foreach (var password in passwordGroup.Passwords)
                {
                    password.Password = CryptographyHelper.Decrypt(password.Password, CryptographyHelper.Key);
                }

                PasswordGroups.Add(passwordGroup);
            }
        }

        #region INavigation Implementation
        void INavigationAware.OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            GetData();
        }

        void INavigationAware.OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
        } 
        #endregion
    }
}
