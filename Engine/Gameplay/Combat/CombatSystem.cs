using LuminaryEngine.Engine.ECS;

namespace LuminaryEngine.Engine.Gameplay.Combat;

public class CombatSystem : LuminSystem
{
    private Queue<Combatant> _turnQueue;
    private CombatState _combatState;
    
    private readonly SpiritTypeSystem _spiritTypeSystem = new SpiritTypeSystem();

    public CombatSystem(World world) : base(world)
    {
        _turnQueue = new Queue<Combatant>();
        _combatState = new CombatState();
    }

    public override void Update()
    {
        if (!_combatState.IsActive)
            return;

        if (_turnQueue.Count == 0)
        {
            InitializeTurnQueue();
        }

        var currentCombatant = _turnQueue.Dequeue();

        if (currentCombatant.IsPlayer)
        {
            HandlePlayerTurn(currentCombatant);
        }
        else
        {
            HandleEnemyTurn(currentCombatant);
        }

        if (_combatState.IsBattleOver())
        {
            EndCombat();
        }
    }

    private void InitializeTurnQueue()
    {
        var combatants = _combatState.GetAllCombatants();
        var orderedCombatants = combatants.OrderByDescending(c => c.Speed).ToList();

        foreach (var combatant in orderedCombatants)
        {
            _turnQueue.Enqueue(combatant);
        }
    }

    private void HandlePlayerTurn(Combatant player)
    {
        Console.WriteLine("Player's Turn! Choose an action:");
        Console.WriteLine("1. Attack");
        Console.WriteLine("2. Use Spirit Ability");
        Console.WriteLine("3. Shift Focus");
        Console.WriteLine("4. Use Item");
        Console.WriteLine("5. Defend");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                PerformAttack(player, _combatState.GetEnemyTarget());
                break;
            case "2":
                UseSpiritAbility(player);
                break;
            case "3":
                ShiftFocus(player);
                break;
            case "4":
                UseItem(player);
                break;
            case "5":
                Defend(player);
                break;
            default:
                Console.WriteLine("Invalid choice. Skipping turn.");
                break;
        }
    }

    private void HandleEnemyTurn(Combatant enemy)
    {
        Console.WriteLine($"{enemy.Name}'s Turn!");
        PerformAttack(enemy, _combatState.GetPlayerCombatant());
    }

    private void PerformAttack(Combatant attacker, Combatant target)
    {
        var damage = CalculateDamage(attacker, target);
        target.Health -= damage;

        Console.WriteLine($"{attacker.Name} attacked {target.Name} for {damage} damage!");

        if (target.Health <= 0)
        {
            Console.WriteLine($"{target.Name} has been defeated!");
            _combatState.RemoveCombatant(target);
        }
    }

    private int CalculateDamage(Combatant attacker, Combatant target)
    {
        float typeEffectiveness = _spiritTypeSystem.GetEffectiveness(attacker.SpiritType, target.SpiritType);
        int baseDamage = attacker.Attack - target.Defense;
        return (int)(baseDamage * typeEffectiveness);
    }

    private void UseSpiritAbility(Combatant combatant)
    {
        Console.WriteLine($"{combatant.Name} used a Spirit Ability!");
        // Implement Spirit ability logic here
    }

    private void ShiftFocus(Combatant combatant)
    {
        Console.WriteLine($"{combatant.Name} shifted Spirit focus!");
        // Implement focus-shifting logic here
    }

    private void UseItem(Combatant combatant)
    {
        Console.WriteLine($"{combatant.Name} used an item!");
        // Implement item usage logic here
    }

    private void Defend(Combatant combatant)
    {
        Console.WriteLine($"{combatant.Name} is defending!");
        // Implement defense logic here
    }

    private void EndCombat()
    {
        Console.WriteLine("Combat has ended!");
        _combatState.IsActive = false;
    }
}