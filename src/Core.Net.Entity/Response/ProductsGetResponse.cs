using Core.Net.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.Response
{
    public class ProductsGetResponse : APIResponse
    {
        public List<ProductsGetDomain> Domains { get; set; }
    }
}
