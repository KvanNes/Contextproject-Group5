using Behaviours;

namespace Interfaces
{
    public interface ICar
    {
        CarBehaviour CarObject { get; set; }
        int CarNumber { get; set; }

        void SendToOther();
    }
}
