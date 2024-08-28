using System.Collections.Concurrent;

namespace HashBruteForce;

internal sealed class HashAttemptPool
{
    private readonly ConcurrentBag<HashAttempt> _pool;
    private readonly int _panLength;

    public HashAttemptPool(
        int panLength,
        int initialCapacity)
    {
        _pool = new ConcurrentBag<HashAttempt>();
        _panLength = panLength;

        for (var i = 0; i < initialCapacity; i++)
        {
            _pool.Add(new HashAttempt(panLength));
        }
    }

    public HashAttempt Rent()
    {
        return _pool.TryTake(out var pool)
            ? pool 
            : new HashAttempt(_panLength);
    }

    public void Return(HashAttempt hashAttempt)
    {
        Array.Clear(hashAttempt.Pan, 0, hashAttempt.Pan.Length);
        Array.Clear(hashAttempt.Utf8Bytes, 0, hashAttempt.Utf8Bytes.Length);
        Array.Clear(hashAttempt.HashedPan, 0, hashAttempt.HashedPan.Length);
        
        _pool.Add(hashAttempt);
    }
}