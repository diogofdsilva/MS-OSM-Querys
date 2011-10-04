using System;
using System.Collections.Generic;
using System.Linq;
using MS.OSM.Querys.DAL.EF.Model;
using MS.OSM.Querys.DAL.IDataMappers;
using MS.OSM.Querys.DTO;

namespace MS.OSM.Querys.DAL.EF.DataMappers
{
    

    public class EFWaysDataMapper : IWaysDataMapper
    {
        private OSMEntities _entities;

        public EFWaysDataMapper(OSMEntities entities)
        {
            // <pex>
            if (entities == null)
                throw new ArgumentNullException("entities");
            // </pex>
            _entities = entities;
        }
        
        #region Implementation of IDalMapper<Way,List<Way>,long>

        public Way Add(Way e)
        {
            _entities.Ways.AddObject(e);
            return e;
        }

        public Way Get(long key)
        {
            return _entities.Ways.SingleOrDefault(p => p.Id == key);
        }

        public Way GetClosestWay(double lat, double lon)
        {
            return _entities.GetClosestWay(lat, lon).FirstOrDefault();
        }

        public List<Way> GetAll()
        {
            return _entities.Ways.ToList();
        }

        public Way Update(Way e)
        {
            _entities.Ways.ApplyCurrentValues(e);
            return e;
        }

        public Way Delete(long key)
        {
            Way way = null;
            
            way = _entities.Ways.SingleOrDefault(p => p.Id == key);

            if (way != null)
            {
                _entities.Ways.DeleteObject(way);    
            }
            
            return way;
        }

        #endregion
    }
}
