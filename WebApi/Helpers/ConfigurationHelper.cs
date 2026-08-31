using Newtonsoft.Json;
using WebApi.Abstractions;

namespace WebApi.Helpers;

public static class ConfigurationHelper
{
    private static Dictionary<string, object> _configurations = new();
    
    public static T GetEntry<T>(this IConfiguration configuration) where T : IConfigurationEntry
    {
        string key = T.Key;

        if (_configurations.TryGetValue(key, out object? value))
        {
            return (T)value;
        }
        
        var dict = configuration.GetSection(T.Key).Get<Dictionary<string, object>>();
        string json = JsonConvert.SerializeObject(dict);
        
        T entry = JsonConvert.DeserializeObject<T>(json)!;
        _configurations[key] = entry;

        return entry;
    }
}