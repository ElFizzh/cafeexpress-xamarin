namespace CafeExpress.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }

        /// <summary>Hash SHA-256 de la contraseña. Nunca se guarda en texto plano.</summary>
        public string ContrasenaHash { get; set; }

        public RolUsuario Rol { get; set; }
    }
}
