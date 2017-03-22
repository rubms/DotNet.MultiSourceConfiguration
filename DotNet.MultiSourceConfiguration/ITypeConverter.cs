namespace DotNet.MultiSourceConfiguration
{
    public interface ITypeConverter<T>
    {
        T FromString(string value);
        T GetDefaultValue();
    }
}