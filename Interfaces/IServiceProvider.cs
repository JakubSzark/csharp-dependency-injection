namespace Szark.DI
{
    public interface IServiceProvider
    {
        T Get<T>() where T : class;
    }
}
