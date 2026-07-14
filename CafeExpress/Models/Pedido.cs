using System;
using System.Collections.Generic;
using System.Linq;

namespace CafeExpress.Models
{
    /// <summary>Ciclo de vida del pedido.</summary>
    public enum EstadoPedido
    {
        Recibido,
        EnPreparacion,
        Listo,
        Entregado
    }

    /// <summary>Una partida del pedido: producto y cantidad.</summary>
    public class DetallePedido
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => Producto.Precio * Cantidad;
        public string Resumen => $"{Cantidad} x {Producto.Nombre}  (${Subtotal:N2})";
    }

    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreCliente { get; set; }
        public DateTime Fecha { get; set; }
        public EstadoPedido Estado { get; set; }
        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();

        public decimal Total => Detalles.Sum(d => d.Subtotal);

        public string Resumen =>
            string.Join(", ", Detalles.Select(d => $"{d.Cantidad}x {d.Producto.Nombre}"));

        public string Descripcion =>
            $"Pedido #{Id} — {NombreCliente} — {Fecha:dd/MM/yyyy HH:mm} — Total: ${Total:N2}";

        public string EstadoTexto =>
            Estado == EstadoPedido.EnPreparacion ? "En preparación" : Estado.ToString();
    }
}
