﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Localization.Sources.Resource
{
    /// <summary>
    /// This class is used to simplify to create a localization source that
    /// uses resource a file.
    /// </summary>
    public class ResourceFileLocalizationSource : ILocalizationSource, ISingletonDependency
    {
        /// <summary>
        /// Unique Name of the source.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Reference to the <see cref="ResourceManager"/> object related to this localization source.
        /// </summary>
        public ResourceManager ResourceManager { get; private set; }

        private ILocalizationConfiguration _configuration;

        /// <param name="name">Unique Name of the source</param>
        /// <param name="resourceManager">Reference to the <see cref="ResourceManager"/> object related to this localization source</param>
        public ResourceFileLocalizationSource(string name, ResourceManager resourceManager)
        {
            Name = name;
            ResourceManager = resourceManager;
        }

        /// <summary>
        /// This method is called by ABP before first usage.
        /// </summary>
        public virtual void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver)
        {
            _configuration = configuration;
        }

        public virtual string GetString(string name)
        {
            var value = GetStringOrNull(name);
            if (value == null)
            {
                return ReturnGivenNameOrThrowException(name, Thread.CurrentThread.CurrentUICulture);
            }

            return value;
        }

        public virtual string GetString(string name, CultureInfo culture)
        {
            var value = GetStringOrNull(name, culture);
            if (value == null)
            {
                return ReturnGivenNameOrThrowException(name, culture);
            }

            return value;
        }

        public string GetStringOrNull(string name, bool tryDefaults = true)
        {
            //WARN: tryDefaults is not implemented!
            return ResourceManager.GetString(name);
        }

        public string GetStringOrNull(string name, CultureInfo culture, bool tryDefaults = true)
        {
            //WARN: tryDefaults is not implemented!
            return ResourceManager.GetString(name, culture);
        }

        /// <summary>
        /// Gets all strings in current language.
        /// </summary>
        public virtual IReadOnlyList<LocalizedString> GetAllStrings(bool includeDefaults = true)
        {
            return GetAllStrings(Thread.CurrentThread.CurrentUICulture, includeDefaults);
        }

        /// <summary>
        /// Gets all strings in specified culture.
        /// </summary>
        public virtual IReadOnlyList<LocalizedString> GetAllStrings(CultureInfo culture, bool includeDefaults = true)
        {
            return ResourceManager
                .GetResourceSet(culture, true, includeDefaults)
                .Cast<DictionaryEntry>()
                .Select(entry => new LocalizedString(entry.Key.ToString(), entry.Value.ToString(), culture))
                .ToImmutableList();
        }

        protected virtual string ReturnGivenNameOrThrowException(string name, CultureInfo culture)
        {
            return LocalizationSourceHelper.ReturnGivenNameOrThrowException(_configuration, Name, name, culture);
        }
    }
}
