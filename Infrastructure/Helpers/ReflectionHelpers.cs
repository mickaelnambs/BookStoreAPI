namespace Infrastructure.Helpers;

public static class ReflectionHelpers
{
    private static readonly string[] DefaultExcludedProperties = ["Id", "CreatedAt"];

    public static void UpdateProperties<T>(T source, T destination,
        string[]? additionalExcludedProperties = null)
    {
        var excludedProperties = additionalExcludedProperties != null
            ? DefaultExcludedProperties.Concat(additionalExcludedProperties).ToArray()
            : DefaultExcludedProperties;

        var properties = typeof(T).GetProperties()
            .Where(p =>
                p.CanWrite &&
                !excludedProperties.Contains(p.Name) &&
                p.GetValue(source) != null);

        foreach (var prop in properties)
        {
            prop.SetValue(destination, prop.GetValue(source));
        }
    }
}
