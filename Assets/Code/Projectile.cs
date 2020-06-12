using DevMath;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Vector2 = DevMath.Vector2;

public class Projectile {
    private const float SCALE = .25f;

    public const float LIFETIME = 5.0f;
    private readonly float accelerationPerSecond;
    private float lifetime;

    private float velocity;
    private readonly Texture2D visual;

    public Projectile(Vector2 position, Vector2 direction, float startVelocity, float acceleration) {
        visual = Resources.Load<Texture2D>("pacman");

        Circle = new Circle {
            Position = position,
            Radius = visual.width * .5f * SCALE
        };

        velocity = startVelocity;
        accelerationPerSecond = acceleration;

        Direction = direction;
    }

    public Circle Circle { get; }

    private Vector2 Position {
        get => Circle.Position;
        set => Circle.Position = value;
    }

    public bool ShouldDie => lifetime >= LIFETIME;

    private Vector2 Direction { get; }

    public void Render() {
        GUI.color = Color.blue;

        GUIUtility.RotateAroundPivot(Vector2.Angle(new Vector2(.0f, .0f), Direction), Position.ToUnity());

        GUIUtility.ScaleAroundPivot(UnityEngine.Vector2.one * SCALE, Position.ToUnity());

        GUI.DrawTexture(
            new Rect(Position.x - visual.width * .5f, Position.y - visual.height * .5f, visual.width, visual.height),
            visual);

        GUI.matrix = Matrix4x4.identity;

        GUI.color = Color.white;
    }

    public void Update() {
        velocity += accelerationPerSecond * Time.deltaTime;

        Position += Direction * velocity * Time.deltaTime;

        lifetime += Time.deltaTime;
    }
}
