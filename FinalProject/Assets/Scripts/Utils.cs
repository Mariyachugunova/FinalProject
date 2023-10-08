using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public static class Utils
{
    public static bool IsCBetweenAB(Vector3 A, Vector3 B, Vector3 C)
    {
        return Vector3.Dot((B - A).normalized, (C - B).normalized) <= 0f && Vector3.Dot((A - B).normalized, (C - A).normalized) <= 0f;
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
    public static List<RaycastResult> Raycast()
    {
        PointerEventData _pointerData = new(EventSystem.current);
        List<RaycastResult> _raycastResult = new();

        _pointerData.position = Mouse.current.position.ReadValue();
        EventSystem.current.RaycastAll(_pointerData, _raycastResult);
        return _raycastResult;     
    }
}
