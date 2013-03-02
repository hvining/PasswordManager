using PasswordManager.Infrastructure;
using PasswordManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordManager.UILogic.ViewModels
{
    public class PasswordItemViewModel : ViewModel, IPasswordViewModel
    {
        private string _accountName;
        private string _userName;
        private string _password;
        private int _id;

        public PasswordItemViewModel(PasswordItem passwordItem)
        {
            _id = passwordItem.Id;
            _accountName = passwordItem.AccountName;
            _userName = passwordItem.UserName;
            _password = passwordItem.Password;

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

        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                this.SetProperty(ref _accountName, value);
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
            }
        }
    }
}
