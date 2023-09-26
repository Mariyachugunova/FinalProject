using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{
    [SerializeField] private PathPoint _currentPathPoint;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _dialogPoint;
    //[Inject]
    [SerializeField] private DialoguePanel _dialoguePanel;
    private PathPoint[] _allPoints;
    private float moveSpeed = 3;

    public event Action<float> playerTurn;
    public event Action ReachPlace;

    private void Start()
    {
        EventManager.ArrowClick += MoveTo;

    }

    private void OnDestroy()
    {
        EventManager.ArrowClick -= MoveTo;
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


    private List<Vector3> _path;
    private PathPoint _targetPathPoint;
    private int _step = -1;

    private List<List<PathPoint>> _listOfPaths = new();

   
    public void GoFromPointToPoint(PathPoint startPoint, PathPoint finishPont)
    {
        if(_targetPathPoint != null)_targetPathPoint._inPlase = false;
        _allPoints = FindObjectsOfType<PathPoint>();
        _currentPathPoint = startPoint;
        _targetPathPoint = finishPont;
        _listOfPaths.Clear();
        ClearPath();
        PathFinder(_currentPathPoint, new());
        int min = _listOfPaths.Min(_=>_.Count);
        _path = _listOfPaths.Where(_ => _.Count == min).First().Select(e => e.transform.position).ToList();
        _step = -1;
        Go();

    }

    private void ClearPath()
    {
        for(int i = 0; i < _allPoints.Length; i++)
        {
            _allPoints[i]._check = false;
        } 
    }
    public void PathFinder(PathPoint pathPoint,List<PathPoint> list)
        {

        foreach(var item in pathPoint._derections) 
        {
            List<PathPoint> newlist = new(list);
            newlist.Add(pathPoint);
            if(pathPoint == _targetPathPoint)
            {
                _listOfPaths.Add(newlist);
                return;
            }

            pathPoint._check = true;
            if(!item.Value._check)
            {                
                PathFinder(item.Value, newlist);               
            }
        }

    }
    public void Go()
    {
        if(_step < _path.Count - 1)
        {
            _step++;
            
            FaceToX(_path[_step].x);
            transform.DOMove(_path[_step], moveSpeed).SetSpeedBased().SetEase(Ease.Linear).onComplete = Go;
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


    public void MoveTo(Direction direction)
    {
       
    }

    [Button]
    public void MoveLeft( )
    {
      
    }
   
    [Button]
    public void MoveRight()
    {
       
 
    }
    [Button]
    public void MoveUp()
    {
       
    }
    [Button]
    public void MoveDown()
    {
      
    }
}
