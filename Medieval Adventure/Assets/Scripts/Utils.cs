using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static bool IsCBetweenAB(Vector3 A, Vector3 B, Vector3 C)
    {
        return Vector3.Dot((B - A).normalized, (C - B).normalized) < 0f && Vector3.Dot((A - B).normalized, (C - A).normalized) < 0f;
    }


    public static Vector3 CalcProjection(Vector3 A, Vector3 B, Vector3 C)
    {
        var a = B - A;
        var c = C - A;
        var X = A + Vector3.Project(a, c.normalized);
        return X;
    }

    public static void Rebuild(Transform transform)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

}
