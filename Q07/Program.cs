for (int i = 0; i < 1000000; i++)
{
    Cache.Add(i, new object());
}

Console.WriteLine("Cache populated");

Console.ReadLine();

class Cache
{
    private static Dictionary<int, object> _cache = new Dictionary<int, object>();
    private static int _maxCacheSize = 1000; // Set desired maximum cache size


    public static void Add(int key, object value)
    {
        if (!_cache.ContainsKey(key))
        {
            if (_cache.Count >= _maxCacheSize)
            {
                // Perform cache eviction (e.g., remove the oldest items)
                // This is a simple example; might want to implement a more sophisticated strategy.
                foreach (var oldestKey in GetOldestKeys())
                {
                    Console.WriteLine(oldestKey.ToString());
                    _cache.Remove(oldestKey);
                }
            }

            _cache.Add(key, value);
        }
        // Optionally, we can also handle the case where the key already exists (update or skip)
    }

    public static object Get(int key)
    {
        return _cache[key];
    }

    private static IEnumerable<int> GetOldestKeys()
    {
        // Implement cache eviction strategy here
        // This example returns the keys of the oldest items in the cache.
        return _cache.Keys.Take(_cache.Count - _maxCacheSize);
    }
}