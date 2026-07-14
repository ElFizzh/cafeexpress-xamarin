using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CafeExpress.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrincipalPage : ContentPage
    {
        public PrincipalPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}
