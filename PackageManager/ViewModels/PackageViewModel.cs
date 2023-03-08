using PackageDataContext;
using PackageDataContext.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PackageManager.ViewModels;

internal class PackageViewModel : ViewModelBase
{
    private readonly PackageDataService _packageDataService;
    private ObservableCollection<Package> _packages;

    public ObservableCollection<Package> Packages
    {
        get => _packages;
        set
        {
            _packages = value;
            OnPropertyChanged(nameof(Packages));
        }
    }

    public PackageViewModel()
    {
        _packageDataService = new PackageDataService();
        _packages = new ObservableCollection<Package>();
        Task.Run(() => LoadData());
    }

    public async Task SaveCommand(List<Package> packages)
    {
        var updatedPackages = await _packageDataService.UpdateRequest(packages);
        Packages = new ObservableCollection<Package>(updatedPackages);
    }

    public async Task CancelCommand() => await LoadData();

    private async Task LoadData()
    {
        var packagesList = await _packageDataService.GetAllPackages();
        Packages = new ObservableCollection<Package>(packagesList);
    }
}
