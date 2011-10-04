using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MS.OSM.Querys.OSM2DB.Loader
{
    public class DBLoader
    {

        //private static SqlConnection connection = new SqlConnection(QUERY_STRING);

        private ConcurrentBag<DTO.Way> ways;

        public DBLoader(ConcurrentBag<DTO.Way> ways, string queryString)
        {
            this.ways = ways;
            this._queryString = queryString;
        }


        /*public Task[] LoadNodes()
        {

            Action action = () =>
                                {
                                    int n = 0;
                                    Node node;
                                    while (true)
                                    {
                                        if (n > 50)
                                        {
                                            break;
                                        }

                                        if (nodes.TryTake(out node))
                                        {
                                            //Task.Factory.StartNew(obj => LoadNode((OSMNode) obj),node);
                                            LoadNode(node);
                                            n = 0;
                                        }

                                        if (nodes.IsEmpty)
                                        {
                                            ++n;
                                            Thread.SpinWait(10);
                                        }
                                    }
                                };
            

            Task[] tasks = new Task[_numberOfWorkers];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Factory.StartNew(action);
            }

            return tasks;
        }*/

        private string _queryString;

        public string QueryString
        {
            set { _queryString = value; }
        }

        //private const string INSERT_NODE = @"insert into Node (id,lat,lon) values (@id,@lat,@lon)";
        //private const string INSERT_WAY = @"insert into Way (id,[type],ref,name,country,oneway) values (@id,@type,@ref,@name,@country,@oneway)";

        private const string INSERT_NODE = @"insert into Node (id,lat,lon,orderId,wayId) values ({0},{1},{2},{3},{4}); ";
        private const string INSERT_WAY = @"insert into Way (id,[type],ref,name,country,oneway) values ({0},{1},{2},{3},{4},{5}); ";

        public Task[] BeginLoadWays(int numberOfWorkers)
        {
#region teste
/*
            Action action = () =>
            {
                DTO.Way way;
                int n = 0;

                using (SIATConnection siatConnection = new SIATConnection())
                {
                    while (true)
                    {
                        if (ways.TryTake(out way))
                        {
                            Way efWay = new Way()
                            {
                                id = way.Id,
                                name = way.Name,
                                @ref = way.Ref,
                                type = way.Type,
                                country = way.Country,
                                oneway = way.OneWay
                            };

                            efWay.Nodes = new EntityCollection<Node>();

                            short orderId = 0;
                            foreach(DTO.Node e in way.Nodes)
                            {
                                efWay.Nodes.Add(new Node()
                                                    {
                                                        id = e.Id,
                                                        wayId = way.Id,
                                                        lat = e.Lat,
                                                        lon = e.Lon,
                                                        orderId = orderId
                                                    });
                                ++orderId;
                            }

                            siatConnection.AddToWays(efWay);
                            ++n;
                        }

                        if (n >= 300)
                        {
                            try
                            {
                                siatConnection.SaveChanges();
                                
                            }
                            catch (Exception)
                            {
                                Debug.WriteLine("Error");
                            }
                            
                            n = 0;
                        }

                        if (ways.IsEmpty)
                        {
                            break;
                        }
                    }
                    
                    siatConnection.SaveChanges();
                }
                
            };
            */
            #endregion teste

            Action actioB = () =>
            {
                DTO.Way way;
                int n = 0;
                SqlCommand command;
                StringBuilder stringCommandBuilder = new StringBuilder();
                using (SqlConnection siatConnection = new SqlConnection(_queryString))
                {
                    siatConnection.Open();

                    while (true)
                    {
                        command = siatConnection.CreateCommand();

                        if (ways.TryTake(out way))
                        {
                            stringCommandBuilder.AppendFormat(INSERT_WAY, way.Id, way.Type.ToStringNullable(), way.Ref.ToStringNullable(), way.Name.ToStringNullable(), way.Country.ToStringNullable(), way.OneWay.ToStringSql());

                            short orderId = 0;
                            foreach (DTO.Node e in way.Nodes)
                            {
                                stringCommandBuilder.AppendFormat(INSERT_NODE, e.Id, e.Lat.ToCoordinate(), e.Lon.ToCoordinate(),orderId,way.Id);
                                ++orderId;
                            }
                            ++n;
                        }

                        if (n >= 20)
                        {
                            try
                            {
                                command.CommandText = stringCommandBuilder.ToString();
                                command.ExecuteNonQuery();
                            }
                            catch (Exception)
                            {
                                Debug.WriteLine("Error");
                            }

                            stringCommandBuilder.Clear();
                            n = 0;
                        }

                        if (ways.IsEmpty)
                        {
                            break;
                        }
                    }
                    
                    command.CommandText = stringCommandBuilder.ToString();
                    if (!string.IsNullOrEmpty(command.CommandText))
                    {
                        command.ExecuteNonQuery();    
                    }

                    siatConnection.Close();
                }

            };

            Task[] tasks = new Task[numberOfWorkers];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Factory.StartNew(actioB);
            }

            return tasks;
        }
    }

    public static class MyExtensions
    {
        public static string ToCoordinate(this double d)
        {
            return d.ToString().Replace(',', '.');
        }

        public static string ToStringNullable(this string d)
        {
            if (d == null)
            {
                return "NULL";
            }

            return "'"+d+"'";
        }

        public static string ToStringSql(this bool d)
        {
            if (d)
            {
                return "1";    
            }
            else
            {
                return "0";
            }
        }
    }
    
}

