using System.Threading.Tasks;
using CafeExpress.Services;
using CafeExpress.Views;
using Xamarin.Forms;

namespace CafeExpress.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _nombreUsuario;
        private string _contrasena;
        private string _mensajeError;

        public string NombreUsuario
        {
            get => _nombreUsuario;
            set => SetProperty(ref _nombreUsuario, value);
        }

        public string Contrasena
        {
            get => _contrasena;
            set => SetProperty(ref _contrasena, value);
        }

        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }

        public Command IniciarSesionCommand { get; }

        public LoginViewModel()
        {
            IniciarSesionCommand = new Command(async () => await IniciarSesionAsync());
        }

        private async Task IniciarSesionAsync()
        {
            MensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrWhiteSpace(Contrasena))
            {
                MensajeError = "Capture su usuario y contraseña.";
                return;
            }

            if (AuthService.IniciarSesion(NombreUsuario.Trim(), Contrasena))
            {
                NombreUsuario = string.Empty;
                Contrasena = string.Empty;
                await Application.Current.MainPage.Navigation.PushAsync(new PrincipalPage());
            }
            else
            {
                MensajeError = "Usuario o contraseña incorrectos.";
            }
        }
    }
}
