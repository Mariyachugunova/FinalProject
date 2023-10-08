using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class DialoguePanel : MonoBehaviour, IPointerClickHandler
{   int _selectedBubleNum = 0;
    [SerializeField] private Transform _dialogues;
    public Dialogue CurrentDialog
    {
        get
        {
            if(_selectedBubleNum >_dialogues.childCount-1) _selectedBubleNum = 0;
            return _dialogues.GetChild(_selectedBubleNum).GetComponent<Dialogue>();
        }
    }
    public Transform DialogPosition { get; set; }
    public bool Selection => _dialogues.transform.childCount > 1;
    public bool _close { get; set; }

    public bool CanGo()
    {
        return !CurrentDialog.Card;
    }
    private Character _playerCharacter;
    private CharacterList _characterList;
    private InfoPanel _infoPanel;
    public Character PlayerCharacter => _playerCharacter;
    private InputController _inputController;
    [Inject]
    private void Construct(Character playerCharacter, CharacterList characterList, InfoPanel infoPanel, InputController inputController) {
        _playerCharacter = playerCharacter;
        _characterList = characterList;
        _infoPanel = infoPanel;
        _inputController = inputController;
    }
   

    private void Start()
    {
        gameObject.SetActive(false);
        _close = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        var raycasts = Utils.Raycast();
        bool dontClose = false;
        raycasts.ForEach(result => {
            if(result.gameObject.GetComponent<Card>()) dontClose = true;
        });

        var characterNames = raycasts.Select(r => r.gameObject.GetComponent<Character>()).Where(n => n != null).ToArray();

        if(characterNames.Length > 0 && CurrentDialog.CurrentDialogueCharacters.Contains(characterNames.FirstOrDefault().CharacterName))
        {
           DialogueContinue();
        }
        else if(!dontClose) 
        {
            Close();
        }
    }
    

    public void Close()
    {
        _close = true;
        for(int i = 0; i < _dialogues.childCount; i++)
        {
            GameObject dialog = _dialogues.GetChild(i).gameObject;
            if(dialog.activeSelf) dialog.GetComponent<Dialogue>().Hide();
        }
        _selectedBubleNum = 0;
    }
    
    public void DialogueContinue()
    {
 
        if(!_characterList.CharactersList[CurrentDialog.CurrentInterlocutor]._isAnimationPlaing)
        {
            if(_infoPanel.NewCard)
            {
                _infoPanel.GetNewCard();
            }
            else
            {
                CurrentDialog.DialogueProcess();
            }
        }
    }
 

    private void Update()
    {
        if(gameObject.activeSelf)
        {
            _dialogues.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, DialogPosition.transform.position);

        }
    }

    private void FixedUpdate()
    {
        if(gameObject.activeSelf && !_inputController.KeyboardControl)
        {

            //если мышь попадает в персонажа, участника диалога, выделить диалог
            var raycasts = Utils.Raycast();
            bool deselect = true;

            var characterNames = raycasts.Select(r => r.gameObject.GetComponent<Character>()).Where(n => n != null).ToArray();

            if(characterNames.Length > 0 && CurrentDialog.CurrentDialogueCharacters.Contains(characterNames.FirstOrDefault().CharacterName))
            {
                deselect = false;
                SelectFirstBuble();
            }
            var dialog = raycasts.Select(r => r.gameObject.GetComponent<Dialogue>()).Where(n => n != null).FirstOrDefault();
            if(dialog)
            {
                deselect = false;
                dialog.Select();
            }

            if(deselect)
            {
             
                DeselectAll();
            }
        }

    }
    public void DeselectAll()
    {
        _selectedBubleNum = 0;
        foreach(var item in _dialogues.GetComponentsInChildren<Dialogue>())
        {
            item.Deselect();            
        }
        
    }
    public void SelectFirstBuble()
    {
       // if(_inputController.KeyboardControl)
        {
            _selectedBubleNum = 0;
            _dialogues.GetChild(_selectedBubleNum).GetComponent<Dialogue>().Select();

        }
        //else
        //{
        //    //DeselectAll();
        //    foreach(var item in _dialogues.GetComponentsInChildren<Dialogue>())
        //    {
        //        item.RayCast();
        //    }
        //    //если мышь попадает в персонажа, участника диалога, выделить диалог
        //    if(Character._selectedCharacter != null)
        //    {
        //        _selectedBubleNum = 0;
        //        _dialogues.GetChild(0).GetComponent<InteractiveGUIElement>().Select();
        //    }
        //}

    }


    public void Up()
    {
        if(_selectedBubleNum > _dialogues.childCount -1 ) _selectedBubleNum = 0;
        if(_selectedBubleNum < 0) _selectedBubleNum = _dialogues.childCount - 1;
        if(gameObject.activeSelf)
        {
            if(InteractiveGUIElement._selectedElement == null)
            {
                _selectedBubleNum = _dialogues.childCount - 1;
            }
            else
            {
                _dialogues.GetChild(_selectedBubleNum).GetComponent<Dialogue>().Deselect();

                _selectedBubleNum--;
                if(_selectedBubleNum < 0) _selectedBubleNum = _dialogues.childCount - 1;
            }
            _dialogues.GetChild(_selectedBubleNum).GetComponent<Dialogue>().Select();

        }
       
    }
    public void Down()
    {
        if(gameObject.activeSelf)
        {
                if(_selectedBubleNum > _dialogues.childCount - 1) _selectedBubleNum = 0;
                if(_selectedBubleNum < 0) _selectedBubleNum = _dialogues.childCount - 1;

                if(Dialogue._selectedElement == null)
            {
                _selectedBubleNum = 0;
            }
            else
            {
                _dialogues.GetChild(_selectedBubleNum).GetComponent<Dialogue>().Deselect();

                _selectedBubleNum++;
                if(_selectedBubleNum > _dialogues.childCount - 1) _selectedBubleNum = 0;
            }
                _dialogues.GetChild(_selectedBubleNum).GetComponent<Dialogue>().Select();
        }
    }


}
