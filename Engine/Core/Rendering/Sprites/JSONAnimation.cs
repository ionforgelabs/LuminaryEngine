using Newtonsoft.Json;
using SDL2;

namespace LuminaryEngine.Engine.Core.Rendering.Sprites;

public class JSONAnimation
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("frames")]
    public List<JSONRect> Frames { get; set; }
    [JsonProperty("frameDuration")]
    public float FrameDuration { get; set; }
    [JsonProperty("isLooping")]
    public bool IsLooping { get; set; }
}

public class JSONRect
{
    [JsonProperty("x")]
    public int X { get; set; }
    [JsonProperty("y")]
    public int Y { get; set; }
    [JsonProperty("w")]
    public int W { get; set; }
    [JsonProperty("h")]
    public int H { get; set; }
}