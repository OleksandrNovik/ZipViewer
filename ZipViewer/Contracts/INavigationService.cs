using Microsoft.UI.Xaml.Controls;

namespace ZipViewer.Contracts
{
    public interface INavigationService
    {
        public Frame? Frame
        {
            get;
            set;
        }
        public void Navigate(object parameter);
    }
}
