using SDL2;

namespace LuminaryEngine.Engine.Core.Rendering.Sprites;

public class Animation
{
    public string Name { get; set; }
    public List<SDL.SDL_Rect> Frames { get; set; }
    public float FrameDuration { get; set; } // Duration of each frame in seconds
    public bool IsLooping { get; set; }
    public bool InvertedLooping { get; set; } // If true, the animation will play in reverse after reaching the end
    public bool ReturnToStart { get; set; } // If true, the animation will return to the start frame after playing

    public Animation(string name, List<SDL.SDL_Rect> frames, float frameDuration, bool isLooping, bool invertedLooping)
    {
        Name = name;
        Frames = frames;
        FrameDuration = frameDuration;
        IsLooping = isLooping;
        InvertedLooping = invertedLooping;
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
        InvertedLooping = jsonAnimation.InvertedLooping;
    }
}