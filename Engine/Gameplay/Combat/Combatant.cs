using LuminaryEngine.Engine.Gameplay.Spirits;

namespace LuminaryEngine.Engine.Gameplay.Combat;

public class Combatant
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public SpiritType SpiritType { get; set; }
    public bool IsPlayer { get; set; }
    
    public string TextureId { get; set; }

    public Combatant(string name, int health, int attack, int defense, int speed, SpiritType spiritType, bool isPlayer)
    {
        Name = name;
        Health = health;
        Attack = attack;
        Defense = defense;
        Speed = speed;
        SpiritType = spiritType;
        IsPlayer = isPlayer;
    }

    public Combatant(JSONCombatant jsonCombatant)
    {
        Name = jsonCombatant.Name;
        Health = jsonCombatant.Health;
        Attack = jsonCombatant.Attack;
        Defense = jsonCombatant.Defense;
        Speed = jsonCombatant.Speed;
        SpiritType = (SpiritType)jsonCombatant.SpiritType;
        TextureId = jsonCombatant.TextureId;
    }
}