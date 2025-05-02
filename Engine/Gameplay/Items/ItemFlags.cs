namespace LuminaryEngine.Engine.Gameplay.Items;

[Flags]
public enum ItemFlags
{
    None = 0,
    IsEquipped = 1 << 0,
    IsUsable = 1 << 1,
    IsConsumable = 1 << 2,
    IsStackable = 1 << 3,
    IsQuestItem = 1 << 4,
    IsCrafted = 1 << 5,
}