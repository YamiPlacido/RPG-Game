using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Managers
{
    interface IGameManager
    {
        ManagerStatus status { get; }
        void Startup(NetworkService service);
    }

    public enum ManagerStatus
    {
        Shutdown,
        Initilizing,
        Started
    }
}
