namespace DependencyInjection
{
    [Service(typeof(IArgs), ServiceType.Singleton)]
    public class Args : IArgs {
        public string[]? Arguments { get; set; }
    }
}
