namespace Phoenix.Core.Servers
{
    public class CoreServer : FSServer
    {
        public CoreServer() : base(FSConfig.ConnectionString("Phoenix_Core")) { }
    }
}
