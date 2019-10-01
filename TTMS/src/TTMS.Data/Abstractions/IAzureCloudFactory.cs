using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace TTMS.Data.Abstractions
{
    public interface IAzureCloudFactory
    {
        CloudStorageAccount CreateStorageAccount(string connectionString);

        CloudTable CreateCloudTable(string connectionString, string tableName);
    }
}