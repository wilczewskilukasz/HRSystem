using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HRinfoAPI.Models;

namespace HRinfoAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/News")]
    public class NewsController : ApiController
    {
        HRinfoEntities database = new HRinfoEntities();

        public IEnumerable<NewsResults> Get()
        {
            var result = from news in database.News
                         where news.IsActive == true
                            && (news.DateFrom == null
                                || news.DateFrom <= DateTime.UtcNow)
                            && (news.DateTo == null
                                || news.DateTo >= DateTime.UtcNow)
                         orderby news.DateFrom ascending
                         select new NewsResults() {
                             ResultNews = news.News1,
                             ResultDateFrom = news.DateFrom,
                             ResultDateTo = news.DateTo };

            return result.ToList();
        }

        /// <summary>
        /// Get top number of records order by id descending.
        /// </summary>
        /// <returns>NewsResults</returns>
        [Route("GetLastAdd")]
        public IEnumerable<NewsResults> GetLastAdd(int numberOfRecords)
        {
            var result = database.News.Where(news => news.IsActive == true
                    && (news.DateFrom == null || news.DateFrom <= DateTime.UtcNow)
                    && (news.DateTo == null || news.DateTo >= DateTime.UtcNow))
                .OrderByDescending(news => news.Id)
                .Select(news => new NewsResults()
                {
                    ResultNews = news.News1,
                    ResultDateFrom = news.DateFrom,
                    ResultDateTo = news.DateTo
                })
                .Take(numberOfRecords);

            return result.ToList();
        }

        /// <summary>
        /// Get limited number of records and skip declare number of records
        /// </summary>
        /// <param name="skipNumber">Number of records to skip</param>
        /// <param name="takeNumber">Number of records to take</param>
        /// <returns></returns>
        [Route("GetRecords")]
        public IEnumerable<NewsResults> GetRecords(int skipNumber, int takeNumber)
        {
            var result = database.News.Where(news => news.IsActive == true
                    && (news.DateFrom == null || news.DateFrom <= DateTime.UtcNow)
                    && (news.DateTo == null || news.DateTo >= DateTime.UtcNow))
                .OrderByDescending(news => news.Id)
                .Select(news => new NewsResults()
                {
                    ResultNews = news.News1,
                    ResultDateFrom = news.DateFrom,
                    ResultDateTo = news.DateTo
                })
                .Skip(skipNumber)
                .Take(takeNumber);

            return result.ToList();
        }
    }
}
