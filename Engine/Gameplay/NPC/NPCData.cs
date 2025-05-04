using System.Numerics;
using LuminaryEngine.Engine.Gameplay.Dialogue;
using LuminaryEngine.Engine.Gameplay.Items;

namespace LuminaryEngine.Engine.Gameplay.NPC;

public class NPCData
{
    public Vector2 Position { get; set; }
    public string TextureName { get; set; }
    public NPCType Type { get; set; }
    public bool Interactive { get; set; }
    public DialogueNode Dialogue { get; set; }

    // Item Giver Specific
    public bool IsSpiritEssence { get; set; }
    public string ItemId { get; set; }
    public ItemFlags ItemFlags { get; set; }
    public Dictionary<string, float> ItemStats { get; set; }
    public int ItemAmount { get; set; }
    public bool IsRepeatable { get; set; }
    public DialogueNode ErrorDialogue { get; set; }
    public bool HasInteracted { get; set; }
    
    // Combatant Specific
    public string CombatId { get; set; }
}