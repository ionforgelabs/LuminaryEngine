namespace LuminaryEngine.Engine.Gameplay.Combat;

public class Combat
{
    public string CombatId { get; set; }
    public List<Combatant> Combatants { get; set; }
    public string BackgroundTextureId { get; set; }
    public string MusicId { get; set; }
    public string VictoryMusicId { get; set; }
    public string DefeatMusicId { get; set; }

    public Combat(JSONCombat combat)
    {
        CombatId = combat.CombatId;
        BackgroundTextureId = combat.BackgroundTextureId;
        MusicId = combat.MusicId;
        VictoryMusicId = combat.VictoryMusicId;
        DefeatMusicId = combat.DefeatMusicId;
        Combatants = new List<Combatant>();
        foreach (var jsonCombatant in combat.Combatants)
        {
            Combatants.Add(new Combatant(jsonCombatant));
        }
    }
}