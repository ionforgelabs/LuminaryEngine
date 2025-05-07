namespace LuminaryEngine.Engine.Gameplay.Items;

public class Item
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TextureId { get; set; }
    public bool IsEquipped { get; set; }
    public string ItemId { get; set; }
    public ItemType Type { get; set; }
    
    // Assigned Data
    public ItemFlags Flags { get; set; }
    public Dictionary<string, float> Stats { get; set; }
    
    public Item Clone()
    {
        if (Stats == null)
        {
            return new Item
            {
                Name = Name,
                Description = Description,
                TextureId = TextureId,
                IsEquipped = IsEquipped,
                ItemId = ItemId,
                Type = Type,
                Flags = Flags,
                Stats = null
            };
        }

        return new Item
        {
            Name = Name,
            Description = Description,
            TextureId = TextureId,
            IsEquipped = IsEquipped,
            ItemId = ItemId,
            Type = Type,
            Flags = Flags,
            Stats = new Dictionary<string, float>(Stats)
        };
    }
}