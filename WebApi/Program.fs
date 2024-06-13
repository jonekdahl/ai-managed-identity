open System
//open Azure.Core.Diagnostics
open Azure.Identity
open Microsoft.ApplicationInsights.AspNetCore.Extensions
open Microsoft.ApplicationInsights.Extensibility
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection

[<EntryPoint>]
let main args =
    //use (listener: AzureEventSourceListener) = AzureEventSourceListener.CreateConsoleLogger()

    let builder = WebApplication.CreateBuilder(args)

    builder.Services.Configure<TelemetryConfiguration>(fun (config: TelemetryConfiguration) ->
       let credential = DefaultAzureCredential(includeInteractiveCredentials = true)
       config.SetAzureTokenCredential(credential)
    )
    |> ignore

    // The following line enables Application Insights telemetry collection.
    // Reads connection string from appsettings.json
    //builder.Services.AddApplicationInsightsTelemetry() |> ignore

    // The following line enables Application Insights telemetry collection

    builder.Services.AddApplicationInsightsTelemetry(
      ApplicationInsightsServiceOptions(
        ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint=https://westeurope-2.in.applicationinsights.azure.com/"
      ))
    |> ignore

    let app = builder.Build()

    app.MapGet("/", Func<string>(fun () -> "Hello World!")) |> ignore
    app.MapGet("/fnork", Func<string>(fun () -> "FNORK!")) |> ignore

    app.Run()

    0 // Exit code

