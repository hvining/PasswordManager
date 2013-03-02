using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Data.Models
{
    public class PasswordGroup
    {
        public String GroupKey { get; set; }

        public IEnumerable<PasswordItem> Passwords { get; set; }
    }
}
