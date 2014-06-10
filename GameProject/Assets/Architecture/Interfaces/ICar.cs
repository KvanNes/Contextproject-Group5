using Behaviours;

namespace Interfaces
{
    public interface ICar
    {
        AutoBehaviour CarObject { get; set; }
        int CarNumber { get; set; }

        void SendToOther();
    }
}
