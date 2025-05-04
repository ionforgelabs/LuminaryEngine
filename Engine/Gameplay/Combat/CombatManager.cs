using LuminaryEngine.Engine.Gameplay.Items;
using Newtonsoft.Json;

namespace LuminaryEngine.Engine.Gameplay.Combat;

public class CombatManager
{
    private static CombatManager instance = null;

    private CombatManager()
    {
    }

    public static CombatManager Instance
    {
        get { return instance ??= new CombatManager(); }
    }

    private Dictionary<string, Combat> _combatDictionary = new();

    public void LoadCombats()
    {
        if (!File.Exists(Path.Combine("Assets", "Combats", "combats.json")))
        {
            throw new FileNotFoundException("Combats JSON file not found", Path.Combine("Assets", "Combats", "combats.json"));
        }

        JsonConvert.DeserializeObject<List<JSONCombat>>(File.ReadAllText(Path.Combine("Assets", "Combats", "combats.json")))
            ?.ForEach(combatJson =>
            {
                Combat combat = new(combatJson);
                combat.Combatants.ForEach(combatant =>
                {
                    combatant.IsPlayer = false;
                });
                
                _combatDictionary.Add(combatJson.CombatId, combat);
            });
    }
    
    public Combat GetCombat(string combatId)
    {
        if (_combatDictionary.TryGetValue(combatId, out var combat))
        {
            return combat;
        }

        throw new KeyNotFoundException($"Combat with ID '{combatId}' not found.");
    }
}