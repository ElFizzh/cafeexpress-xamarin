using CafeExpress.Views;
using Xamarin.Forms;

namespace CafeExpress
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // La aplicación siempre inicia en la pantalla de inicio de sesión:
            // ninguna funcionalidad es accesible sin autenticarse (RF-01).
            MainPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Color.FromHex("#5D4037"),
                BarTextColor = Color.White
            };
        }
    }
}
