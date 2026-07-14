using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CafeExpress.Models;
using CafeExpress.Services;
using Xamarin.Forms;

namespace CafeExpress.ViewModels
{
    /// <summary>
    /// Consulta de pedidos. El alcance de la consulta depende del rol:
    /// Cliente ve solo sus pedidos; Empleado y Administrador ven todos.
    /// </summary>
    public class PedidosViewModel : BaseViewModel
    {
        public ObservableCollection<Pedido> Pedidos { get; } =
            new ObservableCollection<Pedido>();

        public bool PuedeActualizarEstado =>
            RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.ActualizarEstadoPedido);

        public string Titulo =>
            RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.VerTodosLosPedidos)
                ? "Todos los pedidos"
                : "Mis pedidos";

        public Command<Pedido> AvanzarEstadoCommand { get; }

        public PedidosViewModel()
        {
            AvanzarEstadoCommand = new Command<Pedido>(async p => await AvanzarEstadoAsync(p));
            CargarPedidos();
        }

        private void CargarPedidos()
        {
            Pedidos.Clear();

            var lista = RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.VerTodosLosPedidos)
                ? DataService.ObtenerTodosLosPedidos()
                : DataService.ObtenerPedidosDeUsuario(AuthService.UsuarioActual.Id);

            foreach (var pedido in lista)
                Pedidos.Add(pedido);
        }

        private async Task AvanzarEstadoAsync(Pedido pedido)
        {
            // Segunda validación del permiso (nivel de lógica)
            if (!RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.ActualizarEstadoPedido))
                return;

            if (pedido.Estado == EstadoPedido.Entregado)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Pedido entregado", "Este pedido ya llegó al final de su ciclo.", "Aceptar");
                return;
            }

            pedido.Estado = pedido.Estado + 1; // Recibido -> En preparación -> Listo -> Entregado
            CargarPedidos();
        }
    }
}
