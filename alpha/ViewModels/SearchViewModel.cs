using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace alpha.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel()
        {
            Title = "Search";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }
    }
}