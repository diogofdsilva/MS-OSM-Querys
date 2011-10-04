using System.Data.Objects;
using MS.OSM.Querys.DTO;

namespace MS.OSM.Querys.DAL.EF.Model
{
    public class OSMEntities : ObjectContext
    {
        private const string ContainerName = "OSMEntities";

        public OSMEntities()
            : base("name=OSMEntities", ContainerName)  
        {
            
        }

        public OSMEntities(string connectionString)
            : base(connectionString, ContainerName)
        {
            
        }

        public ObjectSet<Node> Nodes
        {
            get
            {
                return _nodes ?? (_nodes = CreateObjectSet<Node>());
            }
        }
        private ObjectSet<Node> _nodes;

        public ObjectSet<Way> Ways
        {
            get
            {
                return _ways ?? (_ways = CreateObjectSet<Way>());
            }
        }
        private ObjectSet<Way> _ways;

        public ObjectResult<Node> GetClosestNode(double latitude,double longitude)
        {
            return base.ExecuteFunction<Node>("GetClosestNode",
                                                  new ObjectParameter("lat", latitude),
                                                  new ObjectParameter("lon", longitude));
        }

        public ObjectResult<Way> GetClosestWay(double latitude, double longitude)
        {
            return base.ExecuteFunction<Way>("GetClosestWay",
                                                  new ObjectParameter("lat", latitude),
                                                  new ObjectParameter("lon", longitude));
        }

        public ObjectResult<Node> GetClosestWayNode(double latitude, double longitude, long idWay)
        {
            return base.ExecuteFunction<Node>("GetClosestWayNode",
                                                  new ObjectParameter("lat", latitude),
                                                  new ObjectParameter("lon", longitude),
                                                  new ObjectParameter("idWay", idWay));
        }
    }
}
