using System.ComponentModel;

namespace RTASS.Common.ViewModels
{
    /// <summary>
    /// Tüm ViewModel'ler için temel sınıf
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
