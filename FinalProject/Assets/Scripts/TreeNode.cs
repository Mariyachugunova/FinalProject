using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum Direction
    {   left,
        right,
        up,
        down
    }

public enum PathPointType
{
    Go,
    Look,
    Talk,
    Pick
}
public class TreeNode :SerializedMonoBehaviour{
    [SerializeField] PathPointType pathPointType;
    public PathPointType PointType => pathPointType;
    private string[] arrowSpritesNames = new string[] {
        "_go",
        "_look",
        "_talk",
        "_pick"
    };
    [SerializeField] private DialogueData _dialogueData;
    public DialogueData DialogueData => _dialogueData;
    [Inject]
    private void Contstruct(ArrowIcone arrowIconePrefab)
    {
        InstantiateArrow(Direction.down, arrowIconePrefab);
        InstantiateArrow(Direction.left, arrowIconePrefab);
        InstantiateArrow(Direction.right, arrowIconePrefab);
        InstantiateArrow(Direction.up, arrowIconePrefab);

    }



    private void InstantiateArrow(Direction direction, ArrowIcone arrowIconePrefab)
    {
        if(derections.ContainsKey(direction))
        {
            var arrowIcone = Instantiate(arrowIconePrefab, transform.position, Quaternion.identity, transform);
            TreeNode nextPoint = derections[direction];
            arrowIcone.Initialize(nextPoint.transform, arrowSpritesNames[(int)nextPoint.PointType], direction);
        }
    }



    public Dictionary<Direction, TreeNode> derections = new Dictionary<Direction, TreeNode>();

    [Button]
    public void SetActiveAll(bool active)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
           
             transform.GetChild(i).gameObject.SetActive(active);
          
        }
    }
    public void DeselectAll()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<InteractiveIcon>()?.DeselectAndHide();
            

        }
    }

    [SerializeField] private DialogNode nodePrefub;
    private DialogNode node;
    [Button]
    private void CreateTree()
    {
        int n = 0;
        while(n < _dialogueData.phrases.Length)
        {
            node = Instantiate(nodePrefub, node.transform);
            node._phrase = _dialogueData.phrases[n];
            node.frasa = _dialogueData.phrases[n].Phrase;
            n++;
            while(_dialogueData.phrases[n].AddSelection)
            {

                DialogNode fork = Instantiate(nodePrefub, node.transform);
                fork._phrase = _dialogueData.phrases[n];
                fork.frasa = _dialogueData.phrases[n].Phrase;
                n++;

            }

        }
    }

}
