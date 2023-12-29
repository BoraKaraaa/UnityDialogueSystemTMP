using UnityEngine;

public class UIBasicDialogueHolder : DialogueHolder
{
    private RealUIBasicDialogue _realUIBasicDialogue = null;
    private UIBasicDialogue _uiBasicDialogue = null;

    protected override void OnStartDialogueActions(Dialogue dialogue)
    {
        _realUIBasicDialogue = new RealUIBasicDialogue();
        
        _realUIBasicDialogue.Init(dialogue);
        
        _uiBasicDialogue = dialogue as UIBasicDialogue;
        
        base.OnStartDialogueActions(dialogue);
    }

    protected override RealDialogue OnCustomDialogueActions(RealDialogue realUIBasicDialogue, int index)
    {
        SetDefaultValues(_uiBasicDialogue, _realUIBasicDialogue, index);
        ControlCustomValues(_uiBasicDialogue, _realUIBasicDialogue, index);
        
        HolderOnCustomDialogueActions?.Invoke(_realUIBasicDialogue, index);

        base.OnCustomDialogueActions(_realUIBasicDialogue, index);

        return _realUIBasicDialogue;
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
