using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Api.Dtos
{
    /// <summary>
    /// 搜索类
    /// </summary>
    public class MongoSearchDto
    {
        /// <summary>
        /// SearchStr
        /// </summary>
        public string SearchStr { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 店铺Id
        /// </summary>
        public string ShopId { get; set; }
        /// <summary>
        /// 账号Id
        /// </summary>
        public string AccountId { get; set; }
        /// <summary>
        /// 账号名称
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 代理商
        /// </summary>
        public string Agent { get; set; }
        /// <summary>
        ///开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        ///结束时间
        /// </summary>
        public string EndTime { get; set; }
    }

    /// <summary>
    /// 搜索类
    /// </summary>
    public class SearchPage: MongoSearchDto
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
