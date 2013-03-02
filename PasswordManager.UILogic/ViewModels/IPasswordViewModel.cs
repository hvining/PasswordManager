using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.UILogic.ViewModels
{
    public interface IPasswordViewModel
    {
        String AccountName { get; set; }

        String UserName { get; set; }

        String Password { get; set; }
    }
}
