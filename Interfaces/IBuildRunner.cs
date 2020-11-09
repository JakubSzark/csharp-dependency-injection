namespace DependencyInjection
{
    public interface IBuildRunner
    {
        void Run(IServiceProvider services);
    }
}
