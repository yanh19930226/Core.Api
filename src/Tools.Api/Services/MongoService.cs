using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Api.Dtos;
using Tools.Api.Models.Mongos;

namespace Tools.Api.Services
{
    /// <summary>
    /// Mongo配置
    /// </summary>
    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    
    public class MongoService
    {
        public List<AdvertiseFee> FetData(MongoSearchDto mongoSearchDto)
        {
            var config = new MongoSettings()
            {
                ConnectionString = "mongodb://127.0.0.1:27017/",
                DatabaseName = "Advertise"
            };
            var client = new MongoClient(config.ConnectionString);
            var database = client.GetDatabase(config.DatabaseName);
            var _collection = database.GetCollection<AdvertiseFee>("AdvertiseFee");

            FilterDefinition<AdvertiseFee> filter = Builders<AdvertiseFee>.Filter.Empty;
            SortDefinition<AdvertiseFee> sorter = Builders<AdvertiseFee>.Sort.Descending("_id");

            if (string.IsNullOrEmpty(mongoSearchDto.SearchStr) == false)
            {
                filter = filter & (Builders<AdvertiseFee>.Filter.Regex(p => p.ActName, mongoSearchDto.SearchStr) |
                    Builders<AdvertiseFee>.Filter.Regex(p => p.Agent, mongoSearchDto.SearchStr) |
                    Builders<AdvertiseFee>.Filter.Regex(p => p.AccountId, mongoSearchDto.SearchStr));
            }
            if (string.IsNullOrEmpty(mongoSearchDto.CompanyId) == false)
            {
                filter = filter & Builders<AdvertiseFee>.Filter.Eq(p=>p.CompanyId, mongoSearchDto.CompanyId);
            }
            if (string.IsNullOrEmpty(mongoSearchDto.ShopId) == false)
            {
                filter = filter & Builders<AdvertiseFee>.Filter.Eq(p => p.ShopId, mongoSearchDto.ShopId);
            }
            if (string.IsNullOrEmpty(mongoSearchDto.AccountId) == false)
            {
                filter = filter & Builders<AdvertiseFee>.Filter.Eq(p => p.AccountId, mongoSearchDto.AccountId);
            }
            if (string.IsNullOrEmpty(mongoSearchDto.AccountName) == false)
            {
                filter = filter & Builders<AdvertiseFee>.Filter.Eq(p => p.ActName, mongoSearchDto.AccountName);
            }
            if (string.IsNullOrEmpty(mongoSearchDto.Agent) == false)
            {
                filter = filter & Builders<AdvertiseFee>.Filter.Eq(p => p.Agent, mongoSearchDto.Agent);
            }
            if (string.IsNullOrEmpty(mongoSearchDto.StartTime) == false)
            {
                var StartTime = Convert.ToDateTime(mongoSearchDto.StartTime).ToUniversalTime().Date;
                filter = filter & Builders<AdvertiseFee>.Filter.Gte(p => p.Date, StartTime);
            }
            if (string.IsNullOrEmpty(mongoSearchDto.EndTime) == false)
            {
                var EndTime = Convert.ToDateTime(mongoSearchDto.EndTime).ToUniversalTime().Date;
                filter = filter & Builders<AdvertiseFee>.Filter.Lte (p => p.Date, EndTime);
            }
            var res = _collection.Find(filter).Sort(sorter).ToList();

            return res;

        }

        #region FindListByPage 分页查询集合
        /// <summary>
        /// 分页查询集合
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="count">总条数</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public  List<AdvertiseFee> FetDataPage(SearchPage searchPage, SortDefinition<AdvertiseFee> sort = null)
        {
            try
            {
                var config = new MongoSettings()
                {
                    ConnectionString = "mongodb://127.0.0.1:27017/",
                    DatabaseName = "Advertise"
                };
                var client = new MongoClient(config.ConnectionString);
                var database = client.GetDatabase(config.DatabaseName);
                var _collection = database.GetCollection<AdvertiseFee>("AdvertiseFee");

                FilterDefinition<AdvertiseFee> filter = Builders<AdvertiseFee>.Filter.Empty;

                if (string.IsNullOrEmpty(searchPage.SearchStr) == false)
                {
                    filter = filter & (Builders<AdvertiseFee>.Filter.Regex(p => p.ActName, searchPage.SearchStr) |
                        Builders<AdvertiseFee>.Filter.Regex(p => p.Agent, searchPage.SearchStr) |
                        Builders<AdvertiseFee>.Filter.Regex(p => p.AccountId, searchPage.SearchStr));
                }
                if (string.IsNullOrEmpty(searchPage.CompanyId) == false)
                {
                    filter = filter & Builders<AdvertiseFee>.Filter.Eq(p => p.CompanyId, searchPage.CompanyId);
                }
                if (string.IsNullOrEmpty(searchPage.ShopId) == false)
                {
                    filter = filter & Builders<AdvertiseFee>.Filter.Eq(p => p.ShopId, searchPage.ShopId);
                }
                if (string.IsNullOrEmpty(searchPage.AccountId) == false)
                {
                    filter = filter & Builders<AdvertiseFee>.Filter.Eq(p => p.AccountId, searchPage.AccountId);
                }
                if (string.IsNullOrEmpty(searchPage.AccountName) == false)
                {
                    filter = filter & Builders<AdvertiseFee>.Filter.Eq(p => p.ActName, searchPage.AccountName);
                }
                if (string.IsNullOrEmpty(searchPage.Agent) == false)
                {
                    filter = filter & Builders<AdvertiseFee>.Filter.Eq(p => p.Agent, searchPage.Agent);
                }
                if (string.IsNullOrEmpty(searchPage.StartTime) == false)
                {
                    var StartTime = Convert.ToDateTime(searchPage.StartTime).ToUniversalTime().Date;
                    filter = filter & Builders<AdvertiseFee>.Filter.Gte(p => p.Date, StartTime);
                }
                if (string.IsNullOrEmpty(searchPage.EndTime) == false)
                {
                    var EndTime = Convert.ToDateTime(searchPage.EndTime).ToUniversalTime().Date;
                    filter = filter & Builders<AdvertiseFee>.Filter.Lte(p => p.Date, EndTime);
                }
                //if (sort == null) return _collection.Find(filter).Skip((searchPage.PageIndex - 1) * searchPage.PageSize).Limit(searchPage.PageSize).ToList();
                //进行排序
                var res= _collection.Find(filter).Sort(sort).Skip((searchPage.PageIndex - 1) * searchPage.PageSize).Limit(searchPage.PageSize).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
