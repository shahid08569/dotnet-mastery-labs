

public static class Helper
{
    public static List<T> myWhere<T>(List<T> source, Func <T, bool> rule)
    {
        var newList = new List<T>();
        foreach (var item in source)
        {
            if (rule(item))
            {
                newList.Add(item);
            }
        }
        return newList;
    }
}
