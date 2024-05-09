namespace Utilities;

public class PriorityComparer : IComparer<string>
{
    private static readonly Dictionary<string, int> PriorityOrder = new Dictionary<string, int>
    {
        { "Low", 3 },
        { "Medium", 2 },
        { "High", 1 }
    };

    public int Compare(string x, string y)
    {
        int xOrder = PriorityOrder.GetValueOrDefault(x, 0);
        int yOrder = PriorityOrder.GetValueOrDefault(y, 0);

        return xOrder.CompareTo(yOrder);
    }
}