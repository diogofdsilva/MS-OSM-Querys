using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using MS.OSM.Querys.DTO;

namespace MS.OSM.Querys.OSM2DB.OSMFileReader
{

    public class XmlOSMFileLoader : IDisposable
    {
        private int _numberNodes, _numberWays, _relations;

        private Dictionary<long ,Node> _nodes;
        private ConcurrentBag<Way> _ways;
        private XmlReader _reader;
        private List<string> _filters;

        public XmlOSMFileLoader(ConcurrentBag<Way> ways)
        {
            _nodes = new Dictionary<long, Node>();
            _ways = ways;
        }

        private void Initialize(string path, List<string> filters)
        {
            _filters = filters;
            _numberWays = _numberNodes = _relations = 0;
            _reader = XmlReader.Create(path);
        }

        public Task BeginRead(string path, List<string> filters)
        {
            return Task.Factory.StartNew(() => Read(path,filters));
        }

        public void Read(string path, List<string> filters)
        {
            Initialize(path,filters);
            
            // skip boring stuff  
            _reader.MoveToContent();

            _reader.ReadStartElement("osm");

            while (_reader.IsStartElement())
            {
                string name = _reader.Name;

                switch (name)
                {
                    case "bounds":
                        ReadBound();
                        break;
                    case "node":
                        ReadNode();
                        break;
                    case "way":
                        ReadWay();
                        break;
                    case "relation":
                        ReadRelation();
                        break;
                    default:
                        throw new Exception("Unknown element encounterd " + name);
                }
            }

            _reader.ReadEndElement(); // osm  

            //Console.WriteLine("nodes {0} ways {1} relations {2}", _numberNodes, _numberWays, _relations);
        }

        private void ReadBound()
        {
            _reader.ReadStartElement();
            _reader.Skip();
        }

        private void ReadNode()
        {
            Node node = new Node();

            node.Id = Convert.ToInt64(_reader.GetAttribute("id"));
            node.Lat = String2Coordinate(_reader.GetAttribute("lat"));
            node.Lon = String2Coordinate(_reader.GetAttribute("lon"));

            _reader.ReadStartElement(); // node  
            _reader.Skip();
            _numberNodes++;
            


            while (_reader.Name == "tag")
            {
                _reader.ReadStartElement();
                _reader.Skip();
            }

            if (_reader.NodeType == XmlNodeType.EndElement)
                _reader.ReadEndElement(); // node  


            _nodes.Add(node.Id,node);
        }

  
        private void ReadWay()
        {
            Way way = new Way();
            way.Nodes = new List<Node>();

            way.Id = Convert.ToInt64(_reader.GetAttribute("id"));

            _reader.ReadStartElement(); // way  
            _reader.Skip();
            _numberWays++;
            

            while (_reader.Name == "nd")
            {
                long l = Convert.ToInt64(_reader.GetAttribute("ref"));
                

                if (!way.Nodes.Any(p => p.Id == l))
                {
                    way.Nodes.Add(_nodes[l]);        
                }

                _reader.ReadStartElement();
                _reader.Skip();
            }
            
            while (_reader.Name == "tag")
            {
                //ir à procura dos types e do ref

                string key = _reader.GetAttribute("k");
                string value = _reader.GetAttribute("v");

                if (key == "ref")
                {
                    way.Ref = value;
                }

                if (key == "name")
                {
                    way.Name = value;
                }

                if (key == "highway")
                {
                    way.Type = value;
                }

                if (key == "oneway")
                {
                    if (value.ToLowerInvariant() == "yes".ToLowerInvariant())
                    {
                        way.OneWay = true;   
                    }
                    else
                    {
                        way.OneWay = false;
                    }
                }
                
                _reader.ReadStartElement();
                _reader.Skip();
            }

            if (_reader.NodeType == XmlNodeType.EndElement)
                _reader.ReadEndElement(); // way  

            if (_filters.Contains(way.Type))
            {
                _ways.Add(way);    
            }
        }

        private void ReadRelation()
        {
            // get attributes  
            string id = _reader.GetAttribute("id");

            _reader.ReadStartElement(); // relation  
            _reader.Skip();
            _relations++;

            while (_reader.Name == "member")
            {
                _reader.ReadStartElement();
                _reader.Skip();
            }
            while (_reader.Name == "tag")
            {
                _reader.ReadStartElement();
                _reader.Skip();
            }

            if (_reader.NodeType == XmlNodeType.EndElement)
                _reader.ReadEndElement(); // relation  
        }

        private double String2Coordinate(string source)
        {
            return Convert.ToDouble(source.Replace('.',','));
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _reader.Close();
            
        }

        #endregion
    }
}