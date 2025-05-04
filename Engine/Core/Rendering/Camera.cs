using System.Numerics;
using LuminaryEngine.Engine.Core.GameLoop;
using LuminaryEngine.Engine.ECS;

namespace LuminaryEngine.Engine.Core.Rendering;

public class Camera
{
    public float X { get; set; }
    public float Y { get; set; }

    private World _world;

    private bool _lockPosition;
    private Vector2 _lastPosition;

    public Camera(int initialX, int initialY, World world)
    {
        X = initialX;
        Y = initialY;
        _world = world;
    }

    public void Follow(Vector2 target)
    {
        if (_world.GetCurrentLevelId() < 0) return;
        
        if (_lockPosition)
        {
            // If the camera is locked, do not update its position.
            return;
        }

        // Center the camera on the target immediately.
        Vector2 desiredPosition = target - new Vector2(Game.DISPLAY_WIDTH * 0.5f, Game.DISPLAY_HEIGHT * 0.5f);
        int clampedX, clampedY;
        if (_world.GetCurrentLevel().PixelWidth - Game.DISPLAY_WIDTH < 0)
        {
            clampedX = -((Game.DISPLAY_WIDTH - _world.GetCurrentLevel().PixelWidth) / 2);
        }
        else
        {
            clampedX = (int)Math.Clamp(desiredPosition.X, 0, _world.GetCurrentLevel().PixelWidth - Game.DISPLAY_WIDTH);
        }

        if (_world.GetCurrentLevel().PixelHeight - Game.DISPLAY_HEIGHT < 0)
        {
            clampedY = -((Game.DISPLAY_HEIGHT - _world.GetCurrentLevel().PixelHeight) / 2);
        }
        else
        {
            clampedY = (int)Math.Clamp(desiredPosition.Y, 0,
                _world.GetCurrentLevel().PixelHeight - Game.DISPLAY_HEIGHT);
        }

        X = clampedX;
        Y = clampedY;
    }
    
    public void LockPosition()
    {
        _lockPosition = true;
        _lastPosition = new Vector2(X, Y);
        X = 0;
        Y = 0;
    }
    
    public void UnlockPosition()
    {
        _lockPosition = false;
        X = _lastPosition.X;
        Y = _lastPosition.Y;
        
        _lastPosition = Vector2.Zero;
    }
}