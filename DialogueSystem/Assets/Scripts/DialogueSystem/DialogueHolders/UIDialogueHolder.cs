using UnityEngine;
using UnityEngine.UI;

public class UIDialogueHolder : DialogueHolder
{
    [Space(10)]
    [Header("UIDialogueHolder Parameters")]
    [SerializeField] private Image _dialogueHolderImage; 
    [SerializeField] private Animator _dialogueAnimator;

    private RealUIDialogue _realUIDialogue = null;
    private UIDialogue _uiDialogue = null;

    protected override void OnStartDialogueActions(Dialogue dialogue)
    {
        _realUIDialogue = new RealUIDialogue();
        
        _realUIDialogue.Init(dialogue);
        
        _uiDialogue = dialogue as UIDialogue;
        
        base.OnStartDialogueActions(dialogue);
    }

    protected override void SetDefaultValues(Dialogue uiDialogue, RealDialogue realUIDialogue, int index)
    {
        base.SetDefaultValues(_uiDialogue, _realUIDialogue, index);
        
        _realUIDialogue.SetCustomAnimatorStateName(index, 
            _uiDialogue.defAnimatorStateNames[_uiDialogue.characterCounts[index]]);
    }

    protected override void ControlCustomValues(Dialogue uiDialogue, RealDialogue realUIDialogue, int index)
    {
        base.ControlCustomValues(_uiDialogue, _realUIDialogue, index);
        
        if (index < _uiDialogue.animatorStateNames.Count)
        {
            _realUIDialogue.SetCustomAnimatorStateName(_uiDialogue.animatorStateNames[index].id, 
                _uiDialogue.animatorStateNames[index].animationStateName);
        }
    }

    protected override RealDialogue OnCustomDialogueActions(RealDialogue realUIDialogue, int index)
    {
        SetDefaultValues(_uiDialogue, _realUIDialogue, index);
        ControlCustomValues(_uiDialogue, _realUIDialogue, index);
        
        HolderOnCustomDialogueActions?.Invoke(_realUIDialogue, index); 
        
        base.OnCustomDialogueActions(_realUIDialogue, index);
        
        _dialogueHolderImage.sprite = _uiDialogue.sprites[_uiDialogue.characterCounts[index]];
        _dialogueAnimator.runtimeAnimatorController = _uiDialogue.animators[_uiDialogue.characterCounts[index]];

        _dialogueAnimator.Play(_realUIDialogue.animatorStateNames[index]);

        return _realUIDialogue;
    }

    protected override void OnOneDialogueEndActions()
    {
        _dialogueAnimator.Play("NotTalking");
        HolderOnOneDialogueEndActions?.Invoke();
    }

    protected override void OnEndDialogueActions()
    {
        base.OnEndDialogueActions();
    }

}
