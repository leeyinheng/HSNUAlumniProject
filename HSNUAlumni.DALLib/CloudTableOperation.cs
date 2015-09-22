using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure;
using HSNUAlumni.ModelLib;
using System.IO;
using Excel;
using System.Data;

namespace HSNUAlumni.DALLib
{
    public class CloudTableOperation<T> where T : ITableEntity, new()
    {
        private string TableName = "classmates";

        private CloudTable table; 

        public CloudTableOperation(string _tablename)
        {
            TableName = _tablename;

            table = CreateTableAsync();
        } 
            
        public void AddorUpdateEntity(T entity)
        {
            // Create or reference an existing table
          
            var newEntity =  InsertOrMergeEntityAsync(table, entity);

        }

        public  void AddorUpdateEntityList(IEnumerable<T> list)
        {
             BatchInsertOfCustomerEntitiesAsync(table, list);
        }

        public void DeleteEntity(T entity)
        {
           DeleteEntityAsync(table, entity);
        }

        public void DeleteEntityByKeys(string partitionKey, string rowKey)
        {
            var entity = RetrieveEntityUsingPointQueryAsync(table, partitionKey, rowKey);

            DeleteEntityAsync(table, entity);
        }

        public void DeleteEntityBySingleKey(string partitionKey)
        {
            var list = GetEntityListByPartiontionKey(partitionKey); 

            foreach(var item in list)
            {
                DeleteEntity(item); 
            }

        }

        public T GetEntityByKeys(string partitionKey, string rowKey)
        {
                                  
            var item =  RetrieveEntityUsingPointQueryAsync(table, partitionKey, rowKey);

            return item; 
        }

        public IEnumerable<T> GetEntityListByPartiontionKey(string partitionKey)
        {
            var list = RetrieveEntityListUsingPointQuery(table,partitionKey);

            return list; 
        }

       
        

        #region private functions 

        private static async Task<IEnumerable<Classmate>> PartitionGetAsync(CloudTable table, string partitionKey)
        {
            TableQuery<Classmate> partitionScanQuery = new TableQuery<Classmate>().Where
                (TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            TableContinuationToken token = null;

            var list = new List<Classmate>();

            // Page through the results
            do
            {
                TableQuerySegment<Classmate> segment = await table.ExecuteQuerySegmentedAsync(partitionScanQuery, token);
                token = segment.ContinuationToken;
                foreach (Classmate entity in segment)
                {
                    list.Add(entity);
                }
            }
            while (token != null);

            return list;
        }

        private void BatchInsertOfCustomerEntitiesAsync(CloudTable table, IEnumerable<T> list)
        {
            // Create the batch operation. 
            TableBatchOperation batchOperation = new TableBatchOperation();

            // The following code  generates test data for use during the query samples.  
            foreach (var item in list)
            {
                batchOperation.InsertOrMerge(item);
            }
            
            // Execute the batch operation.
            var results =   table.ExecuteBatch(batchOperation);

        }

        private T RetrieveEntityUsingPointQueryAsync(CloudTable table, string partitionKey, string rowKey)
        {
           
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
          
            TableResult result = table.Execute(retrieveOperation);
             
            return (T)result.Result;
        }

        private  IEnumerable<T>  RetrieveEntityListUsingPointQuery(CloudTable table, string partitionKey)
        {
            var list = new List<T>(); 
            // Create the range query using the fluid API 
            TableQuery<T> rangeQuery = new TableQuery<T>().Where(                                
                                       TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            // Page through the results - requesting 50 results at a time from the server. 
            TableContinuationToken token = null;
            rangeQuery.TakeCount = 50;
            do
            {
                TableQuerySegment<T> segment =  table.ExecuteQuerySegmented<T>(rangeQuery, token);
                token = segment.ContinuationToken;
                foreach (T entity in segment)
                {
                    list.Add(entity); 
                }
            }
            while (token != null);

            return list; 
        }

        private void DeleteEntityAsync(CloudTable table, T deleteEntity)
        {
            if (deleteEntity == null)
            {
                throw new ArgumentNullException("deleteEntity");
            }

            TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

            table.Execute(deleteOperation);
        }

        private T InsertOrMergeEntityAsync(CloudTable table, T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            // Create the InsertOrReplace  TableOperation
         
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
                      
            var result = table.Execute(insertOrMergeOperation);
          
            return (T)result.Result;
          
          
        }

        private CloudTable  CreateTableAsync()
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            
            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(TableName);
            try
            {
                if (table.CreateIfNotExists())
                {
                    Console.WriteLine("Created Table named: {0}", TableName);
                }
                else
                {
                    Console.WriteLine("Table {0} already exists", TableName);
                }
            }
            catch (StorageException)
            {
                throw;
            }

            return table;
        }

        /// <summary>
        /// Validate the connection string information in app.config and throws an exception if it looks like 
        /// the user hasn't updated this to valid values. 
        /// </summary>
        /// <param name="storageConnectionString">Connection string for the storage service or the emulator</param>
        /// <returns>CloudStorageAccount object</returns>
        private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");

                throw;
            }

            return storageAccount;
        }

        #endregion


    }
}
