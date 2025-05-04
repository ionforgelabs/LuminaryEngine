using LuminaryEngine.Engine.Gameplay.Spirits;

namespace LuminaryEngine.Engine.Gameplay.Combat;

public class SpiritTypeSystem
{
    private readonly Dictionary<(SpiritType attackerType, SpiritType defenderType), float> _typeEffectiveness;

    public SpiritTypeSystem()
    {
        _typeEffectiveness = new Dictionary<(SpiritType, SpiritType), float>
        {
            { (SpiritType.Fire, SpiritType.Earth), 2.0f },
            { (SpiritType.Earth, SpiritType.Fire), 0.5f },
            { (SpiritType.Water, SpiritType.Fire), 2.0f },
            { (SpiritType.Fire, SpiritType.Water), 0.5f },
            { (SpiritType.Light, SpiritType.Shadow), 2.0f },
            { (SpiritType.Shadow, SpiritType.Shadow), 2.0f },
            // Add more type matchups as needed
        };
    }

    public float GetEffectiveness(SpiritType attackerType, SpiritType defenderType)
    {
        if (_typeEffectiveness.TryGetValue((attackerType, defenderType), out var effectiveness))
        {
            return effectiveness;
        }

        // Default to neutral effectiveness
        return 1.0f;
    }
}