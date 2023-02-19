using alpha.ViewModels;

using System;
using System.Collections.Generic;
using System.IO;
using SQLite;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using alpha.Tables;

namespace alpha.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
            this.BindingContext = new SignupViewModel();
        }
        //void Handle_Clicked(object sender, System.EventArgs e)
        //{
        //    var database_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDB.db");
        //    var db = new SQLiteConnection(database_path);
        //    db.CreateTable<RegisteredUserTable>();

        //    if(passwordEntry.Text != passwordEntry1.Text)
        //    {

        //    }
        //    var temp = new RegisteredUserTable()
        //    {
        //        UserName = emailEntry.Text,
        //        Password = 
        //    }
        //}
    }
}