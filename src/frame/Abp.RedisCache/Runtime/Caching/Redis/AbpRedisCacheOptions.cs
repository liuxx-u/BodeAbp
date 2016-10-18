﻿using System.Configuration;
using Abp.Extensions;

namespace Abp.Runtime.Caching.Redis
{
    public class AbpRedisCacheOptions
    {
        private const string ConnectionStringKey = "Abp.Redis.Cache";
        private const string DatabaseIdSettingKey = "Abp.Redis.Cache.DatabaseId";

        public string ConnectionString { get; set; }
        public int DatabaseId { get; set; }

        public AbpRedisCacheOptions()
        {
            ConnectionString = GetDefaultConnectionString();
            DatabaseId = GetDefaultDatabaseId();
        }

        private static int GetDefaultDatabaseId()
        {
            var appSetting = ConfigurationManager.AppSettings[DatabaseIdSettingKey];
            if (appSetting.IsNullOrEmpty())
            {
                return -1;
            }

            int databaseId;
            if (!int.TryParse(appSetting, out databaseId))
            {
                return -1;
            }

            return databaseId;
        }

        private static string GetDefaultConnectionString()
        {
            var connStr = ConfigurationManager.ConnectionStrings[ConnectionStringKey];
            if (connStr == null || connStr.ConnectionString.IsNullOrWhiteSpace())
            {
                return "localhost";
            }

            return connStr.ConnectionString;
        }
    }
}