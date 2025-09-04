public interface IUpgradeable
{
    void Upgrade();
    int CurrentLevel { get; }
    int MaxLevel { get; }
    bool CanUpgrade { get; }
}
