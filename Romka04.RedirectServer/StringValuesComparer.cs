using Microsoft.Extensions.Primitives;

namespace Romka04.RedirectServer;

public class StringValuesComparer
    : IEqualityComparer<StringValues>
{
    public static StringValuesComparer Ordinal { get; } = new(StringComparer.Ordinal);
    public static StringValuesComparer OrdinalIgnoreCase { get; }
        = new(StringComparer.OrdinalIgnoreCase);


    private StringComparer Comparer { get; }

    public StringValuesComparer(StringComparer comparer)
    {
        Comparer   = comparer;
    }

    public bool Equals(StringValues left, StringValues right)
    {
        int count = left.Count;

        if (count != right.Count)
        {
            return false;
        }

        for (int i = 0; i < count; i++)
        {
            if (!Comparer.Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(StringValues obj)
    {
        return obj.GetHashCode();
    }
}