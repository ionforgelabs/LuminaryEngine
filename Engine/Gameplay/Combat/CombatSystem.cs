using System.Numerics;
using LuminaryEngine.Engine.Core.GameLoop;
using LuminaryEngine.Engine.Core.Rendering;
using LuminaryEngine.Engine.Core.Rendering.Sprites;
using LuminaryEngine.Engine.Core.ResourceManagement;
using LuminaryEngine.Engine.ECS;
using LuminaryEngine.Engine.ECS.Components;
using LuminaryEngine.Engine.Gameplay.Dialogue;
using LuminaryEngine.Engine.Gameplay.Player;
using LuminaryEngine.Extras;
using Nuke.Common.Utilities;
using SDL2;

namespace LuminaryEngine.Engine.Gameplay.Combat;

public class CombatSystem : LuminSystem
{
    private Queue<Entity> _turnQueue;
    private CombatState _combatState;

    private Game _game;
    
    private readonly SpiritTypeSystem _spiritTypeSystem = new SpiritTypeSystem();

    public CombatSystem(World world, Game game) : base(world)
    {
        _turnQueue = new Queue<Entity>();
        _combatState = new CombatState();
        
        _game = game;
    }
    
    public void InitializeCombat(string combatId)
    {
        Combat combat = CombatManager.Instance.GetCombat(combatId);
        
        Combatant player = _world.GetEntitiesWithComponents(typeof(TransformComponent), typeof(PlayerComponent))[0].GetComponent<CombatantComponent>().Combatant;
        
        _world.LoadCombatBackdrop(combat.BackgroundTextureId, new List<Combatant>(combat.Combatants) { player }, CombatCallback);
    }

    private async void CombatCallback(List<Combatant> combatants)
    {
        foreach (var combatant in combatants)
        {
            Entity entity = _world.CreateEntity();
            if (!combatant.TextureId.IsNullOrEmpty())
            {
                entity.AddComponent(new SpriteComponent(combatant.TextureId, 32));
                entity.GetComponent<SpriteComponent>().IsShifted = false;
            }

            if (combatant.HasAnimations)
            {
                AnimationResponse response = _game.ResourceCache.GetAnimation(combatant.TextureId.Split(".")[0]);
                entity.AddComponent(new AnimationComponent());
                entity.GetComponent<AnimationComponent>().AddAnimations(response.Animations);
                entity.GetComponent<AnimationComponent>().PlayAnimation("Idle");
            }
            
            entity.AddComponent(new CombatantComponent(combatant));
            entity.AddComponent(new TransformComponent(180, 40));
            
            entity.GetComponent<TransformComponent>().Scale = new Vector2(3, 3);
            
            _combatState.AddCombatant(entity);
        }

        DialogueNode node = new DialogueNode("Warning! A frenzied " + combatants[0].Name + " has appeared!");
        _game.DialogueBox.SetDialogue(node);

        await TaskEx.WaitUntil(() => !_game.DialogueBox.IsVisible);
        
        _combatState.IsActive = true;
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

        if (currentCombatant.GetComponent<CombatantComponent>().Combatant.IsPlayer)
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
        var orderedCombatants = combatants.OrderByDescending(c => c.GetComponent<CombatantComponent>().Combatant.Speed).ToList();

        foreach (var combatant in orderedCombatants)
        {
            _turnQueue.Enqueue(combatant);
        }
    }

    private void HandlePlayerTurn(Entity player)
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

    private void HandleEnemyTurn(Entity enemy)
    {
        Console.WriteLine($"{enemy.GetComponent<CombatantComponent>().Combatant.Name}'s Turn!");
        PerformAttack(enemy, _combatState.GetPlayerCombatant());
    }

    private void PerformAttack(Entity attacker, Entity target)
    {
        var damage = CalculateDamage(attacker, target);
        target.GetComponent<CombatantComponent>().Combatant.Health -= damage;

        Console.WriteLine($"{attacker.GetComponent<CombatantComponent>().Combatant.Name} attacked {target.GetComponent<CombatantComponent>().Combatant.Name} for {damage} damage!");

        if (target.GetComponent<CombatantComponent>().Combatant.Health <= 0)
        {
            Console.WriteLine($"{target.GetComponent<CombatantComponent>().Combatant.Name} has been defeated!");
            _combatState.RemoveCombatant(target);
        }
    }

    private int CalculateDamage(Entity attacker, Entity target)
    {
        float typeEffectiveness = _spiritTypeSystem.GetEffectiveness(attacker.GetComponent<CombatantComponent>().Combatant.SpiritType, target.GetComponent<CombatantComponent>().Combatant.SpiritType);
        int baseDamage = attacker.GetComponent<CombatantComponent>().Combatant.Attack - target.GetComponent<CombatantComponent>().Combatant.Defense;
        return (int)(baseDamage * typeEffectiveness);
    }

    private void UseSpiritAbility(Entity combatant)
    {
        Console.WriteLine($"{combatant.GetComponent<CombatantComponent>().Combatant.Name} used a Spirit Ability!");
        // Implement Spirit ability logic here
    }

    private void ShiftFocus(Entity combatant)
    {
        Console.WriteLine($"{combatant.GetComponent<CombatantComponent>().Combatant.Name} shifted Spirit focus!");
        // Implement focus-shifting logic here
    }

    private void UseItem(Entity combatant)
    {
        Console.WriteLine($"{combatant.GetComponent<CombatantComponent>().Combatant.Name} used an item!");
        // Implement item usage logic here
    }

    private void Defend(Entity combatant)
    {
        Console.WriteLine($"{combatant.GetComponent<CombatantComponent>().Combatant.Name} is defending!");
        // Implement defense logic here
    }

    private void EndCombat()
    {
        Console.WriteLine("Combat has ended!");
        _combatState.IsActive = false;
        _world.UnloadCombatBackdrop();
    }
}