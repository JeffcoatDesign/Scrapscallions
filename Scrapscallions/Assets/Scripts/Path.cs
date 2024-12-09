using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Path : MonoBehaviour
{
    public Transform[] pathNodes;
    private List<LineSegment> _lineSegments = new List<LineSegment>();

    private void Start()
    {
        if (pathNodes.Length < 2) {
            throw new Exception("Woah there! Theres not enough points in this path");
        }
        for (int i = 0; i < pathNodes.Length; i++)
        {
            LineSegment segment = new LineSegment();
            segment.point1 = pathNodes[i].position;
            if (i < pathNodes.Length - 2)
                segment.point2 = pathNodes[i + 1].position;
            else
                segment.point2 = pathNodes[0].position;
            _lineSegments.Add(segment);
        }
    }

    internal float GetParam(Vector3 futurePos, Vector3 position)
    {
        LineSegment closestSegment = GetClosestSegmentOnPath(position);
        Vector3 closestPoint = GetClosestPointOnLine(closestSegment, position);
        int segmentIndex = _lineSegments.IndexOf(closestSegment);
        float result = 0;
        for (int i = 0; i < segmentIndex; i++)
        {
            result += _lineSegments[i].Length;
        }
        result += Mathf.Abs(Vector3.Distance(closestPoint, closestSegment.point1));
        return result;
    }

    internal Vector3 GetPosition(float targetParam)
    {
        Debug.Log($"Target Param: {targetParam}");
        LineSegment currentSegment = null;
        float currentDistance = 0;
        int index = -1;
        while (targetParam > currentDistance && index < _lineSegments.Count - 1)
        {
            index++;
            currentSegment = _lineSegments[index];
            currentDistance += currentSegment.Length;
        }
        Debug.Log($"{currentSegment.point1}, {currentSegment.point2}");
        Vector3 result = Vector3.Lerp(currentSegment.point1, currentSegment.point2, (targetParam - (currentDistance - currentSegment.Length)) / currentSegment.Length);
        return result;
    }

    private LineSegment GetClosestLineSegment(Vector3 input)
    {
        List<LineSegment> segments = _lineSegments;
        segments = segments.OrderBy(x => Vector3.Distance(x.MidPoint, input)).ToList();
        LineSegment result = segments[0];
        return result;
    }

    private LineSegment GetClosestSegmentOnPath (Vector3 point)
    {
        LineSegment result = null;
        float distance = float.MaxValue;
        foreach (LineSegment segment in _lineSegments)
        {
            Vector3 closestPointOnSegment = GetClosestPointOnLine(segment, point);
            float pointDistance = Vector3.Distance(closestPointOnSegment, point);
            if (pointDistance < distance)
            {
                distance = pointDistance;
                result = segment;
            }
        }
        return result;
    }

    private Vector3 GetClosestPointOnLine (LineSegment lineSegment, Vector3 point)
    {
        Vector3 heading = lineSegment.point1 - lineSegment.point2;
        float length = heading.magnitude;
        heading.Normalize();

        Vector3 pointToOrigin = lineSegment.point1 - point;
        float dotProduct = Vector3.Dot(pointToOrigin, heading);
        dotProduct = Mathf.Clamp(dotProduct, 0f, length);
        return lineSegment.point1 + heading * dotProduct;
    }

    //private LineSegment FindLineSegment(Vector3 point1, Vector3 point2)
    //{
    //    LineSegment result = null;
    //    foreach (LineSegment segment in _lineSegments)
    //    {
    //        if ((segment.point1 == point1 && segment.point2 == point2) || (segment.point1 == point2 && segment.point2 == point1))
    //            result = segment;
    //    }
    //    return result;
    //}
}

public class LineSegment {
    public Vector3 point1;
    public Vector3 point2;

    public float Length { get { return Mathf.Abs(Vector3.Distance(point1, point2)); } }
    public Vector3 MidPoint { get { return Vector3.Lerp(point1, point2, 0.5f); } }
}