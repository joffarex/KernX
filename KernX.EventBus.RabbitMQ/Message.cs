using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KernX.EventBus.RabbitMQ
{
    internal readonly struct Message
    {
        public EventHeaders Headers { get; }

        public string Content { get; }

        public Message
            (ReadOnlySpan<byte> content, EventHeaders headers) =>
            (Content, Headers) = (Encoding.UTF8.GetString(content), headers);

        public async Task<T> ParseMessage<T>()
        {
            if (!Headers.ContentType.Equals("application/json"))
            {
                throw new ArgumentOutOfRangeException(nameof(Headers.ContentType), "Other content types not supported");
            }

            await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(Content));
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }

        public override string ToString() => $"{JsonSerializer.Serialize(this)}";
    }
}