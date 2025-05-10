using Tigrin.Views;

namespace Tigrin
{
    public partial class App : Application
    {
        public App(TransactionList listPage)
        {
            InitializeComponent();

            MainPage = new NavigationPage (listPage);
        }
    }
}
