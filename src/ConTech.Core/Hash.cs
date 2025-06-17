using Sqids;

namespace ConTech.Core;

public static class RandomCode
{
    static readonly Random _rnd = new();

    public static string Generate()
    {
        var number = _rnd.Next(0, Int32.MaxValue);

        return Hash.EncodeInt(number);
    }
}

public static class Hash
{
    static SqidsEncoder<long> _longHash = default!;

    static SqidsEncoder<int> _intHash = default!;

    static SqidsEncoder<short> _shortHash = default!;

    /// <summary>
    /// Must be initialized at Program.cs
    /// </summary>
    /// <param name="longAlphabet"></param>
    /// <param name="intAlphabet"></param>
    /// <param name="shortAlphabet"></param>
    public static void Init(string longAlphabet, string intAlphabet, string shortAlphabet)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(longAlphabet);
        ArgumentException.ThrowIfNullOrWhiteSpace(intAlphabet);
        ArgumentException.ThrowIfNullOrWhiteSpace(shortAlphabet);

        _longHash = new SqidsEncoder<long>(new()
        {
            MinLength = 10,
            Alphabet = longAlphabet,
        });

        _intHash = new SqidsEncoder<int>(new()
        {
            MinLength = 10,
            // Alphabet = intAlphabet,
            Alphabet = longAlphabet,
        });

        _shortHash = new SqidsEncoder<short>(new()
        {
            MinLength = 10,
            Alphabet = shortAlphabet
        });
    }


    public static int DecodeToInt(string hash)
        => _intHash.Decode(hash)[0];

    public static long DecodeToLong(string hash)
        => _longHash.Decode(hash)[0];

    public static short DecodeToShort(string hash)
        => _shortHash.Decode(hash)[0];

    public static string EncodeInt(int number)
        => _intHash.Encode(number);

    public static string EncodeLong(long number)
        => _longHash.Encode(number);

    public static string EncodeShort(short number)
        => _shortHash.Encode(number);
}
