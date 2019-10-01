using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TTMS.Data.Abstractions;

namespace TTMS.Data.Azure
{
    public class AzureCloudFactory : IAzureCloudFactory
    {
        public CloudTable CreateCloudTable(string connectionString, string tableName)
        {
            CloudStorageAccount storageAccount = CreateStorageAccount(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            return tableClient.GetTableReference(tableName);
        }

        public CloudStorageAccount CreateStorageAccount(string connectionString)
        {
            return CloudStorageAccount.Parse(connectionString);
        }
    }
}
