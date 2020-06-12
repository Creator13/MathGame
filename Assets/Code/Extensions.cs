using System;
using UnityEngine;

public static class Extensions {
    public static Vector2 ToUnity(this DevMath.Vector2 v) {
        return new Vector2(v.x, v.y);
    }

    public static DevMath.Vector2 ToDevMath(this Vector2 v) {
        return new DevMath.Vector2(v.x, v.y);
    }

    public static Matrix4x4 ToUnity(this DevMath.Matrix4x4 m) {
        throw new NotImplementedException();
    }

    public static DevMath.Matrix4x4 ToDevMath(this Matrix4x4 m) {
        throw new NotImplementedException();
    }
}
