using System;
using System.Collections.Generic;
using System.Linq;
using CafeExpress.Models;

namespace CafeExpress.Services
{
    /// <summary>
    /// Acceso a datos de la aplicación. En esta versión los datos se mantienen
    /// en memoria (colecciones locales) para operar sin conexión.
    /// </summary>
    public static class DataService
    {
        private static int _siguientePedidoId = 1;
        private static int _siguienteProductoId = 6;

        private static readonly List<Usuario> Usuarios = new List<Usuario>
        {
            new Usuario { Id = 1, NombreUsuario = "admin",    NombreCompleto = "Administrador General",
                          ContrasenaHash = AuthService.CalcularHash("admin123"),    Rol = RolUsuario.Administrador },
            new Usuario { Id = 2, NombreUsuario = "empleado", NombreCompleto = "Empleado de Mostrador",
                          ContrasenaHash = AuthService.CalcularHash("empleado123"), Rol = RolUsuario.Empleado },
            new Usuario { Id = 3, NombreUsuario = "cliente",  NombreCompleto = "Cliente de Prueba",
                          ContrasenaHash = AuthService.CalcularHash("cliente123"),  Rol = RolUsuario.Cliente }
        };

        private static readonly List<Producto> Productos = new List<Producto>
        {
            new Producto { Id = 1, Nombre = "Café americano",     Descripcion = "Taza de 12 oz de café de grano",         Categoria = "Bebidas calientes", Precio = 35m },
            new Producto { Id = 2, Nombre = "Capuchino",          Descripcion = "Espresso con leche vaporizada y espuma", Categoria = "Bebidas calientes", Precio = 48m },
            new Producto { Id = 3, Nombre = "Frappé de moka",     Descripcion = "Bebida fría de café con chocolate",      Categoria = "Bebidas frías",     Precio = 55m },
            new Producto { Id = 4, Nombre = "Panini de jamón",    Descripcion = "Pan artesanal con jamón y queso",        Categoria = "Alimentos",         Precio = 65m },
            new Producto { Id = 5, Nombre = "Rebanada de pastel", Descripcion = "Pastel de chocolate de la casa",         Categoria = "Postres",           Precio = 45m }
        };

        private static readonly List<Pedido> Pedidos = new List<Pedido>();

        // ----- Usuarios -----
        public static Usuario BuscarUsuario(string nombreUsuario)
            => Usuarios.FirstOrDefault(u =>
                   u.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase));

        // ----- Productos -----
        public static List<Producto> ObtenerProductos()
            => Productos.OrderBy(p => p.Categoria).ThenBy(p => p.Nombre).ToList();

        public static void AgregarProducto(Producto producto)
        {
            producto.Id = _siguienteProductoId++;
            Productos.Add(producto);
        }

        public static void EliminarProducto(Producto producto)
            => Productos.Remove(producto);

        // ----- Pedidos -----
        public static Pedido RegistrarPedido(List<DetallePedido> detalles)
        {
            var pedido = new Pedido
            {
                Id = _siguientePedidoId++,
                UsuarioId = AuthService.UsuarioActual.Id,
                NombreCliente = AuthService.UsuarioActual.NombreCompleto,
                Fecha = DateTime.Now,
                Estado = EstadoPedido.Recibido,
                Detalles = new List<DetallePedido>(detalles)
            };
            Pedidos.Add(pedido);
            return pedido;
        }

        public static List<Pedido> ObtenerTodosLosPedidos()
            => Pedidos.OrderByDescending(p => p.Fecha).ToList();

        public static List<Pedido> ObtenerPedidosDeUsuario(int usuarioId)
            => Pedidos.Where(p => p.UsuarioId == usuarioId)
                      .OrderByDescending(p => p.Fecha)
                      .ToList();
    }
}
