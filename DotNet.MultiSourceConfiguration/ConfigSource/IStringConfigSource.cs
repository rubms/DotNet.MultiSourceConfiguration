namespace DotNet.MultiSourceConfiguration.ConfigSource
{
    public interface IStringConfigSource
    {
        bool TryGetString(string property, out string value);
    }
}