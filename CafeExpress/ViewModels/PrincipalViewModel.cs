using System.Threading.Tasks;
using CafeExpress.Models;
using CafeExpress.Services;
using CafeExpress.Views;
using Xamarin.Forms;

namespace CafeExpress.ViewModels
{
    /// <summary>
    /// Menú principal. Construye las opciones visibles a partir de la matriz RBAC:
    /// las funcionalidades no permitidas para el rol ni siquiera se muestran.
    /// </summary>
    public class PrincipalViewModel : BaseViewModel
    {
        public string Bienvenida =>
            $"Hola, {AuthService.UsuarioActual?.NombreCompleto} ({AuthService.UsuarioActual?.Rol})";

        // Propiedades enlazadas a IsVisible en la Vista (control de acceso en la interfaz)
        public bool PuedeVerMenu =>
            RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.VerMenu);

        public bool PuedeVerPedidos =>
            RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.VerPedidosPropios);

        public bool PuedeGestionarProductos =>
            RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.GestionarProductos);

        public Command IrAMenuCommand { get; }
        public Command IrAPedidosCommand { get; }
        public Command IrAProductosCommand { get; }
        public Command CerrarSesionCommand { get; }

        public PrincipalViewModel()
        {
            IrAMenuCommand = new Command(async () => await NavegarAsync(new MenuPage()));
            IrAPedidosCommand = new Command(async () => await NavegarAsync(new PedidosPage()));
            IrAProductosCommand = new Command(async () => await IrAProductosAsync());
            CerrarSesionCommand = new Command(async () => await CerrarSesionAsync());
        }

        private async Task NavegarAsync(Page pagina)
            => await Application.Current.MainPage.Navigation.PushAsync(pagina);

        private async Task IrAProductosAsync()
        {
            // Segunda validación (nivel de lógica): aunque el botón estuviera visible
            // por error, la operación se bloquea si el rol no tiene el permiso.
            if (!RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.GestionarProductos))
                return;

            await NavegarAsync(new AdminProductosPage());
        }

        private async Task CerrarSesionAsync()
        {
            AuthService.CerrarSesion();
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }
    }
}
