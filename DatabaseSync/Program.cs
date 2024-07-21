using System;
using System.Linq;

namespace DatabaseSync
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var centralContext = new CentralDbContext())
            using (var clientContext = new ClientDbContext())
            {
                // Step 1: Get data from central database
                var centralData = centralContext.DataItems.ToList();

                // Step 2: Get data from client database
                var clientData = clientContext.DataItems.ToList();

                // Step 3: Determine changes to upload to central
                foreach (var clientItem in clientData)
                {
                    var centralItem = centralData.FirstOrDefault(c => c.Id == clientItem.Id);
                    if (centralItem == null)
                    {
                        // New item in client, add to central
                        centralContext.DataItems.Add(clientItem);
                    }
                    else if (clientItem.LastUpdated > centralItem.LastUpdated)
                    {
                        // Updated item in client, update central
                        centralItem.Name = clientItem.Name;
                        centralItem.LastUpdated = clientItem.LastUpdated;
                    }
                }

                // Step 4: Save changes to central database
                centralContext.SaveChanges();

                // Step 5: Determine changes to download to client
                foreach (var centralItem in centralData)
                {
                    var clientItem = clientData.FirstOrDefault(c => c.Id == centralItem.Id);
                    if (clientItem == null)
                    {
                        // New item in central, add to client
                        clientContext.DataItems.Add(centralItem);
                    }
                    else if (centralItem.LastUpdated > clientItem.LastUpdated)
                    {
                        // Updated item in central, update client
                        clientItem.Name = centralItem.Name;
                        clientItem.LastUpdated = centralItem.LastUpdated;
                    }
                }

                // Step 6: Save changes to client database
                clientContext.SaveChanges();

                Console.WriteLine("Synchronization complete.");
            }
        }
    }
}
