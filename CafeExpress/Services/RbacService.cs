using System.Collections.Generic;
using CafeExpress.Models;

namespace CafeExpress.Services
{
    /// <summary>
    /// Implementación en código de la matriz RBAC (Role Based Access Control).
    /// Cada rol tiene asignado un conjunto de permisos; los usuarios heredan
    /// los permisos del rol al que pertenecen.
    /// </summary>
    public static class RbacService
    {
        private static readonly Dictionary<RolUsuario, HashSet<Permiso>> Matriz =
            new Dictionary<RolUsuario, HashSet<Permiso>>
            {
                [RolUsuario.Administrador] = new HashSet<Permiso>
                {
                    Permiso.VerMenu, Permiso.CrearPedido, Permiso.VerPedidosPropios,
                    Permiso.VerTodosLosPedidos, Permiso.ActualizarEstadoPedido,
                    Permiso.GestionarProductos, Permiso.GestionarUsuarios
                },
                [RolUsuario.Empleado] = new HashSet<Permiso>
                {
                    Permiso.VerMenu, Permiso.CrearPedido, Permiso.VerPedidosPropios,
                    Permiso.VerTodosLosPedidos, Permiso.ActualizarEstadoPedido
                },
                [RolUsuario.Cliente] = new HashSet<Permiso>
                {
                    Permiso.VerMenu, Permiso.CrearPedido, Permiso.VerPedidosPropios
                }
            };

        /// <summary>Único punto del sistema donde se decide si una acción está autorizada.</summary>
        public static bool TienePermiso(Usuario usuario, Permiso permiso)
            => usuario != null && Matriz[usuario.Rol].Contains(permiso);
    }
}
