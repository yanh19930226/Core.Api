using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Api.Dtos
{
    public class UpdateBookDto : CreateBookDto
    {
        public string Id { get; set; }

        [JsonIgnore]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
