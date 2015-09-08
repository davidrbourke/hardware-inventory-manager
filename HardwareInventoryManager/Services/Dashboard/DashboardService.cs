using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace HardwareInventoryManager.Services.Dashboard
{
    public class DashboardService
    {

        private string _userName;

        public DashboardService(string userName)
        {
            _userName = userName;
        }

        public IList<TwoColumnChartData> AssetsByCategoryPieChart()
        {
            IRepository<Asset> repository = new Repository<Asset>(_userName);
            var chartData = repository.GetAll()
                .GroupBy(x => x.Category.Description)
                .Select(group => new TwoColumnChartData
                {
                    ColumnDescription = group.Key,
                    CountOf = group.Count()
                })
                .OrderBy(x => x.ColumnDescription);
            return chartData.ToList();
        }
    
        public IList<TwoColumnChartData> AssetsByExpiry4Months()
        {
            IRepository<Asset> repository = new Repository<Asset>(_userName);


            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = new DateTime(DateTime.Now.AddMonths(4).Year,
                DateTime.Now.AddMonths(4).Month, 1).AddMonths(1).AddDays(-1);


            var chartData = repository.GetAll()
                .Where(x => x.ObsolescenseDate.HasValue
                &&  x.ObsolescenseDate.Value >= startDate
                && x.ObsolescenseDate.Value <= endDate).ToList();
                

            var groupedChartData = chartData
                .GroupBy(x => LinqExention.MonthYear(x.ObsolescenseDate))
                .Select(group => new TwoColumnChartData
                {
                    ColumnDescription = group.Key,
                    CountOf = group.Count()
                })
                .OrderBy(x => x.ColumnDescription);

            return groupedChartData.ToList();
        }
    }

    public class TwoColumnChartData
    {
        public string ColumnDescription { get; set; }
        public int CountOf {get;set;}
    }

    public class LinqExention
    {
        public static string MonthYear(DateTime? date)
        {
                if(date.HasValue)
                {
                    return date.Value.ToString("MMM-yy");
                }
                return string.Empty;
        }
    }
}