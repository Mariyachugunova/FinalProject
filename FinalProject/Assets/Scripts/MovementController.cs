using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class MovementController : MonoBehaviour
{
    [SerializeField] private PathPoint _currentPathPoint;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _dialogPoint;

    public event Action<float> playerTurn;
    public event Action ReachPlace;
    private float _moveSpeedDOTwin = 3;
    private float _moveSpeedVector3 = 0.026f;
    
    private bool _keyboarMove;
    private Path _path;
  
    [Inject]
    private void Construct(Path path)
    {
        _path = path;
    }
    public void FaceToX(float x)
    {
        if(transform.localScale.x > 0 && x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            playerTurn?.Invoke(transform.localScale.x);
        }
        else if(transform.localScale.x < 0 && x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            playerTurn?.Invoke(transform.localScale.x);
        }

    }


    private List<Vector3> _pathVectors;
    private PathPoint _targetPathPoint;
    private int _step = -1;

    private List<List<PathPoint>> _listOfPaths = new();


    public void GoFromPointToPoint(PathPoint startPoint, PathPoint finishPont)
    {
        StopMouseMove();
        _keyboarMove = false;
        if(_targetPathPoint != null) _targetPathPoint._inPlase = false;
        _currentPathPoint = startPoint;
        _targetPathPoint = finishPont;
        _listOfPaths.Clear();
        PathFinder(_currentPathPoint, new());
        //_listOfPaths.ForEach(l => { l.ForEach(p => print(p));print("-------------------------"); });
        int min = _listOfPaths.Min(_ => _.Count);
        _listOfPaths = _listOfPaths.Where(_ => _.Count == min).ToList();
        float[] distance = new float[_listOfPaths.Count];
        for(int i = 0; i < _listOfPaths.Count; i++)
        {
            for(int j = 0; j < _listOfPaths.Count - 1; j++)
            {
              distance[i] +=  Vector3.Distance(_listOfPaths[i][j].transform.position, _listOfPaths[i][j + 1].transform.position);
            }
        }
        int n = 0;
        for(int i = 0; i < distance.Length; i++)
        {
            if(distance[i] < distance[n]) n = i;
        }
        _pathVectors = _listOfPaths[n].Select(p => p.transform.position).ToList();
        _step = -1;
        Go();

    }



    public void PathFinder(PathPoint pathPoint,List<PathPoint> list)
        {

        foreach(var item in pathPoint._derections) 
        {
            List<PathPoint> newlist = new(list);
            if(!newlist.Contains(pathPoint))
            {
                newlist.Add(pathPoint);
            }
                if(pathPoint == _targetPathPoint)
            {
                _listOfPaths.Add(newlist);
                return;
            }

            if(!newlist.Contains(item.Value)){
                newlist.Add(item.Value);
                PathFinder(item.Value, newlist);               
            }
        }

    }
    public void Go()
    {
        if(_step < _pathVectors.Count - 1)
        {
            _step++;
            
            FaceToX(_pathVectors[_step].x);
            transform.DOMove(_pathVectors[_step], _moveSpeedDOTwin).SetSpeedBased().SetEase(Ease.Linear).onComplete = Go;
            _animator.SetBool("go", true);

        }
        else
        {
            InPlace();
        }
    }

    private void InPlace()
    {
         _animator.SetBool("go", false);
         _targetPathPoint._inPlase = true;
         ReachPlace?.Invoke();
    }
    public void StartKeyboardMove()
    {
        if(!_keyboarMove)
        {
            if(DOTween.IsTweening(transform))
            {
                DOTween.Kill(transform);
               
            }
            if(_targetPathPoint) _targetPathPoint._inPlase = false;
            if(_currentPathPoint) _currentPathPoint._inPlase = false;

            _path.RemoveWanderingPoints();
            _animator.SetBool("go", true);
            _keyboarMove = true;
        }
    }
    public void StopMouseMove()
    {
        if(DOTween.IsTweening(transform) && _animator.GetBool("go"))
        {
            DOTween.Kill(transform);
            _targetPathPoint._inPlase = false; 
            _animator.SetBool("go", false);
          
        }
    }
    public void StopKeyboardMove()
    {
        if(_keyboarMove)
        {
            _keyboarMove = false;
            _animator.SetBool("go", false);
        }
    }
   
    public void MoveTo(Direction direction)
    {
        StartKeyboardMove();
        bool betwen2 = false;
        foreach(var item in _path.GetComponentsInChildren<PathPoint>())
        {
            item.GetComponent<SpriteRenderer>().color = Color.white;
        }
        Dictionary<Direction,PathPoint> openWays;
        if(Vector3.Distance(transform.position, _currentPathPoint.transform.position) < 0.2f)
        {
            openWays = new(_currentPathPoint._derections);
        }
        else
        {
            openWays = new();            
            for(int i = 0; i < _currentPathPoint._derections.Values.Count; i++)
            {
                Vector3 p = transform.position;
                Vector3 p1 = _currentPathPoint.transform.position;
                Vector3 p2 = _currentPathPoint._derections.Values.ToArray()[i].transform.position;
                PathPoint pp = _currentPathPoint._derections.Values.ToArray()[i];
                if(Mathf.Abs(Vector3.Distance(p, p1) + Vector3.Distance(p, p2) - Vector3.Distance(p1, p2)) < 0.1f)
                {
                    betwen2 = true;
                    openWays.TryAdd(_currentPathPoint._derections.Keys.ToArray()[i], pp);
                    openWays.TryAdd(pp._derections.FirstOrDefault(x => x.Value == _currentPathPoint).Key, _currentPathPoint);
                }
            }
        }
        if(openWays.ContainsKey(direction))
        {          
            GoTowards(openWays[direction]);
        }
        else if(openWays.Count == 2 && betwen2)
        {
            if(openWays.ContainsKey(Direction.right))
                if(!openWays[Direction.right]._derections.ContainsKey(direction))
                    openWays.Remove(Direction.right);
            if(openWays.ContainsKey(Direction.left))
                if(!openWays[Direction.left]._derections.ContainsKey(direction))
                    openWays.Remove(Direction.left);
            if(openWays.ContainsKey(Direction.down))
                if(!openWays[Direction.down]._derections.ContainsKey(direction))
                    openWays.Remove(Direction.down);
            if(openWays.ContainsKey(Direction.up))
                if(!openWays[Direction.up]._derections.ContainsKey(direction))
                    openWays.Remove(Direction.up);
            if(openWays.Count > 0)
            { 
                PathPoint nearestOpenWay = openWays[Nearest(openWays)];
                if(nearestOpenWay._derections.ContainsKey(direction))
                {
                    GoTowards(nearestOpenWay);
                }
                else
                {

                    StopKeyboardMove();
                }
            }
            else
            {
                StopKeyboardMove();
            }
           
        }
        else
        {
            StopKeyboardMove();
        }
        foreach(var item in openWays.Values)
        {
            item.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void GoTowards(PathPoint openWay)
    {
        FaceToX(openWay.transform.position.x);
        transform.position = Vector3.MoveTowards(transform.position, openWay.transform.position, _moveSpeedVector3);
        if(Vector3.Distance(transform.position, openWay.transform.position) < 0.2f)
        {
            _currentPathPoint = openWay;
        }
    }

    private Direction Nearest(Dictionary<Direction, PathPoint> openWays) {
        int n = 0;
        for(int i = 0; i < openWays.Values.Count; i++)
        {
            if(Vector3.Distance(transform.position, openWays.Values.ToArray()[i].transform.position) < Vector3.Distance(transform.position, openWays.Values.ToArray()[n].transform.position))
            {
                n = i;
            }
        }
        return openWays.Keys.ToArray()[n];
    }

    public void SetCurrentPathPoint(PathPoint pp)
    {
        _currentPathPoint = pp;
    }

    public bool CanGo() {

        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")|| _animator.GetCurrentAnimatorStateInfo(0).IsName("Walk");
    }
}
