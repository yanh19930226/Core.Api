using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Api.Dtos
{
    public class CreateBookDto
    {
        public string BookName { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
