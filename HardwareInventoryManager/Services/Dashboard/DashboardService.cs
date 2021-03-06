﻿using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using System.Collections.Specialized;
using HardwareInventoryManager.Helpers.ApplicationSettings;

namespace HardwareInventoryManager.Helpers.Dashboard
{
    public class DashboardService
    {

        private string _userName;

        public DashboardService(string userName)
        {
            _userName = userName;
        }

        /// <summary>
        /// Gets data for pie chart: assets by category
        /// </summary>
        /// <returns></returns>
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
    
        /// <summary>
        /// Gets data for column graph: assets expiring in the next 4 months
        /// </summary>
        /// <returns></returns>
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
                .GroupBy(x => LinqExtention.MonthYear(x.ObsolescenseDate))
                .Select(group => new TwoColumnChartData
                {
                    ColumnDescription = group.Key.ToString("MMM-yy"),
                    CountOf = group.Count(),
                    OrderByDate = group.Key
                })
                .OrderBy(x => x.OrderByDate);
                
            return groupedChartData.ToList();
        }

        public IList<TwoColumnChartData> AssetsWarrantyExpiry4Months()
        {
            IRepository<Asset> repository = new Repository<Asset>(_userName);

            DateTime startDate = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.Month, 1);
            DateTime endDate = new DateTime
                (DateTime.Now.AddYears(-1).AddMonths(4).Year,
                DateTime.Now.AddMonths(4).Month, 1).AddMonths(1).AddDays(-1);


            var chartData = repository.GetAll()
                .Include(x => x.WarrantyPeriod)
                .Where(x => x.PurchaseDate.HasValue
                    && x.WarrantyPeriod != null 
                && x.PurchaseDate.Value >= startDate
                && x.PurchaseDate.Value <= endDate).ToList();

            var groupedChartData = chartData
                .GroupBy(x => LinqExtention.MonthYear(x.WarrantyExpiryDate))
                .Select(group => new TwoColumnChartData
                {
                    ColumnDescription = group.Key.ToString("MMM-yy"),
                    CountOf = group.Count(),
                    OrderByDate = group.Key
                })
                .OrderBy(x => x.OrderByDate);

            return groupedChartData.ToList();
        }

        /// <summary>
        /// Summary of wishlist quotes by status
        /// </summary>
        /// <returns></returns>
        public int[] WishListSummary()
        {
            IRepository<QuoteRequest> repository = new Repository<QuoteRequest>(_userName);

            int countPending = repository.GetAll().Where(x
                => x.QuoteRequestStatus.Type.Description == EnumHelper.LookupTypes.QuoteRequestStatus.ToString()
                && x.QuoteRequestStatus.Description == EnumHelper.QuoteRequestTypes.Pending.ToString()).Count();

            int countProcessing = repository.GetAll().Where(x
                => x.QuoteRequestStatus.Type.Description == EnumHelper.LookupTypes.QuoteRequestStatus.ToString()
                && x.QuoteRequestStatus.Description == EnumHelper.QuoteRequestTypes.Processing.ToString()).Count();

            int countSupplied = repository.GetAll().Where(x
                => x.QuoteRequestStatus.Type.Description == EnumHelper.LookupTypes.QuoteRequestStatus.ToString()
                && x.QuoteRequestStatus.Description == EnumHelper.QuoteRequestTypes.Supplied.ToString()).Count();

            int countComplete = repository.GetAll().Where(x
                => x.QuoteRequestStatus.Type.Description == EnumHelper.LookupTypes.QuoteRequestStatus.ToString()
                && x.QuoteRequestStatus.Description == EnumHelper.QuoteRequestTypes.Complete.ToString()).Count();

            int[] wishlistStats = new int[4];
            wishlistStats[0] = countPending;
            wishlistStats[1] = countProcessing;
            wishlistStats[2] = countSupplied;
            wishlistStats[3] = countComplete;
            return wishlistStats;
        }

        public NameValueCollection DisplayPanels(string userId)
        {
            ApplicationSettingsService settings = new ApplicationSettingsService(userId);
            var dashboardSettingsForUser = settings.GetDashboardSettingsForUser();

            NameValueCollection collection = new NameValueCollection();
            foreach(var item in dashboardSettingsForUser)
            {
                collection.Add(item.AppSetting.Key, item.Value);
            }
            return collection;
        }

    }

    public class TwoColumnChartData
    {
        public string ColumnDescription { get; set; }
        public int CountOf {get;set;}
        public DateTime OrderByDate { get; set; }
    }

    public class LinqExtention
    {
        public static DateTime MonthYear(DateTime? date)
        {
            if (date.HasValue)
            {
                return new DateTime(date.Value.Year, date.Value.Month, 1);
            }
            return new DateTime();
        }
    }

}