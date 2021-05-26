﻿using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace KernX.Logger
{
    public static class KXLogger
    {
        private const string OutputTemplate =
            "[{Timestamp:HH:mm}] [{SourceContext}] [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}";

        private static readonly Serilog.Core.Logger SerilogLogger = new LoggerConfiguration().MinimumLevel.Verbose()
            .MinimumLevel
            .Override("Microsoft", LogEventLevel.Information)
            .Enrich.With(new ThreadEnricher())
            .WriteTo.Console(outputTemplate: OutputTemplate)
            .CreateLogger();

        public static ILogger<T> CreateLogger<T>() =>
            new LoggerFactory().AddSerilog(SerilogLogger).CreateLogger<T>();
    }
}