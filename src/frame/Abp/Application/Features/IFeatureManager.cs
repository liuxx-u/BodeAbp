﻿using System.Collections.Generic;

namespace Abp.Application.Features
{
    /// <summary>
    /// Feature manager.
    /// </summary>
    public interface IFeatureManager
    {
        /// <summary>
        /// Gets the <see cref="Feature"/> by specified name.
        /// </summary>
        /// <param name="name">Unique name of the feature.</param>
        Feature Get(string name);

        /// <summary>
        /// Gets the <see cref="Feature"/> by specified name or returns null if not found.
        /// </summary>
        /// <param name="name">The name.</param>
        Feature GetOrNull(string name);

        /// <summary>
        /// Gets all <see cref="Feature"/>s.
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<Feature> GetAll();
    }
}