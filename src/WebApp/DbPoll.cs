using MongoDB.Driver;
using MongoDB.Driver.Core.Authentication;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public class DbPoll
    {
        private static DbPoll _instance = null;
        private DbPoll()
        {
            InitDbPoll();
        }
        public static DbPoll CreateInstance()
        {
            if (_instance == null)
            {
                _instance = new DbPoll();
            }
            return _instance;
        }

        private MongoClient client;
        private MongoDatabaseBase database;
        //private void config(ClusterBuilder cbuilder)
        //{
        //    cbuilder = new ClusterBuilder();
        //    List<IAuthenticator> list = new List<IAuthenticator>();
        //    list.Add(new DefaultAuthenticator(new UsernamePasswordCredential("mongodb://11.29.17.141:27017", "root", "root")));
        //    cbuilder.ConfigureConnection((connectionSettings) =>
        //    { return new ConnectionSettings(list, new TimeSpan(0, 3, 0), new TimeSpan(1, 0, 0), "search"); });
        //    cbuilder.ConfigureConnectionPool((connectionPoolSettings) =>
        //    { return new ConnectionPoolSettings(new TimeSpan(0, 30, 0), 90, 20, 400, new TimeSpan(0, 0, 30)); });
        //    cbuilder.ConfigureServer((server) => { return new ServerSettings(new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 5)); });
        //    cbuilder.ConfigureWithConnectionString("mongodb://11.29.17.141:27017");
        //}
        public void InitDbPoll()
        {
            List<MongoServerAddress> list = new List<MongoServerAddress>();
            MongoServerAddress primary = new MongoServerAddress("11.29.17.141", 27017);
            list.Add(primary);
            List<MongoCredential> listCredential = new List<MongoCredential>();
            listCredential.Add(MongoCredential.CreateCredential("search", "root", "root"));
            //listCredential.Add(MongoCredential.CreateCredential("myblog", "root", "root"));
            MongoClientSettings mcs = new MongoClientSettings()
            {
                MaxConnectionPoolSize = 500,
                MinConnectionPoolSize = 20,
                WaitQueueSize = 860,
                MaxConnectionIdleTime = new TimeSpan(0, 3, 0),
                WaitQueueTimeout = new TimeSpan(0, 1, 0),
                Servers = list,
                Server = primary,
                ReadPreference = ReadPreference.SecondaryPreferred,
                //  Credentials = listCredential,
                ReplicaSetName = "rs1"
            };
            //  mcs.ClusterConfigurator = (Action<ClusterBuilder>)config;
            //mongoclientsettings.fromurl(new mongourl("mongodb://11.29.17.141:27017"));
            client = new MongoClient(mcs);
            database = (MongoDatabaseBase)client.GetDatabase("search");
        }
        public MongoClient getClient()
        {
            return client;
        }
        public MongoDatabaseBase getMongoDatabaseBase()
        {
            return database;
        }
        public static readonly DbPoll instance = new DbPoll();
    }

    public class Singleton
    {
        private volatile static Singleton _instance = null;
        private static readonly object lockHelper = new object();
        private Singleton() { }
        public static Singleton CreateInstance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new Singleton();
                }
            }
            return _instance;
        }
    }
}
