using Behaviours;

namespace Mock
{
    public interface ICar
    {
        AutoBehaviour CarObject { get; set; }
        int CarNumber { get; set; }

        void SendToOther();
    }
}
