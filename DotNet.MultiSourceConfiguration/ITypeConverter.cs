namespace MultiSourceConfiguration.Config
{
    public interface ITypeConverter<T>
    {
        T FromString(string value);
        T GetDefaultValue();
    }
}