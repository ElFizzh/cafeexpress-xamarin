using System;
using System.Security.Cryptography;
using System.Text;
using CafeExpress.Models;

namespace CafeExpress.Services
{
    /// <summary>
    /// Servicio de autenticación. Valida credenciales contra el hash SHA-256
    /// almacenado y mantiene la sesión activa de la aplicación.
    /// </summary>
    public static class AuthService
    {
        /// <summary>Usuario con sesión activa (null si nadie ha iniciado sesión).</summary>
        public static Usuario UsuarioActual { get; private set; }

        public static bool IniciarSesion(string nombreUsuario, string contrasena)
        {
            var usuario = DataService.BuscarUsuario(nombreUsuario);
            if (usuario == null)
                return false;

            if (usuario.ContrasenaHash != CalcularHash(contrasena))
                return false;

            UsuarioActual = usuario; // sesión activa
            return true;
        }

        public static void CerrarSesion()
        {
            UsuarioActual = null;
        }

        /// <summary>Calcula el hash SHA-256 de un texto (usado para las contraseñas).</summary>
        public static string CalcularHash(string texto)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(texto));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
