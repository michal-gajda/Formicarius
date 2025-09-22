namespace Formicarius.WebApi;

using System.Diagnostics;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class Program
{
    private const string SERVICE_NAME = "webapi";
    private const string SERVICE_NAMESPACE = "formicarius";
    private const string SERVICE_VERSION = "poc";

    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks();

        var attributes = new Dictionary<string, object>
        {
            ["environment.name"] = "docker",
            ["team.name"] = "dev",
        };

        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(serviceName: SERVICE_NAME, serviceNamespace: SERVICE_NAMESPACE, serviceVersion: SERVICE_VERSION)
            .AddAttributes(attributes);
        builder.Services.AddLogging(configure => configure.AddOpenTelemetry(options => options.SetResourceBuilder(resourceBuilder).AddOtlpExporter()));

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .SetResourceBuilder(resourceBuilder)
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter())
                ;

        var tags = attributes.Select(attribute => new KeyValuePair<string, object?>(attribute.Key, attribute.Value));

        builder.Services.AddSingleton(new ActivitySource(SERVICE_NAME, SERVICE_VERSION, tags));

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseHealthChecks("/health");

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}
