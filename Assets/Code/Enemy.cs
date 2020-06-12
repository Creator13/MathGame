using DevMath;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Vector2 = DevMath.Vector2;

public class Enemy {
    private const float CHASE_DISTANCE = 250.0f;
    private readonly float moveSpeed = 300.0f;

    private readonly Texture2D visual;

    public Enemy(Vector2 position) {
        visual = Resources.Load<Texture2D>("pacman");

        Circle = new Circle();
        Circle.Radius = visual.width * .5f;

        Position = position;
    }

    public Vector2 Position {
        get => Circle.Position;
        set => Circle.Position = value;
    }

    public Circle Circle { get; }

    public float Rotation { get; private set; }

    public void Render() {
        GUIUtility.RotateAroundPivot(Rotation, Position.ToUnity());

        GUI.color = Color.red;

        GUI.DrawTexture(new Rect(Position.x - Circle.Radius, Position.y - Circle.Radius, visual.width, visual.height), visual);

        GUI.color = Color.white;

        GUI.matrix = Matrix4x4.identity;
    }

    public void Update(Player player) {
        var directionToPlayer = player.Position - Position;
        var distanceToPlayer = directionToPlayer.Magnitude;

        if (distanceToPlayer < CHASE_DISTANCE) {
            var playerFacing = UnityEngine.Vector2.Dot(directionToPlayer.Normalized.ToUnity(), player.Direction.ToUnity());

            Vector2 moveDirection;
            if (playerFacing > .0f) {
                moveDirection = directionToPlayer.Normalized;
            }
            else {
                moveDirection = -directionToPlayer.Normalized;
            }

            Position += moveDirection * moveSpeed * Time.deltaTime;

            Rotation = DevMath.DevMath.RadToDeg(Vector2.Angle(new Vector2(.0f, .0f), moveDirection));
        }
    }
}
