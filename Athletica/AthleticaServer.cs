using Phoenix.Core;

namespace Athletica
{
    public class AthleticaServer : FSServer
    {
        public AthleticaServer()
            : base(FSConfig.ConnectionString("Athletica"))
        {}
    }
}
