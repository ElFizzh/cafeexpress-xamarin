using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CafeExpress.Models;
using CafeExpress.Services;
using Xamarin.Forms;

namespace CafeExpress.ViewModels
{
    /// <summary>
    /// Gestión del catálogo de productos (altas, ediciones y bajas).
    /// Funcionalidad exclusiva del rol Administrador según la matriz RBAC.
    /// </summary>
    public class AdminProductosViewModel : BaseViewModel
    {
        private Producto _seleccionado;
        private string _nombre;
        private string _descripcion;
        private string _categoria;
        private string _precio;

        public ObservableCollection<Producto> Productos { get; } =
            new ObservableCollection<Producto>();

        public Producto Seleccionado
        {
            get => _seleccionado;
            set
            {
                if (SetProperty(ref _seleccionado, value) && value != null)
                {
                    Nombre = value.Nombre;
                    Descripcion = value.Descripcion;
                    Categoria = value.Categoria;
                    Precio = value.Precio.ToString();
                }
            }
        }

        public string Nombre { get => _nombre; set => SetProperty(ref _nombre, value); }
        public string Descripcion { get => _descripcion; set => SetProperty(ref _descripcion, value); }
        public string Categoria { get => _categoria; set => SetProperty(ref _categoria, value); }
        public string Precio { get => _precio; set => SetProperty(ref _precio, value); }

        public Command NuevoCommand { get; }
        public Command GuardarCommand { get; }
        public Command EliminarCommand { get; }

        public AdminProductosViewModel()
        {
            NuevoCommand = new Command(LimpiarFormulario);
            GuardarCommand = new Command(async () => await GuardarAsync());
            EliminarCommand = new Command(async () => await EliminarAsync());
            CargarProductos();
        }

        private void CargarProductos()
        {
            Productos.Clear();
            foreach (var producto in DataService.ObtenerProductos())
                Productos.Add(producto);
        }

        private void LimpiarFormulario()
        {
            Seleccionado = null;
            Nombre = Descripcion = Categoria = Precio = string.Empty;
        }

        private async Task GuardarAsync()
        {
            // Segunda validación del permiso (nivel de lógica)
            if (!RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.GestionarProductos))
                return;

            if (string.IsNullOrWhiteSpace(Nombre) || !decimal.TryParse(Precio, out var precio))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Datos incompletos", "Capture al menos el nombre y un precio válido.", "Aceptar");
                return;
            }

            if (Seleccionado == null)
            {
                DataService.AgregarProducto(new Producto
                {
                    Nombre = Nombre.Trim(),
                    Descripcion = Descripcion?.Trim(),
                    Categoria = Categoria?.Trim(),
                    Precio = precio
                });
            }
            else
            {
                Seleccionado.Nombre = Nombre.Trim();
                Seleccionado.Descripcion = Descripcion?.Trim();
                Seleccionado.Categoria = Categoria?.Trim();
                Seleccionado.Precio = precio;
            }

            LimpiarFormulario();
            CargarProductos();
        }

        private async Task EliminarAsync()
        {
            if (!RbacService.TienePermiso(AuthService.UsuarioActual, Permiso.GestionarProductos))
                return;

            if (Seleccionado == null)
                return;

            var confirmar = await Application.Current.MainPage.DisplayAlert(
                "Eliminar producto",
                $"¿Desea eliminar \"{Seleccionado.Nombre}\" del menú?",
                "Sí, eliminar", "Cancelar");

            if (!confirmar)
                return;

            DataService.EliminarProducto(Seleccionado);
            LimpiarFormulario();
            CargarProductos();
        }
    }
}
