using System.Collections.Generic;
using System.ServiceModel;
using MS.OSM.Querys.DTO;

namespace MS.OSM.Querys
{
    [ServiceContract]
    public interface IOSMService
    {
        [OperationContract]
        Way GetWay(long id);

        [OperationContract]
        Node GetNode(long id);

        [OperationContract]
        List<Node> GetWayNodes(long wayId);

        [OperationContract]
        Node GetClosestWayNode(double lat, double lon, long wayId);

        [OperationContract]
        Way GetClosestWay(double lat, double lon);

        [OperationContract]
        Node GetClosestNode(double lat, double lon);
    }
}
