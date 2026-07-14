using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CafeExpress.Models;
using CafeExpress.Services;
using Xamarin.Forms;

namespace CafeExpress.ViewModels
{
    /// <summary>
    /// Catálogo de productos y armado del pedido (carrito).
    /// </summary>
    public class MenuViewModel : BaseViewModel
    {
        public ObservableCollection<Producto> Productos { get; } =
            new ObservableCollection<Producto>();

        public ObservableCollection<DetallePedido> Carrito { get; } =
            new ObservableCollection<DetallePedido>();

        public string TotalTexto =>
            $"Total del pedido: ${Carrito.Sum(d => d.Subtotal):N2}";

        public bool PuedeCrearPedido =>
            RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.CrearPedido);

        public Command<Producto> AgregarCommand { get; }
        public Command<DetallePedido> QuitarCommand { get; }
        public Command ConfirmarPedidoCommand { get; }

        public MenuViewModel()
        {
            AgregarCommand = new Command<Producto>(Agregar);
            QuitarCommand = new Command<DetallePedido>(Quitar);
            ConfirmarPedidoCommand = new Command(async () => await ConfirmarPedidoAsync());

            foreach (var producto in DataService.ObtenerProductos())
                Productos.Add(producto);
        }

        private void Agregar(Producto producto)
        {
            var existente = Carrito.FirstOrDefault(d => d.Producto.Id == producto.Id);
            if (existente != null)
            {
                // Reemplazar la partida para que la lista se refresque en pantalla
                var indice = Carrito.IndexOf(existente);
                Carrito[indice] = new DetallePedido
                {
                    Producto = producto,
                    Cantidad = existente.Cantidad + 1
                };
            }
            else
            {
                Carrito.Add(new DetallePedido { Producto = producto, Cantidad = 1 });
            }
            OnPropertyChanged(nameof(TotalTexto));
        }

        private void Quitar(DetallePedido detalle)
        {
            Carrito.Remove(detalle);
            OnPropertyChanged(nameof(TotalTexto));
        }

        private async Task ConfirmarPedidoAsync()
        {
            // Segunda validación del permiso (nivel de lógica)
            if (!RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.CrearPedido))
                return;

            if (Carrito.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Pedido vacío", "Agregue al menos un producto.", "Aceptar");
                return;
            }

            var pedido = DataService.RegistrarPedido(Carrito.ToList());
            Carrito.Clear();
            OnPropertyChanged(nameof(TotalTexto));

            await Application.Current.MainPage.DisplayAlert(
                "Pedido confirmado",
                $"Su pedido #{pedido.Id} fue registrado con estado \"Recibido\".",
                "Aceptar");
        }
    }
}
