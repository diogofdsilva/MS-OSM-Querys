using System.Collections.Generic;
using MS.OSM.Querys.DAL.Shared;
using MS.OSM.Querys.DTO;

namespace MS.OSM.Querys.DAL.IDataMappers
{
    public interface INodesDataMapper : IDalMapper<Node, List<Node>, long>
    {
        Node GetClosestNode(double lat, double lon);
        Node GetClosestWayNode(double lat, double lon, long wayId);
        List<Node> GetAllFromWay(long wayId);
    }
}