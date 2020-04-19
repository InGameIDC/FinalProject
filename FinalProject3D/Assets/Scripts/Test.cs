using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Test ///Author: Ilan
{

    public static void CreateASphre(Vector3 pos)
    {
        CreateASphre(pos, Color.red, 0.5f);
    }

    public static void CreateASphre(Vector3 pos, Color color)
    {
        CreateASphre(pos, color, 0.5f);
    }

    public static void CreateASphre(Vector3 pos, float size)
    {
        CreateASphre(pos, Color.red, size);
    }

    public static void CreateASphre(Vector3 pos, Color color, float size)
    {
        GameObject testSphre = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        testSphre.transform.position = pos;
        testSphre.transform.localScale *= size;
        SetTargetColor(testSphre, color);
    }

    public static IEnumerator ActiveOnIntervals(Action func, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            func();
        }
    }

    public static void SetTargetColor(GameObject target, Color color)
    {
        Renderer rend = target.GetComponent<Renderer>();

        if (rend == null)
            rend.material = new Material(Shader.Find("Specular"));

        rend.material.color = color;
    }

    public static void DrawLine(Vector3 pos1, Vector3 pos2)
    {
        Debug.DrawLine(pos1, pos2, Color.red, 100f);
    }

    public static void DrawLine(Vector3 pos1, Vector3 pos2, Color color)
    {
        Debug.DrawLine(pos1, pos2, color, 100f);
    }

    public static LineRenderer DrawCircle(this GameObject container, float radius, float lineWidth)
    {
        int segments = 360;
        LineRenderer line = container.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);

        return line;
    }

    public static LineRenderer DrawCircle(this GameObject container, float radius, float lineWidth, Color color)
    {
        LineRenderer line = DrawCircle(container, radius, lineWidth);

        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = color;
        line.endColor = color;

        return line;
    }


    public static IEnumerator MarkCircleAtPos(Vector3 pos, float duration)
    {
        yield return MarkCircleAtPos(pos, duration, Color.red);
    }

    public static IEnumerator MarkCircleAtPos(Vector3 pos, float duration, Color color)
    {
        GameObject circle = new GameObject();
        pos.y += 0.05f;
        circle.transform.position = pos;
        DrawCircle(circle, 1f, 0.1f, color);
        yield return new WaitForSeconds(duration);
        circle.SetActive(false);
        MonoBehaviour.Destroy(circle);
    }
}
