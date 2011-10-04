using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.Threading;
using MS.OSM.Querys;
using MS.OSM.Querys.DAL;
using MS.OSM.Querys.DTO;

namespace SIAT.OSM.Service
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple, InstanceContextMode=InstanceContextMode.Single)]
    class OSMService : IOSMService
    {
        private int _ticketNumber;
        private string[] _connectionStrings;

        public OSMService()
        { 
            List<string> validConnectionStrings = new List<string>();
            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                if (OSMDataAccessLayer.TestConnectionString(ConfigurationManager.ConnectionStrings[i].ConnectionString))
                    validConnectionStrings.Add(ConfigurationManager.ConnectionStrings[i].ConnectionString);
                
            }

            Console.WriteLine("System validated {0} query strings.",validConnectionStrings.Count);

            _connectionStrings = validConnectionStrings.ToArray();
        }

        public string[] ConnectionStrings
        {
            get { return _connectionStrings; }
        }

        public Way GetWay(long id)
        {
            using (OSMDataAccessLayer data = new OSMDataAccessLayer(GetNextConnection()))
            {
                try
                {
                    return data.Ways.Get(id);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Node GetNode(long id)
        {
            using (OSMDataAccessLayer data = new OSMDataAccessLayer(GetNextConnection()))
            {
                try
                {
                    return data.Nodes.Get(id);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public List<Node> GetWayNodes(long wayId)
        {
            using (OSMDataAccessLayer data = new OSMDataAccessLayer(GetNextConnection()))
            {
                try
                {
                    return data.Nodes.GetAllFromWay(wayId);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Node GetClosestWayNode(double lat, double lon, long wayId)
        {
            using (OSMDataAccessLayer data = new OSMDataAccessLayer(GetNextConnection()))
            {
                try
                {
                    return data.Nodes.GetClosestWayNode(lat, lon, wayId);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        [OperationBehavior]
        public Way GetClosestWay(double lat, double lon)
        {
            DateTime init = DateTime.Now;

            using (OSMDataAccessLayer data = new OSMDataAccessLayer(GetNextConnection()))
            {
                try
                {
                    return data.Ways.GetClosestWay(lat, lon);
                }
                catch (Exception)
                {
                    return null;
                }
            }

        }
        
        [OperationBehavior]
        public Node GetClosestNode(double lat, double lon)
        {
            DateTime init = DateTime.Now;

            using (OSMDataAccessLayer data = new OSMDataAccessLayer(GetNextConnection()))
            {
                try
                {
                    return data.Nodes.GetClosestNode(lat, lon);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


        private string GetNextConnection() 
        {
            int connectionNumber = Interlocked.Increment(ref _ticketNumber);

            connectionNumber = connectionNumber % _connectionStrings.Length;

            return _connectionStrings[connectionNumber];
        }
    }
}
