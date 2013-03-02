using PasswordManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Data
{
    public interface IPasswordRepository
    {
        IEnumerable<PasswordGroup> GetAllPasswords();

        void SavePassword(PasswordItem password);

        void DeletePassword(PasswordItem _selectedPasswordItem);
    }
}
