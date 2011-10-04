using System.Collections.Generic;
using MS.OSM.Querys.DAL.Shared;
using MS.OSM.Querys.DTO;

namespace MS.OSM.Querys.DAL.IDataMappers
{
    public interface IWaysDataMapper : IDalMapper<Way, List<Way>, long>
    {
        Way GetClosestWay(double lat, double lon);
    }
}