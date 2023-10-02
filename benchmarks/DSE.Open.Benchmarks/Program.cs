using BenchmarkDotNet.Running;

namespace DSE.Open.Benchmarks;

public sealed class Program
{
    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
