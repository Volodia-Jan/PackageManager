using PackageDataContext.Entities;

namespace PackageDataContext;
public class PackageDataService
{
    private readonly PackagesDataContext _db;

    public PackageDataService()
    {
        _db = new PackagesDataContext(@"Data Source=C:\Users\Vova_Hula\source\repos\PackageManager\packagesdb.db");
    }

    public async Task<List<Package>> GetAllPackages()
        => await _db.Select();

    public async Task<List<Package>> GetAllPackages(int limit)
        => await _db.Select(limit);

    public async Task<List<Package>> UpdateRequest(List<Package> packages)
    {
        var packagesInBd = await GetAllPackages();

        foreach (var package in packages)
        {
            var matchesPackage = packagesInBd.FirstOrDefault(p => p.Id == package.Id);
            if (matchesPackage != null && !matchesPackage.Equals(package))
                await _db.Update(package);
            else if (matchesPackage == null)
                await _db.Insert(package);
            //else
            //    await _db.DeleteById(package.Id);
        }

        return await GetAllPackages();
    }
}
