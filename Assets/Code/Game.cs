using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = DevMath.Vector2;

public class Game : MonoBehaviour {
    private List<Enemy> enemies;

    private Player player;

    private List<Projectile> projectiles;

    public static Game Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        player = new Player();

        enemies = new List<Enemy>();
        for (var i = 0; i < 1; ++i) {
            enemies.Add(new Enemy(new Vector2(Random.Range(.0f, Screen.width), Random.Range(.0f, Screen.height))));
        }

        projectiles = new List<Projectile>();
    }

    private void OnGUI() {
        player?.Render();

        enemies.ForEach(e => e.Render());

        projectiles.ForEach(p => p.Render());

        if (player == null) {
            //Use Sin to animate the colour of the text (GUI.color) between alpha 0.5 and 1.0
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Mathf.Sin(Time.time) * .25f + .75f);
            GUI.Label(new Rect(Screen.width * .5f - 50.0f, Screen.height * .5f - 10.0f, 100.0f, 100.0f), "YOU LOSE!");
        }
    }

    public void CreateProjectile(Vector2 position, Vector2 direction, float startVelocity, float acceleration) {
        projectiles.Add(new Projectile(position, direction, startVelocity, acceleration));
    }

    private void ScreenShake() {
        //Implement screen shake with Sin + Matrices
    }

    private void Update() {
        if (player == null) return;

        player.Update();

        enemies.ForEach(e => e.Update(player));

        for (var i = projectiles.Count - 1; i >= 0; i--) {
            projectiles[i].Update();
            if (projectiles[i].ShouldDie) {
                projectiles.RemoveAt(i);
            }
        }

        foreach (var e in enemies) {
            if (e.Circle.CollidesWith(player.Circle)) {
                player = null;
            }
        }

        for (var i = projectiles.Count - 1; i >= 0; i--) {
            for (var j = enemies.Count - 1; j >= 0; --j) {
                if (projectiles[i].Circle.CollidesWith(enemies[j].Circle)) {
                    enemies.RemoveAt(j);
                    projectiles.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
