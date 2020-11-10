namespace Szark.DI
{
    [Service(typeof(IArgs), ServiceType.Singleton)]
    public class Args : IArgs
    {
        public string[]? Arguments { get; set; }
    }
}
