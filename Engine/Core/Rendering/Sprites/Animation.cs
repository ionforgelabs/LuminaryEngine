using SDL2;

namespace LuminaryEngine.Engine.Core.Rendering.Sprites;

public class Animation
{
    public string Name { get; set; }
    public List<SDL.SDL_Rect> Frames { get; set; }
    public float FrameDuration { get; set; } // Duration of each frame in seconds
    public bool IsLooping { get; set; }

    public Animation(string name, List<SDL.SDL_Rect> frames, float frameDuration, bool isLooping)
    {
        Name = name;
        Frames = frames;
        FrameDuration = frameDuration;
        IsLooping = isLooping;
    }
    
    public Animation(JSONAnimation jsonAnimation) 
    {
        Name = jsonAnimation.Name;
        Frames = new List<SDL.SDL_Rect>();
        foreach (var frame in jsonAnimation.Frames)
        {
            Frames.Add(new SDL.SDL_Rect { x = frame.X, y = frame.Y, w = frame.W, h = frame.H });
        }
        FrameDuration = jsonAnimation.FrameDuration;
        IsLooping = jsonAnimation.IsLooping;
    }
}