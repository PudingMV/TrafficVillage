namespace TrafficVillage.Utils;

public static class Guard
{
    public static T NotNull<T>(T? value, string paramName) where T : class
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        return value;
    }
}
