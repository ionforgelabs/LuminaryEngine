using LuminaryEngine.Engine.Audio;
using LuminaryEngine.Engine.Core.Rendering.Fonts;
using LuminaryEngine.Engine.Core.Rendering.Sprites;
using LuminaryEngine.Engine.Core.Rendering.Textures;
using Newtonsoft.Json;
using SDL2;

namespace LuminaryEngine.Engine.Core.ResourceManagement;

public class ResourceCache
{
    public static Font DefaultFont;

    private IntPtr _renderer;
    private Dictionary<string, Texture> _textureCache;
    private Dictionary<string, Texture> _spritesheetCache;
    private Dictionary<string, Font> _fontCache;
    private Dictionary<string, Sound> _soundCache;
    private Dictionary<string, AnimationResponse> _animationCache;
    private AudioManager _audioManager;

    private TextureLoadingSystem _textureLoadingSystem;
    private FontLoadingSystem _fontLoadingSystem;

    public ResourceCache(IntPtr renderer, TextureLoadingSystem textureLoadingSystem,
        FontLoadingSystem fontLoadingSystem, AudioManager audioManager)
    {
        _renderer = renderer;
        _textureLoadingSystem = textureLoadingSystem;
        _fontLoadingSystem = fontLoadingSystem;
        _textureCache = new Dictionary<string, Texture>();
        _spritesheetCache = new Dictionary<string, Texture>();
        _fontCache = new Dictionary<string, Font>();
        _audioManager = audioManager;
        _soundCache = new Dictionary<string, Sound>();
        _animationCache = new Dictionary<string, AnimationResponse>();
    }

    public Font GetFont(string fontId, int fontSize)
    {
        string cacheKey = $"{fontId}-{fontSize}";
        if (_fontCache.ContainsKey(cacheKey))
        {
            return _fontCache[cacheKey];
        }

        string fontPath = Path.Combine("Assets", "Fonts", $"{fontId}.ttf");

        Font font = _fontLoadingSystem.LoadFont(fontPath, fontSize);
        _fontCache[cacheKey] = font;

        DefaultFont ??= font;

        return font;
    }

    public Texture GetTexture(string textureId)
    {
        if (_textureCache.ContainsKey(textureId))
        {
            return _textureCache[textureId];
        }

        string texturePath = Path.Combine("Assets", "Textures", textureId);

        Texture texture = _textureLoadingSystem.LoadTexture(_renderer, texturePath);
        texture.AssignTextureId(textureId);

        _textureCache[textureId] = texture;
        return texture;
    }

    public Texture GetSpritesheet(string spritesheetId)
    {
        if (_spritesheetCache.ContainsKey(spritesheetId))
        {
            return _spritesheetCache[spritesheetId];
        }

        string texturePath = Path.Combine("Assets", "Spritesheet", spritesheetId);

        Texture texture = _textureLoadingSystem.LoadTexture(_renderer, texturePath);
        texture.AssignTextureId(spritesheetId);

        _spritesheetCache[spritesheetId] = texture;
        return texture;
    }

    public Sound GetSound(string id)
    {
        if (_soundCache.TryGetValue(id, out Sound sound))
        {
            return sound;
        }

        // Assuming sound IDs correspond to file paths in "Assets/Audio/"
        string filePath = $"Assets/Audio/{id}";
        Sound newSound = _audioManager.LoadSound(id, filePath); // AudioManager handles the actual loading
        if (newSound != null)
        {
            _soundCache[id] = newSound;
            return newSound;
        }

        return null;
    }
    
    public AnimationResponse GetAnimation(string animationId)
    {
        if (_animationCache.TryGetValue(animationId, out AnimationResponse animationResponse))
        {
            return animationResponse;
        }

        string animationPath = Path.Combine("Assets", "Animations", $"{animationId}.json");

        List<Animation> animations = new List<Animation>();
        
        // Load the animation data from the JSON file
        string jsonData = File.ReadAllText(animationPath);
        var animationData = JsonConvert.DeserializeObject<List<JSONAnimation>>(jsonData);
        foreach (var anim in animationData)
        {
            Animation animation = new Animation(anim);
            animations.Add(animation);
        }

        animationResponse = new AnimationResponse
        {
            TextureId = $"{animationId}.png",
            Animations = animations
        };

        _animationCache[animationId] = animationResponse;

        return animationResponse;
    }

    public void ClearCache()
    {
        foreach (var texture in _textureCache.Values)
        {
            texture.Destroy();
        }

        _textureCache.Clear();

        foreach (var font in _fontCache.Values)
        {
            font.Dispose();
        }

        _fontCache.Clear();

        foreach (var sound in _soundCache.Values)
        {
            sound.Dispose();
        }

        _soundCache.Clear();
        
        _animationCache.Clear();
    }
}

public struct AnimationResponse
{
    public string TextureId;
    public List<Animation> Animations;
}