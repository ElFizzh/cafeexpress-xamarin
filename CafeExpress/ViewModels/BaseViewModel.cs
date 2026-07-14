using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CafeExpress.ViewModels
{
    /// <summary>
    /// Clase base de todos los ViewModels. Implementa INotifyPropertyChanged,
    /// mecanismo por el cual las Vistas se actualizan automáticamente (data binding).
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string nombre = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));

        protected bool SetProperty<T>(ref T campo, T valor, [CallerMemberName] string nombre = null)
        {
            if (EqualityComparer<T>.Default.Equals(campo, valor))
                return false;

            campo = valor;
            OnPropertyChanged(nombre);
            return true;
        }
    }
}
