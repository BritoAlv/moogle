using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Diagnosers;

public class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<benchmark>();
 /*     for(int i = 1; i < 100; i++)
        {
            string a = r_string.random_string(i);
            string b = r_string.random_string(i);

            Console.WriteLine(a + " " + b + " " + levensthein_implementations.levensthein_0(a,b) + " " +  levensthein_implementations.levensthein_1(a,b));
        } */
              
    }
}





public class config : ManualConfig
{
    public CsvExporter exporter = new CsvExporter(
    CsvSeparator.CurrentCulture,
    new SummaryStyle(
        cultureInfo: System.Globalization.CultureInfo.CurrentCulture,
        printUnitsInHeader: true,
        printUnitsInContent: false,
        timeUnit: Perfolizer.Horology.TimeUnit.Microsecond,
        sizeUnit: SizeUnit.B
    ));
    public config()
    {
        AddExporter(exporter);
    }
}

[Config(typeof(config))]
[MemoryDiagnoser]
/*[HardwareCounters(
        HardwareCounter.BranchMispredictions,
        HardwareCounter.BranchInstructions)] */        
public class benchmark
{
    [Params(10,100,1000,10000)]
    public int size {get; set ;}
    public string A;
    public string B;
    [GlobalSetup]
    public void Setup()
    {
        A = r_string.random_string(size);
        B = r_string.random_string(size); 
    }
    
    [Benchmark(Baseline = true)]
    public int levensthein_00()
    {
        return levensthein_implementations.levensthein_0(A,B);
    }
    [Benchmark]
    public int levensthein_01()
    {
        return levensthein_implementations.levensthein_1(A,B);
    }
    [Benchmark]
    public int levensthein_02()
    {
        return levensthein_implementations.levensthein_2(A,B);
    }
    [Benchmark]
    public int levensthein_03()
    {
        return levensthein_implementations.levensthein_3(A,B);
    }    
    [Benchmark]
    public int levensthein_04()
    {
        return levensthein_implementations.levensthein_4(A,B);
    }
    [Benchmark]
    public int levensthein_05()
    {
        return levensthein_implementations.levensthein_4(A,B);
    }   
    [Benchmark]
    public int levensthein_06()
    {
        return levensthein_implementations.levensthein_4(A,B);
    }                       
}