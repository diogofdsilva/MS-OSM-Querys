using System;
using System.Collections.Generic;
using System.Linq;
using MS.OSM.Querys.DAL.EF.Model;
using MS.OSM.Querys.DAL.IDataMappers;
using MS.OSM.Querys.DTO;

namespace MS.OSM.Querys.DAL.EF.DataMappers
{
    public class EFNodesDataMapper : INodesDataMapper
    {
        private OSMEntities _entities;

        public EFNodesDataMapper(OSMEntities entities)
        {
            // <pex>
            if (entities == (OSMEntities)null)
                throw new ArgumentNullException("entities");
            // </pex>
            _entities = entities;
        }
        
        #region Implementation of IDalMapper<Nodes,List<Nodes>,int>

        public Node Add(Node e)
        {
            _entities.Nodes.AddObject(e);

            return e;
        }
        
        public Node Get(long key)
        {  
            return _entities.Nodes.SingleOrDefault(o => o.Id == key);   
        }
        
        public Node GetClosestNode(double lat, double lon)
        {
            return _entities.GetClosestNode(lat, lon).FirstOrDefault();
        }

        public Node GetClosestWayNode(double lat, double lon, long wayId)
        {
            return _entities.GetClosestWayNode(lat, lon, wayId).FirstOrDefault();
        }

        public List<Node> GetAll()
        {
            return _entities.Nodes.ToList();
        }

        public List<Node> GetAllFromWay(long wayId)
        {
            return _entities.Nodes.Where(w => w.Id == wayId).ToList();
        }

        public Node Update(Node e)
        {
            throw new NotImplementedException();
        }

        public Node Delete(long key)
        {
            Node n = _entities.Nodes.SingleOrDefault(o => o.Id == key);
            _entities.Nodes.DeleteObject(n);

            return n;
        }

        #endregion
    }
}
