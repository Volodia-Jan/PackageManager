using PackageDataContext.Entities;
using System.Data.SQLite;
using System.Text;

namespace PackageDataContext;
internal class PackagesDataContext
{
    private readonly string _connectionString;
    public PackagesDataContext(string connectionString)
    {
        _connectionString = connectionString;
        CreatePackageTable();
    }

    public async Task<int> Insert(Package package)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();
        using var insertDataCommand = new SQLiteCommand(@$"INSERT INTO main.Packages (SenderName, ReceiverName, Description, Weight) 
VALUES ('{package.SenderName}', '{package.ReceiverName}', '{package.Description}', {package.Weight})", connection);
        var rowsChanges = await insertDataCommand.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        
        return rowsChanges;
    }

    public async Task<List<Package>> Select(int limit = default)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();
        var selectDataQuery = "SELECT * FROM main.Packages ";
        if (limit > 0)
            selectDataQuery += $"LIMIT={limit}";

        using var selectDataCommand = new SQLiteCommand(selectDataQuery, connection);
        using var dataReader = await selectDataCommand.ExecuteReaderAsync();

        List<Package> packages = new();
        if (dataReader.HasRows)
        {
            while (dataReader.Read())
            {
                Package package = new()
                {
                    Id = dataReader.GetInt16(0),
                    SenderName = dataReader.GetString(1),
                    ReceiverName = dataReader.GetString(2),
                    Description = dataReader.GetString(3),
                    Weight = dataReader.GetInt32(4)
                };
                packages.Add(package);
            }
        }
        await connection.CloseAsync();

        return packages;
    }

    public async Task<Package?> SelectById(int id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();
        using var selectDataQuery = new SQLiteCommand($"SELECT * FROM main.Packages WHERE id={id}", connection);
        Package? package = null;
        var reader = await selectDataQuery.ExecuteReaderAsync();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                package = new()
                {
                    Id = reader.GetInt16(0),
                    SenderName = reader.GetString(1),
                    ReceiverName = reader.GetString(2),
                    Description = reader.GetString(3),
                    Weight = reader.GetInt32(4)
                };
            }
        }
        await connection.CloseAsync();
        return package;
    }

    public async Task<Package> Update(Package package)
    {
        if (await SelectById(package.Id) == null)
            throw new ArgumentException($"Package was not found by Id:{package.Id}");

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();
        using var updateDataCommand = new SQLiteCommand(@$"UPDATE main.Packages SET SenderName='{package.SenderName}', ReceiverName='{package.ReceiverName}',
Description='{package.Description}', Weight={package.Weight} WHERE Id={package.Id}", connection);

        await updateDataCommand.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        return package;
    }

    public async Task<bool> DeleteById(int id) 
    {
        if (await SelectById(id) == null)
            throw new ArgumentException($"Package was not found by Id:{id}");

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();
        using var deleteDataCommand = new SQLiteCommand($"DELETE FROM main.Packages WHERE id={id}", connection);

        var isDeleted = await deleteDataCommand.ExecuteNonQueryAsync() > 0;
        await connection.CloseAsync();
        return isDeleted;
    }

    private void CreatePackageTable()
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        using var createTableCommand = new SQLiteCommand(@$"CREATE TABLE IF NOT EXISTS main.Packages (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
SenderName TEXT, ReceiverName TEXT, Description TEXT, Weight INT)", connection);

        createTableCommand.ExecuteNonQuery();
        connection.Close();
    }
}
