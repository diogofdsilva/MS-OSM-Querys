using System.Runtime.Serialization;

namespace MS.OSM.Querys.DTO
{
    [DataContract]
    public class Node
    {
        [DataMember]
        private long _id;
        [DataMember]
        private double _lat;
        [DataMember]
        private double _lon;
        [DataMember]
        private long _wayId;
        [DataMember]
        private short _orderId;


        public Node(long id, double lat, double lon, long wayId, short orderId)
        {
            this._id = id;
            this._lat = lat;
            this._lon = lon;
            this._wayId = wayId;
            this._orderId = orderId;
        }

        public Node()
        {
        }

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public double Lat
        {
            get { return _lat; }
            set { _lat = value; }
        }

        public double Lon
        {
            get { return _lon; }
            set { _lon = value; }
        }

        public long WayId
        {
            get { return _wayId; }
            set { _wayId = value; }
        }

        public short OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        public Way Way { get; set; }
    }
}
