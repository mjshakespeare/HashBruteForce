using System.Security.Cryptography;
using System.Text;

namespace HashBruteForce;

public sealed class PanHashCracker
    : IDisposable
{
    private readonly ThreadLocal<SHA512> _threadLocalHasher;
    
    public PanHashCracker()
    {
        _threadLocalHasher
            = new ThreadLocal<SHA512>(
                SHA512.Create,
                true);
    }
    
    public string CrackHash(
        string bin,
        int panLength,
        byte[] hash)
    {
        var hashDigits = panLength - bin.Length;
        
        var maxNumber = (long)Math.Pow(10, hashDigits);

        var hashAttemptPool 
            = new HashAttemptPool(
                panLength,
                100);

        var binChars
            = bin.ToCharArray();
        
        var matchFound = false;
        var rawPan = string.Empty;

        Parallel.For(0, maxNumber, (number, state) =>
        {
            if (Volatile.Read(ref matchFound))
            {
                state.Break();
                return;
            }

            var attempt = hashAttemptPool.Rent();

            if (AttemptHash(attempt, binChars, number, hash))
            {
                rawPan = new string(attempt.Pan);
                Volatile.Write(ref matchFound, true);
                hashAttemptPool.Return(attempt);
                state.Break();
            }
            else
            {
                hashAttemptPool.Return(attempt);
            }
        });

        return rawPan;
    }
    
    private bool AttemptHash(
        HashAttempt attempt,
        char[] bin,
        long number,
        byte[] hash)
    {
        var panSpan
            = attempt
                .Pan
                .AsSpan();
       
        var numberSlice
            = panSpan[bin.Length..];
        
        bin.CopyTo(panSpan);
        number.TryFormat(numberSlice, out _, "D8");

        Encoding.UTF8.GetBytes(
            panSpan,
            attempt.Utf8Bytes);

        var hasher 
            = _threadLocalHasher
                .Value!;
        
        hasher.
            TryComputeHash(
                attempt.Utf8Bytes,
                attempt.HashedPan,
                out _);
        
        return attempt.HashedPan.AsSpan().SequenceEqual(hash);
    }
    
    public void Dispose()
    {
        if (_threadLocalHasher.IsValueCreated)
        {
            foreach (var hasher in _threadLocalHasher.Values)
            {
                hasher.Dispose();
            }
        }
        _threadLocalHasher.Dispose();
    }
}
