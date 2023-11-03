using System.Threading.Tasks;
using System.Windows;

namespace WarehouseSystem.Utilities;

internal class ConfirmationDialog
{
    public async Task<bool> ShowConfirmationDialog(string message)
    {
        var result = MessageBox.Show(message, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }
}