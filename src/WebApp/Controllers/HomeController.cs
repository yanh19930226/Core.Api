using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class Userinfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; }//id
        public string name { get; set; }//姓名
        public int age { get; set; }//年龄
        public int level { get; set; }//年龄
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Date { get; set; }
        public Ename ename { get; set; }//英文名
    }

    public class Ename
    {
        [BsonElement("firstname")]
        public string ming { get; set; }
        [BsonElement("lastname")]
        public string xing { get; set; }
    }
    /// <summary>
    /// 学生类
    /// </summary
    public class Student
    {
        public string name { get; set; }//姓名
        public int age { get; set; }//年龄
        public int level { get; set; }//班级编号
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult MongoJson()
        {


            //List<MongoServerAddress> list = new List<MongoServerAddress>();
            //MongoServerAddress primary = new MongoServerAddress("120.78.202.190", 27017);
            //MongoServerAddress swc = new MongoServerAddress("120.78.202.190", 27018);
            //list.Add(primary);
            //list.Add(swc);
            //MongoClientSettings mcs = new MongoClientSettings()
            //{
            //    MaxConnectionPoolSize = 500,
            //    MinConnectionPoolSize = 20,
            //    WaitQueueSize = 860,
            //    MaxConnectionIdleTime = new TimeSpan(0, 3, 0),
            //    WaitQueueTimeout = new TimeSpan(0, 1, 0),
            //    Servers = list,
            //    Server = primary,
            //    ReadPreference = ReadPreference.SecondaryPreferred,
            //    //  Credentials = listCredential,
            //    ReplicaSetName = "myrs"
            //};
            ////  mcs.ClusterConfigurator = (Action<ClusterBuilder>)config;
            ////mongoclientsettings.fromurl(new mongourl("mongodb://11.29.17.141:27017"));
            //var client = new MongoClient(mcs);
            //var mydb = (MongoDatabaseBase)client.GetDatabase("tttt");




            ////连接数据库
            ///
            //var client = new MongoClient("mongodb://127.0.0.1:27018,127.0.0.1:27019");
            var client = new MongoClient("mongodb://120.78.202.190:27017,120.78.202.190:27018");
            //获取database
            var mydb = client.GetDatabase("tttt");
            var collection = mydb.GetCollection<Userinfo>("userinfos");
            List<Userinfo> userinfos = collection.Find(p => p.age == 29999).ToList();
            return Json(userinfos);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
