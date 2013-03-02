using PasswordManager.Data.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Data
{
    public class PasswordRepository : IPasswordRepository
    {
        private string _connectionString;

        public PasswordRepository()
        {
            _connectionString = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, Constants.SQLiteDBFile);

            SetupTables();
        }

        private void SetupTables()
        {
            using (SQLiteConnection context = new SQLiteConnection(_connectionString))
            {
                var passwordTable = (from p in context.TableMappings
                                     where p.TableName == Constants.PasswordItemTableName
                                     select p).FirstOrDefault();

                if (passwordTable == null)
                    context.CreateTable<PasswordItem>();
            }
        }
        public IEnumerable<PasswordGroup> GetAllPasswords()
        {
            using(SQLiteConnection context = new SQLiteConnection(_connectionString))
            {
                IEnumerable<PasswordGroup> group = from p in context.Table<PasswordItem>()
                                                   group p by p.AccountName.Substring(0,1) into pg
                                                   orderby pg.Key ascending
                                                   select new PasswordGroup()
                                                   {
                                                       GroupKey = pg.Key.ToUpper(),
                                                       Passwords = pg
                                                   };

                return group.ToList();
            }
        }


        public void SavePassword(PasswordItem password)
        {
            using (SQLiteConnection context = new SQLiteConnection(_connectionString))
            {
                var existingPassword = (from p in context.Table<PasswordItem>()
                                       where p.Id == password.Id
                                       select p).FirstOrDefault();


                if (existingPassword != null)
                {
                    context.Update(password);
                }
                else
                {
                    context.Insert(password);
                }
            }
        }


        public void DeletePassword(PasswordItem _selectedPasswordItem)
        {
            using (SQLiteConnection context = new SQLiteConnection(_connectionString))
            {
                context.Delete(_selectedPasswordItem);
            }
        }
    }
}
