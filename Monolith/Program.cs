// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Monolith;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;

public class Program
{
    public static readonly string ServiceName = "RockPaperScissor";
    public static TracerProvider TracerProvider;
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
    
    public static ILogger Logger1 => Serilog.Log.Logger;
    
    public static void Main()
    {
        
        
        
        
        TracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddConsoleExporter()
            .AddZipkinExporter()
            .AddSource(ActivitySource.Name)
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName))
            .Build();
        
        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341")
            .Enrich.WithSpan()
            .CreateLogger();

        
            using var activity = Program.ActivitySource.StartActivity();
        
        var game = new Game();
        
        for (int i = 0; i < 1000; i++)
        {
            using var activity2 = Program.ActivitySource.StartActivity("act2: Game nr " + i.ToString());
            Logger1.Verbose("Game Nr - forloop logger {0}!", i);
            game.Start();
        }
        
        Console.WriteLine("Finished");
        Log.CloseAndFlush();
        TracerProvider.ForceFlush();

    }
}