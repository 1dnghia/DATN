public interface IPickupable
{
    void OnPickup();
    bool CanPickup { get; }
}
