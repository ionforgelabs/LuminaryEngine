using Newtonsoft.Json;

namespace LuminaryEngine.Engine.Gameplay.Combat;

public class JSONCombat
{
    [JsonProperty("combatId")]
    public string CombatId { get; set; }
    [JsonProperty("combatants")]
    public List<JSONCombatant> Combatants { get; set; }
    [JsonProperty("backgroundTextureId")]
    public string BackgroundTextureId { get; set; }
    [JsonProperty("musicId")]
    public string MusicId { get; set; }
    [JsonProperty("victoryMusicId")]
    public string VictoryMusicId { get; set; }
    [JsonProperty("defeatMusicId")]
    public string DefeatMusicId { get; set; }
}

public class JSONCombatant
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("health")]
    public int Health { get; set; }
    [JsonProperty("attack")]
    public int Attack { get; set; }
    [JsonProperty("defense")]
    public int Defense { get; set; }
    [JsonProperty("speed")]
    public int Speed { get; set; }
    [JsonProperty("spiritType")]
    public int SpiritType { get; set; }
    [JsonProperty("textureId")]
    public string TextureId { get; set; }
    [JsonProperty("hasAnimations")]
    public bool HasAnimations { get; set; }
}