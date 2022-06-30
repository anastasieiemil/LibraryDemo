using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core
{
    public static class ResourceManagerLocker
    {
        private const int LOCKER_TIMEOUT = 5000;

        /// <summary>
        /// Locks a specific resource for preventing another thread of accessing.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool LockResource(object collection, string key)
        {
            if (collection == null || string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            try
            {

                if (Monitor.TryEnter(locker, LOCKER_TIMEOUT))
                {
                    // Get collection
                    if (!lockedResources.TryGetValue(collection.GetHashCode(), out HashSet<string> protectedCollection))
                    {
                        lockedResources.Add(collection.GetHashCode(), protectedCollection = new HashSet<string>());
                    }

                    // Try to lock resource.
                    if (!protectedCollection.Contains(key))
                    {
                        protectedCollection.Add(key);
                        return true;
                    }
                }
            }
            catch { }
            finally
            {
                Monitor.Exit(locker);
            }

            return false;
        }

        /// <summary>
        /// Release a locked resource.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        public static void ReleaseResource(object collection, string key)
        {
            if (Monitor.TryEnter(locker, LOCKER_TIMEOUT + 2000))
            {
                if(lockedResources.TryGetValue(collection.GetHashCode(), out HashSet<string> protectedCollection) && protectedCollection.TryGetValue(key,out string outKey))
                {
                    protectedCollection.Remove(key);
                }
                Monitor.Exit(locker);
            }
        }

        #region private

        private static volatile object locker = new object();
        private static Dictionary<int, HashSet<string>> lockedResources = new Dictionary<int, HashSet<string>>();
        #endregion
    }
}
