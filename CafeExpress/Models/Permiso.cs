namespace CafeExpress.Models
{
    /// <summary>Permisos que pueden asignarse a los roles en la matriz RBAC.</summary>
    public enum Permiso
    {
        VerMenu,
        CrearPedido,
        VerPedidosPropios,
        VerTodosLosPedidos,
        ActualizarEstadoPedido,
        GestionarProductos,
        GestionarUsuarios
    }
}
