using LuminaryEngine.Engine.ECS;

namespace LuminaryEngine.Engine.Gameplay.Combat;

public class CombatState
{
    private List<Entity> _combatants;

    public bool IsActive { get; set; }

    public CombatState()
    {
        _combatants = new List<Entity>();
        IsActive = false;
    }

    public void AddCombatant(Entity combatant)
    {
        _combatants.Add(combatant);
    }

    public void RemoveCombatant(Entity combatant)
    {
        _combatants.Remove(combatant);
    }

    public List<Entity> GetAllCombatants()
    {
        return _combatants;
    }

    public Entity GetPlayerCombatant()
    {
        return _combatants.Find(c => c.GetComponent<CombatantComponent>().Combatant.IsPlayer);
    }

    public Entity GetEnemyTarget()
    {
        return _combatants.Find(c => c.GetComponent<CombatantComponent>().Combatant.IsPlayer == false);
    }

    public bool IsBattleOver()
    {
        return _combatants.Count == 0 || _combatants.All(c => c.GetComponent<CombatantComponent>().Combatant.IsPlayer) || _combatants.All(c => !c.GetComponent<CombatantComponent>().Combatant.IsPlayer);
    }
}