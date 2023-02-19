using alpha.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace alpha.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}