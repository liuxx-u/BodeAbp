using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Threading;

namespace Abp.Configuration
{
    /// <summary>
    /// Extension methods for <see cref="ISettingManager"/>.
    /// </summary>
    public static class SettingManagerExtensions
    {
        /// <summary>
        /// Gets value of a setting in given type (<see cref="T"/>).
        /// </summary>
        /// <typeparam name="T">Type of the setting to get</typeparam>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Value of the setting</returns>
        public static async Task<T> GetSettingValueAsync<T>(this ISettingManager settingManager, string name)
            where T : struct
        {
            return (await settingManager.GetSettingValueAsync(name)).To<T>();
        }

        /// <summary>
        /// Gets current value of a setting for the application level.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Current value of the setting for the application</returns>
        public static async Task<T> GetSettingValueForApplicationAsync<T>(this ISettingManager settingManager, string name)
            where T : struct
        {
            return (await settingManager.GetSettingValueForApplicationAsync(name)).To<T>();
        }

        /// <summary>
        /// Gets current value of a setting for a user level.
        /// It gets the setting value, overwritten by given user.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="userId">User id</param>
        /// <returns>Current value of the setting for the user</returns>
        public static async Task<T> GetSettingValueForUserAsync<T>(this ISettingManager settingManager, string name, long userId)
           where T : struct
        {
            return (await settingManager.GetSettingValueForUserAsync(name, userId)).To<T>();
        }

        /// <summary>
        /// Gets current value of a setting.
        /// It gets the setting value, overwritten by application and the current user if exists.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Current value of the setting</returns>
        public static string GetSettingValue(this ISettingManager settingManager, string name)
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueAsync(name));
        }

        /// <summary>
        /// Gets current value of a setting for the application level.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Current value of the setting for the application</returns>
        public static string GetSettingValueForApplication(this ISettingManager settingManager, string name)
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForApplicationAsync(name));
        }

        /// <summary>
        /// Gets current value of a setting for a user level.
        /// It gets the setting value, overwritten by given user.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="userId">User id</param>
        /// <returns>Current value of the setting for the user</returns>
        public static string GetSettingValueForUser(this ISettingManager settingManager, string name, long userId)
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForUserAsync(name, userId));
        }

        /// <summary>
        /// Gets value of a setting.
        /// </summary>
        /// <typeparam name="T">Type of the setting to get</typeparam>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Value of the setting</returns>
        public static T GetSettingValue<T>(this ISettingManager settingManager, string name)
            where T : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueAsync<T>(name));
        }
        
        /// <summary>
        /// Gets current value of a setting for the application level.
        /// </summary>
        /// <typeparam name="T">Type of the setting to get</typeparam>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Current value of the setting for the application</returns>
        public static T GetSettingValueForApplication<T>(this ISettingManager settingManager, string name)
            where T : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForApplicationAsync<T>(name));
        }

        /// <summary>
        /// Gets current value of a setting for a user level.
        /// It gets the setting value, overwritten by given user.
        /// </summary>
        /// <typeparam name="T">Type of the setting to get</typeparam>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="userId">User id</param>
        /// <returns>Current value of the setting for the user</returns>
        public static T GetSettingValueForUser<T>(this ISettingManager settingManager, string name, long userId)
            where T : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForUserAsync<T>(name, userId));
        }
        
        /// <summary>
        /// Gets current values of all settings.
        /// It gets all setting values, overwritten by application and the current user if exists.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <returns>List of setting values</returns>
        public static IReadOnlyList<ISettingValue> GetAllSettingValues(this ISettingManager settingManager)
        {
            return AsyncHelper.RunSync(settingManager.GetAllSettingValuesAsync);
        }

        /// <summary>
        /// Gets a list of all setting values specified for the application.
        /// It returns only settings those are explicitly set for the application.
        /// If a setting's default value is used, it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValues"/> method.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <returns>List of setting values</returns>
        public static IReadOnlyList<ISettingValue> GetAllSettingValuesForApplication(this ISettingManager settingManager)
        {
            return AsyncHelper.RunSync(settingManager.GetAllSettingValuesForApplicationAsync);
        }

        /// <summary>
        /// Gets a list of all setting values specified for a user.
        /// It returns only settings those are explicitly set for the user.
        /// If a setting's value is not set for the user (for example if user uses the default value), it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValues"/> method.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="user">User to get settings</param>
        /// <returns>All settings of the user</returns>
        public static IReadOnlyList<ISettingValue> GetAllSettingValuesForUser(this ISettingManager settingManager, UserIdentifier user)
        {
            return AsyncHelper.RunSync(() => settingManager.GetAllSettingValuesForUserAsync(user));
        }

        /// <summary>
        /// Changes setting for the application level.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        public static void ChangeSettingForApplication(this ISettingManager settingManager, string name, string value)
        {
            AsyncHelper.RunSync(() => settingManager.ChangeSettingForApplicationAsync(name, value));
        }

        /// <summary>
        /// Changes setting for a user.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="user">User</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        public static void ChangeSettingForUser(this ISettingManager settingManager, UserIdentifier user, string name, string value)
        {
            AsyncHelper.RunSync(() => settingManager.ChangeSettingForUserAsync(user, name, value));
        }
    }
}