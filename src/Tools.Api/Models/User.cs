using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Api.Models
{
    [Serializable]
    public class User
    {
         public int id { get; set; }
         public string username { get; set; }
         public string password { get; set; }
         public int enabled { get; set; }
     }
}
