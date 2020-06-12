using DevMath;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Rigidbody = DevMath.Rigidbody;
using Vector2 = DevMath.Vector2;

public class Player {
    private const float MAX_CHARGE_TIME = 1.0f;

    private const float MIN_PROJECTILE_START_VELOCITY = .0f;
    private const float MAX_PROJECTILE_START_VELOCITY = 10.0f;
    private const float MIN_PROJECTILE_START_ACCELERATION = 10.0f;
    private const float MAX_PROJECTILE_START_ACCELERATION = 20.0f;

    private readonly float moveSpeed = 500.0f;

    private float chargeTime;

    private readonly Texture2D pixel;

    private readonly Rigidbody rigidbody;
    private readonly Texture2D visual;

    public Player() {
        visual = Resources.Load<Texture2D>("pacman");

        Circle = new Circle();
        Circle.Radius = visual.width * .5f;

        pixel = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        pixel.SetPixel(0, 0, Color.white);
        pixel.Apply();

        Position = new Vector2(Screen.width * .5f - visual.width * .5f, Screen.height * .5f - visual.height * .5f);

        rigidbody = new Rigidbody {
            mass = 1.0f,
            force = 150.0f,
            dragCoefficient = .47f
        };
    }

    public Vector2 Position {
        get => Circle.Position;
        set => Circle.Position = value;
    }

    public Vector2 Direction => Vector2.DirectionFromAngle(Rotation);

    public Circle Circle { get; }

    public float Rotation { get; private set; }

    public void Render() {
        GUIUtility.RotateAroundPivot(Rotation, Position.ToUnity());

        GUI.DrawTexture(new Rect(Position.x - Circle.Radius, Position.y - Circle.Radius, visual.width, visual.height), visual);

        var p = DevMath.DevMath.Clamp(chargeTime, .0f, MAX_CHARGE_TIME) / MAX_CHARGE_TIME;
        var fireVelocity = DevMath.DevMath.Lerp(MIN_PROJECTILE_START_VELOCITY, MAX_PROJECTILE_START_VELOCITY, p);
        var fireAcceleration = DevMath.DevMath.Lerp(MIN_PROJECTILE_START_ACCELERATION, MAX_PROJECTILE_START_ACCELERATION, p);

        var distanceTraveled = DevMath.DevMath.DistanceTraveled(fireVelocity, fireAcceleration, Projectile.LIFETIME);

        //Implementeer de Line class met de IntersectsWith(Circle) functie en gebruik deze om de lijn rood te kleuren wanneer je een enemy zou raken
        GUI.DrawTexture(new Rect(Position.x, Position.y, distanceTraveled, 1.0f), pixel);

        GUI.matrix = Matrix4x4.identity;
    }

    private void UpdatePhysics() {
        var forceDirection = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        rigidbody.AddForce(forceDirection, Time.deltaTime);

        Position += rigidbody.Velocity;
    }

    public void Update() {
        UpdatePhysics();

        var mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        var mouseDir = (mousePos - Position).Normalized;

        Rotation = DevMath.DevMath.RadToDeg(Vector2.Angle(new Vector2(.0f, .0f), mouseDir));
        
        if (Input.GetMouseButton(0)) {
            chargeTime += Time.deltaTime;
        }
        else if (Input.GetMouseButtonUp(0)) {
            var p = DevMath.DevMath.Clamp(chargeTime, .0f, MAX_CHARGE_TIME) / MAX_CHARGE_TIME;

            Game.Instance.CreateProjectile(Position, Direction, DevMath.DevMath.Lerp(MIN_PROJECTILE_START_VELOCITY, MAX_PROJECTILE_START_VELOCITY, p), DevMath.DevMath.Lerp(MIN_PROJECTILE_START_ACCELERATION, MAX_PROJECTILE_START_ACCELERATION, p));

            chargeTime = .0f;
        }
    }
}
