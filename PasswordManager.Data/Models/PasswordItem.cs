using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Data.Models
{
    public class PasswordItem
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public String AccountName { get; set; }

        public String UserName { get; set; }

        public String Password { get; set; }

        public DateTime Modified { get; set; }
    }
}
