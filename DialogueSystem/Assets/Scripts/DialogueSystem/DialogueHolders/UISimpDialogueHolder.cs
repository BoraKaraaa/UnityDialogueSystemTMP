using UnityEngine;

public class UISimpDialogueHolder : DialogueHolder
{
    [Space(10)]
    [Header("UISimpDialogueHolder Parameters")]
    [SerializeField] private Animator _dialogueAnimator;

    private RealUISimpDialogue _realUISimpDialogue = null;
    private UISimpDialogue _uiSimpDialogue = null;
    
    
    protected override void OnStartDialogueActions(Dialogue dialogue)
    {
        _realUISimpDialogue = new RealUISimpDialogue();
        
        _realUISimpDialogue.Init(dialogue);
        
        _uiSimpDialogue = dialogue as UISimpDialogue;
        
        base.OnStartDialogueActions(dialogue);
    }
    
    protected override void SetDefaultValues(Dialogue uiSimpDialogue, RealDialogue realUISimpDialogue, int index)
    {
        base.SetDefaultValues(_uiSimpDialogue, _realUISimpDialogue, index);
        
        _realUISimpDialogue.SetCustomAnimatorStateName(index, 
            _uiSimpDialogue.defAnimatorStateNames[_uiSimpDialogue.characterCounts[index]]);
    }

    protected override void ControlCustomValues(Dialogue uiSimpDialogue, RealDialogue realUISimpDialogue, int index)
    {
        base.ControlCustomValues(_uiSimpDialogue, _realUISimpDialogue, index);
        
        if (index < _uiSimpDialogue.animatorStateNames.Count)  
            _realUISimpDialogue.SetCustomAnimatorStateName(_uiSimpDialogue.animatorStateNames[index].id, 
                _uiSimpDialogue.animatorStateNames[index].animationStateName);
    }

    protected override RealDialogue OnCustomDialogueActions(RealDialogue realUISimpDialogue, int index)
    {
        SetDefaultValues(_uiSimpDialogue, _realUISimpDialogue, index);
        ControlCustomValues(_uiSimpDialogue, _realUISimpDialogue, index);
        
        HolderOnCustomDialogueActions?.Invoke(_realUISimpDialogue, index);

        base.OnCustomDialogueActions(_realUISimpDialogue, index);

        _dialogueAnimator.runtimeAnimatorController = _uiSimpDialogue.animators[_uiSimpDialogue.characterCounts[index]];
        _dialogueAnimator.Play(_realUISimpDialogue.animatorStateNames[index]);

        return _realUISimpDialogue;
    }

    protected override void OnOneDialogueEndActions()
    {
        HolderOnOneDialogueEndActions?.Invoke();
    }

    protected override void OnEndDialogueActions()
    {
        base.OnEndDialogueActions();
    }

}
