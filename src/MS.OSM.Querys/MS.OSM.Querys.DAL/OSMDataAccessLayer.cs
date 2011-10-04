using System;
using MS.OSM.Querys.DAL.EF.DataMappers;
using MS.OSM.Querys.DAL.EF.Model;
using MS.OSM.Querys.DAL.IDataMappers;
using MS.OSM.Querys.DAL.Shared;

namespace MS.OSM.Querys.DAL
{
    public class OSMDataAccessLayer : IUnitOfWork
    {
        private readonly OSMEntities _entities;
        private bool _saved;
        public OSMDataAccessLayer(string connectionString)
        {
            _entities = new OSMEntities(connectionString);
        }
        
        public OSMDataAccessLayer()
        {
            _entities = new OSMEntities();
        }

        public static bool TestConnectionString(string connection)
        {
            try 
            {
                var temp = new OSMEntities(connection);
                return temp.DatabaseExists();
            }
            catch(Exception)
            {
                return false;
            }
        }

        private INodesDataMapper _nodes;

        public INodesDataMapper Nodes
        { 
            get { return _nodes ?? (_nodes = new EFNodesDataMapper(_entities)); }
        }

        private IWaysDataMapper _ways;

        public IWaysDataMapper Ways
        {
            get { return _ways ?? (_ways = new EFWaysDataMapper(_entities)); }
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (!_saved)
            {
                Commit();
            }

            _entities.Dispose();
        }

        #endregion

        public void Commit()
        {
            _saved = true;
            _entities.SaveChanges();
        }

        public void Rollback()
        {
            
        }
    }
}
