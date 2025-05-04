namespace LuminaryEngine.Engine.Gameplay.Combat;

public class CombatState
{
    private List<Combatant> _combatants;

    public bool IsActive { get; set; }

    public CombatState()
    {
        _combatants = new List<Combatant>();
        IsActive = true;
    }

    public void AddCombatant(Combatant combatant)
    {
        _combatants.Add(combatant);
    }

    public void RemoveCombatant(Combatant combatant)
    {
        _combatants.Remove(combatant);
    }

    public List<Combatant> GetAllCombatants()
    {
        return _combatants;
    }

    public Combatant GetPlayerCombatant()
    {
        return _combatants.Find(c => c.IsPlayer);
    }

    public Combatant GetEnemyTarget()
    {
        return _combatants.Find(c => !c.IsPlayer);
    }

    public bool IsBattleOver()
    {
        return _combatants.Count == 0 || _combatants.All(c => c.IsPlayer) || _combatants.All(c => !c.IsPlayer);
    }
}