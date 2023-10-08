using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;


public class InteractiveArrowIcon : InteractiveIcon
{
    [OnValueChanged("ChangeDirection")]
    [SerializeField] private Direction _direction;


    private void ChangeDirection()
    {
        switch(_direction)
        {
            case Direction.left:
            _sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
            break;
            case Direction.up:
            _sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
            break;
            case Direction.right:
            _sprite.transform.rotation = Quaternion.Euler(0, 0, 180);
            break;
            case Direction.down:
            _sprite.transform.rotation = Quaternion.Euler(0, 0, 270);
            break;
        }

    }




}

