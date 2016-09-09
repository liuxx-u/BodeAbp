using System.Collections.Concurrent;

namespace Abp.Extensions
{
    public static class DictionaryExtensions
    {

        #region ConcurrentDictionary
        #endregion
        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            TValue value;
            return dict.TryRemove(key, out value);
        }
    }
}
