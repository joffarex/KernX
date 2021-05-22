namespace KernX.EventBus.RabbitMQ
{
    public sealed class RabbitMQSettings
    {
        public int Port { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string VHost { get; set; }
    }
}