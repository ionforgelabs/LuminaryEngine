namespace LuminaryEngine.Engine.Gameplay.Items;

public static class ItemFlagsExtensions
{
    public static bool HasFlag(this ItemFlags flags, ItemFlags flag) 
    {
        return (flags & flag) == flag;
    }
    
    public static ItemFlags AddFlag(this ItemFlags flags, ItemFlags flag) 
    {
        return flags | flag;
    }
    
    public static ItemFlags RemoveFlag(this ItemFlags flags, ItemFlags flag) 
    {
        return flags & ~flag;
    }

    public static List<ItemFlags> GetFlags(this ItemFlags flags)
    {
        var result = new List<ItemFlags>();
        foreach (ItemFlags flag in Enum.GetValues(typeof(ItemFlags)))
        {
            if (flags.HasFlag(flag))
            {
                result.Add(flag);
            }
        }
        return result;
    }

    public static ItemFlags CopyFlags(this ItemFlags flags, ItemFlags copyFlags)
    {
        var result = flags;
        foreach (ItemFlags flag in Enum.GetValues(typeof(ItemFlags)))
        {
            if (copyFlags.HasFlag(flag))
            {
                if (!result.HasFlag(flag))
                {
                    result = result.AddFlag(flag);
                }
            }
        }
        return result;
    }
}