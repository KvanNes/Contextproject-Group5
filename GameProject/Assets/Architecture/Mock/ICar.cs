public interface ICar
{
    AutoBehaviour CarObject { get; set; }
    int carNumber { get; set; }

    void SendToOther();
}
