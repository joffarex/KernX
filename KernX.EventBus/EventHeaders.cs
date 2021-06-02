using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace KernX.EventBus
{
    public readonly struct EventHeaders
    {
        public string AppId { get; init; }
        public string MessageId { get; init; }
        public string ContentType { get; init; }
        public long Timestamp { get; init; }

        public override string ToString() => JsonSerializer.Serialize(this);

        private static class Headers
        {
            public const string AppId = "X-App-Id";
            public const string ContentType = "X-Content-Type";
            public const string MessageId = "X-Message-Id";
            public const string Timestamp = "X-Timestamp";
        }

        public static Dictionary<string, object> ToDictionary(string appId) => new()
        {
            {Headers.AppId, appId},
            {Headers.ContentType, "application/json"},
            {Headers.MessageId, Guid.NewGuid().ToString()},
            {Headers.Timestamp, DateTimeOffset.Now.ToUnixTimeSeconds()}
        };

        public static EventHeaders Create(IDictionary<string, object> headers) => new()
        {
            AppId = Encoding.UTF8.GetString((byte[]) headers[Headers.AppId]),
            ContentType = Encoding.UTF8.GetString((byte[]) headers[Headers.ContentType]),
            MessageId = Encoding.UTF8.GetString((byte[]) headers[Headers.MessageId]),
            Timestamp = (long) headers[Headers.Timestamp]
        };

        public static EventHeaders Create(IDictionary<string, string> headers) => new()
        {
            AppId = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(headers[Headers.AppId])),
            ContentType = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(headers[Headers.ContentType])),
            MessageId = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(headers[Headers.MessageId])),
            Timestamp = long.Parse(headers[Headers.Timestamp])
        };


        public static List<string> GetNames => new()
            {nameof(AppId), nameof(ContentType), nameof(MessageId), nameof(Timestamp)};
    }
}