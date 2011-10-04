using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MS.OSM.Querys.DTO
{ 
    [DataContract]
    public class Way
    {
        [DataMember]
        private long _id;
        [DataMember]
        private string _ref;
        [DataMember]
        private string _name;
        [DataMember]
        private string _type;
        [DataMember]
        private string _country;
        [DataMember]
        private bool _oneWay;

        public Way(long id, string @ref, string name, string type)
        {
            _id = id;
            _ref = @ref;
            _name = name;
            _type = type;
        }

        public Way()
        {
        }

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Ref
        {
            get { return _ref; }
            set { _ref = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public bool OneWay
        {
            get { return _oneWay; }
            set { _oneWay = value; }
        }

        public List<Node> Nodes { get; set; }
    }
}
