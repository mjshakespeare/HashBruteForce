namespace HashBruteForce;

internal sealed class HashAttempt
{
    public readonly char[] Pan;
    public readonly byte[] Utf8Bytes;
    public readonly byte[] HashedPan;

    public HashAttempt(
        int panLength)
    {
        Pan = new char[panLength];
        Utf8Bytes = new byte[panLength];
        HashedPan = new byte[64];
    }
}