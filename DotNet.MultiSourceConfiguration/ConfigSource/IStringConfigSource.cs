namespace MultiSourceConfiguration.Config.ConfigSource
{
    public interface IStringConfigSource
    {
        bool TryGetString(string property, out string value);
    }
}