using System.IO;
using System;
using SQLite;
using alpha.Tables;
using alpha.Views;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace alpha.Services
{
    public class CredentialsService : ICredentialsService
    {
        private Dictionary<string, string> users;

        public CredentialsService()
        {
            users = new Dictionary<string, string>();
            users.Add("test", "123");
            users.Add("admin", "admin123");
        }

        public async Task<bool> CheckLogin(string userName, string password)
        {
            var database_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDB.db");
            var db = new SQLiteConnection(database_path);
            var query = db.Table<RegisteredUserTable>().Where(user => user.UserName.Equals(userName) && user.Password.Equals(password)).FirstOrDefault();

            if (query != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}