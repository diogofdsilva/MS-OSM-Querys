using System;

namespace MS.OSM.Querys.DAL.Shared
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}