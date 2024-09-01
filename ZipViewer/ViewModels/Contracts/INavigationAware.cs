namespace ZipViewer.ViewModels.Contracts;

public interface INavigationAware
{
    public void OnNavigatedTo(object parameter);
    public void OnNavigatedFrom();
}