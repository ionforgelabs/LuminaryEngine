using LuminaryEngine.Engine.Core.Logging;
using LuminaryEngine.Engine.ECS;
using LuminaryEngine.Engine.Gameplay.Items;
using LuminaryEngine.Engine.Gameplay.Spirits;

namespace LuminaryEngine.Engine.Gameplay.Player;

public class InventoryComponent : IComponent
{
    private readonly Dictionary<Item, int> _items = new Dictionary<Item, int>();
    private readonly Dictionary<SpiritEssence, int> _spiritEssences = new Dictionary<SpiritEssence, int>();

    public int Capacity { get; set; } = 30; // Default inventory capacity
    public int UsedSlots => _items.Count;
    public int RemainingSlots => Capacity - UsedSlots;

    // Item Management
    public void AddItem(Item item, int quantity = 1)
    {
        if (quantity <= 0)
        {
            return;
        }
        
        LuminLog.Debug($"Adding item '{item.Name}' to inventory.");

        if (_items.ContainsKey(item))
        {
            _items[item] += quantity;
        }
        else if (UsedSlots < Capacity)
        {
            _items.Add(item, quantity);
        }
        else
        {
            LuminLog.Warning($"Inventory is full, cannot add item: {item.ItemId}");
        }
    }

    public bool RemoveItem(Item item, int quantity = 1)
    {
        if (quantity <= 0 || !_items.ContainsKey(item))
        {
            return false;
        }

        if (_items[item] > quantity)
        {
            _items[item] -= quantity;
            return true;
        }

        if (_items[item] == quantity)
        {
            _items.Remove(item);
            return true;
        }

        LuminLog.Warning($"Attempted to remove more of item '{item.ItemId}' than present in inventory.");
        return false;
    }
    
    public bool RemoveItem(string itemId, int quantity = 1)
    {
        var item = _items.FirstOrDefault(x => x.Key.ItemId == itemId).Key;
        return item != null && RemoveItem(item, quantity);
    }

    public bool HasItem(Item item, int quantity = 1)
    {
        return _items.ContainsKey(item) && _items[item] >= quantity;
    }
    
    public bool HasItem(string itemId, int quantity = 1)
    {
        return _items.Any(o => o.Key.ItemId == itemId && o.Value >= quantity);
    }

    public int GetItemCount(Item item)
    {
        return _items.TryGetValue(item, out int count) ? count : 0;
    }

    public Dictionary<Item, int> GetInventory()
    {
        return new Dictionary<Item, int>(_items); // Return a copy
    }

    public void SetInventory(Dictionary<Item, int> items)
    {
        _items.Clear();
        foreach (var item in items)
        {
            if (item.Value > 0)
            {
                _items[item.Key] = item.Value;
            }
        }
    }

    // Spirit Essence Management
    public void AddSpiritEssence(SpiritEssence essence, int quantity = 1)
    {
        if (quantity <= 0)
        {
            return;
        }

        if (_spiritEssences.ContainsKey(essence))
        {
            _spiritEssences[essence] += quantity;
        }
        else
        {
            _spiritEssences.Add(essence, quantity);
        }
    }

    public bool RemoveSpiritEssence(SpiritEssence essence, int quantity = 1)
    {
        if (quantity <= 0 || !_spiritEssences.ContainsKey(essence))
        {
            return false;
        }

        if (_spiritEssences[essence] > quantity)
        {
            _spiritEssences[essence] -= quantity;
            return true;
        }

        if (_spiritEssences[essence] == quantity)
        {
            _spiritEssences.Remove(essence);
            return true;
        }

        LuminLog.Warning($"Attempted to remove more of spirit essence '{essence.EssenceID}' than present in inventory.");
        return false;
    }
    
    public bool RemoveSpiritEssence(string essenceId, int quantity = 1)
    {
        var essence = _spiritEssences.FirstOrDefault(x => x.Key.EssenceID == essenceId).Key;
        return essence != null && RemoveSpiritEssence(essence, quantity);
    }

    public bool HasSpiritEssence(SpiritEssence essence, int quantity = 1)
    {
        return _spiritEssences.ContainsKey(essence) && _spiritEssences[essence] >= quantity;
    }
    
    public bool HasSpiritEssence(string essenceId, int quantity = 1)
    {
        return _spiritEssences.Any(o => o.Key.EssenceID == essenceId && o.Value >= quantity);
    }

    public int GetSpiritEssenceCount(SpiritEssence essence)
    {
        return _spiritEssences.TryGetValue(essence, out int count) ? count : 0;
    }

    public Dictionary<SpiritEssence, int> GetSpiritEssences()
    {
        return new Dictionary<SpiritEssence, int>(_spiritEssences); // Return a copy
    }

    public void SetSpiritEssences(Dictionary<SpiritEssence, int> spiritEssences)
    {
        _spiritEssences.Clear();
        foreach (var essence in spiritEssences)
        {
            if (essence.Value > 0)
            {
                _spiritEssences[essence.Key] = essence.Value;
            }
        }
    }

    // Other Inventory Operations
    public bool CanAddItem(Item item, int quantity = 1)
    {
        if (quantity <= 0)
        {
            return true;
        }

        if (_items.ContainsKey(item))
        {
            return true; // Can always add to an existing stack (assuming no max stack size limit here)
        }

        return UsedSlots + 1 <= Capacity;
    }

    public bool IsFull()
    {
        return UsedSlots >= Capacity;
    }

    public void Clear()
    {
        _items.Clear();
        _spiritEssences.Clear();
    }

    public void Expand(int amount)
    {
        if (amount > 0)
        {
            Capacity += amount;
            LuminLog.Debug($"Inventory expanded by {amount}. New capacity: {Capacity}");
        }
        else
        {
            LuminLog.Warning($"Attempted to expand inventory by a non-positive amount: {amount}");
        }
    }

    public void Shrink(int amount)
    {
        if (amount > 0 && Capacity - amount >= UsedSlots)
        {
            Capacity -= amount;
            LuminLog.Debug($"Inventory shrunk by {amount}. New capacity: {Capacity}");
        }
    }
}