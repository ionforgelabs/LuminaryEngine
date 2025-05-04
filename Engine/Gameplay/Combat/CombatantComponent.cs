using LuminaryEngine.Engine.ECS;

namespace LuminaryEngine.Engine.Gameplay.Combat;

public class CombatantComponent : IComponent
{
    public Combatant Combatant { get; set; }
    
    public CombatantComponent(Combatant combatant)
    {
        Combatant = combatant;
    }
}