using Phoenix.Core;

namespace HavocNet
{
    public class HavocServer : FSServer
    {
        public HavocServer() : base(FSConfig.ConnectionString("HavocNet")) { }
    }
}
