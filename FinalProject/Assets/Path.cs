using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public class Path : MonoBehaviour, IPointerClickHandler
{ 
    [SerializeField] private GameObject _arrow;
    private PathPoint[] _allPoints;
    [SerializeField] private PathPoint wanderingPoint1;
    [SerializeField] private PathPoint wanderingPoint2;
    private MovementController _movementController;
    [Inject]
    private void Construct(MovementController movementController)
    {
        _movementController = movementController;
    }
   
    private void Start()
    {
        _allPoints = GetComponentsInChildren<PathPoint>();
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private Vector3 Projection(Vector3 mousepos,out PathPoint point1, out PathPoint point2)
    {
        float mindist = float.MaxValue;
        Vector3 nearestPoint = mousepos;
        point1 = null;
        point2 = null;
        for(int i = 0; i < _allPoints.Length; i++)
            foreach(var item in _allPoints[i]._derections)
            {
                Vector3 p1 = _allPoints[i].transform.position;
                Vector3 p2 = item.Value.transform.position;
                Vector3 point = Utils.CalcProjection(p1, mousepos, p2);
                float dist = Vector3.Distance(mousepos,point);

                if(dist < mindist && Utils.IsCBetweenAB(p1, p2, mousepos))
                {
                    mindist = dist;
                    nearestPoint = point;
                    point1 = _allPoints[i];
                    point2 = item.Value;
                }

            }
        return nearestPoint;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
      
        PathPoint point1;
        PathPoint point2;
        RemovePointFromPath(wanderingPoint1);
        RemovePointFromPath(wanderingPoint2);
        
        Vector3 startPoint = Projection(_movementController.transform.position, out point1, out point2); 
        AddPointAtPath(startPoint, wanderingPoint1, point1, point2);

        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 finishPoint = Projection(mousePoint,out point1, out point2);
        _arrow.transform.position = finishPoint;
        _arrow.gameObject.SetActive(true);
        
        
        AddPointAtPath(finishPoint, wanderingPoint2, point1, point2);
       _movementController.GoFromPointToPoint(wanderingPoint1, wanderingPoint2);

    }
    public void GoToPoint(PathPoint targetPoint)
    {

        PathPoint point1;
        PathPoint point2;
        RemovePointFromPath(wanderingPoint1);
        RemovePointFromPath(wanderingPoint2);

        Vector3 startPoint = Projection(_movementController.transform.position, out point1, out point2);
        AddPointAtPath(startPoint, wanderingPoint1, point1, point2);
        _arrow.transform.position = targetPoint.transform.position;
        _arrow.gameObject.SetActive(true);
        _movementController.GoFromPointToPoint(wanderingPoint1, targetPoint);

    }


    private void RemovePointFromPath(PathPoint point)
    {
        point.transform.SetParent(null);
        if(point._derections.Count == 2)
        {
            PathPoint[] pathPoints =  point._derections.Values.ToArray();
            PathPoint.Direction key1 = PathPoint.Direction.left;
            foreach(var item in pathPoints[0]._derections)
            {
                if(item.Value == point)
                {
                    key1 = item.Key;
                        
                }

            }
            pathPoints[0]._derections[key1] = pathPoints[1];


            foreach(var item in pathPoints[1]._derections)
            {
                if(item.Value == point)
                {
                    key1 = item.Key;
                }

            }
            pathPoints[1]._derections[key1] = pathPoints[0];
        }


    }
    private void AddPointAtPath(Vector3 pos, PathPoint point0, PathPoint point1, PathPoint point2)
    {
        point0.transform.position = pos;

        PathPoint.Direction key1 = PathPoint.Direction.left;
        PathPoint.Direction key2 = PathPoint.Direction.right;
        foreach(var item in point1._derections)
        {
            if(item.Value == point2)
            {                
                key1 = item.Key;
            }

        }
        point1._derections[key1] = point0;
        foreach(var item in point2._derections)
        {
            if(item.Value == point1)
            {
                key2 = item.Key;
            }

        }
        point2._derections[key2] = point0;
        point0._derections.Clear();
        point0._derections.Add(key1, point2);
        point0._derections.Add(key2, point1); 
        point0.transform.SetParent(transform);
    }
}
