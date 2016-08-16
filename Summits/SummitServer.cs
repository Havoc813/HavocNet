using Phoenix.Core;

namespace Summits
{
    public class SummitServer : FSServer
    {
        public SummitServer() : base(FSConfig.ConnectionString("Summit")) { }
    }
}
