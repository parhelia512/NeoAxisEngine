﻿#if !NO_LITE_DB
using System;
using System.Collections.Generic;
using static Internal.LiteDB.Constants;

namespace Internal.LiteDB.Engine
{
    public partial class LiteEngine
    {
        /// <summary>
        /// Get registered system collection
        /// </summary>
        internal SystemCollection GetSystemCollection(string name)
        {
            if (_systemCollections.TryGetValue(name, out var sys))
            {
                return sys;
            }

            throw new LiteException(0, $"System collection '{name}' are not registered as system collection");
        }

        /// <summary>
        /// Register a new system collection that can be used in query for input/output data
        /// Collection name must starts with $
        /// </summary>
        internal void RegisterSystemCollection(SystemCollection systemCollection)
        {
            if (systemCollection == null) throw new ArgumentNullException(nameof(systemCollection));

            _systemCollections[systemCollection.Name] = systemCollection;
        }

        /// <summary>
        /// Register a new system collection that can be used in query for input data
        /// Collection name must starts with $
        /// </summary>
        internal void RegisterSystemCollection(string collectionName, Func<IEnumerable<BsonDocument>> factory)
        {
            if (collectionName.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collectionName));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _systemCollections[collectionName] = new SystemCollection(collectionName, factory);
        }
    }
}
#endif