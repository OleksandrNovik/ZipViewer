using CommunityToolkit.Mvvm.Input;
using ICommand = System.Windows.Input.ICommand;

namespace ZipViewer.Helpers.Extensions;
public static class CommandExtensions
{
    public static void ExecuteIfCan(this ICommand command, object commandParameter)
    {
        if (command.CanExecute(commandParameter))
        {
            command.Execute(commandParameter);
        }
    }

    public static async Task ExecuteIfCanAsync(this IAsyncRelayCommand command, object commandParameter)
    {
        if (command.CanExecute(commandParameter))
        {
            await command.ExecuteAsync(commandParameter);
        }
    }
}
