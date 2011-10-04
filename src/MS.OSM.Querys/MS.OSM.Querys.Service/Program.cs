using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace SIAT.OSM.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\t ***************************************************");
            Console.WriteLine("\n\t *              OSM - Service                      *\n");
            Console.WriteLine("\t ***************************************************");
            using (ServiceHost host = new ServiceHost(typeof(OSMService))) {
                Console.WriteLine("\n -> Initiating OSM Service");
                host.Open();

                OSMService service = (OSMService) host.SingletonInstance;


                Console.WriteLine(" -> Press 'e' to EXIT and 'r' to Refresh");

                char key;
                do
                {
                    key = Convert.ToChar(Console.Read());
                    
                    switch (key)
                    {
                        case 'r':
                            PrintConnectionStrings((OSMService)host.SingletonInstance);
                            break;
                            
                    }

                } while (key != 'e');

                host.Close();
            }

            
        }

        private static void PrintConnectionStrings(OSMService service)
        {
            if (service == null)
            {
                Console.WriteLine("\nService not Ready");
                return;
            }
            
            Console.WriteLine("\nPrinting connection strings:");

            foreach (var connectionString in service.ConnectionStrings)
            {
                Console.WriteLine(" -> {0}",connectionString);
            }
              
        }
    }
}
