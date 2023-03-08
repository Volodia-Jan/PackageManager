using PackageDataContext.Entities;
using PackageManager.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PackageManager.Pages;
/// <summary>
/// Interaction logic for PackagesPage.xaml
/// </summary>
public partial class PackagesPage : Page
{
    public PackagesPage()
    {
        InitializeComponent();
        DataContext = new PackageViewModel();
    }

    private async void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as PackageViewModel;
        if (viewModel == null)
        {
            MessageBox.Show("Something went wrong! Please try again later.");
            return;
        }
        var packagesUI = PackagesDataGrip.Items.OfType<Package>().ToList();
        await viewModel.SaveCommand(packagesUI);
    }

    private async void CancelBtn_Click(object sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as PackageViewModel;
        if (viewModel == null)
        {
            MessageBox.Show("Something went wrong! Please try again later.");
            return;
        }

        await viewModel.CancelCommand();
    }
}
