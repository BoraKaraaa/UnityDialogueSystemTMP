# Unity Dialogue System TMP

Unity Dialogue System using Text Mesh Pro (TMP) for

- Produce quick and easy dialogues
- Produce text with effects and colors

https://user-images.githubusercontent.com/72511237/211632477-5a0d6d56-f0d7-4651-ab35-66455ba4236f.mp4


## Usage

Crate DialogueSystem Prefab from
```bash
Assets\Prefabs\DialogueSystem
```

![DialogueSystemPrefab](https://user-images.githubusercontent.com/72511237/211633753-18a8d19b-7468-462a-abaf-38b1e259137a.PNG)

### Dialogue Manager

![DialogueManagerR](https://user-images.githubusercontent.com/72511237/211634812-3e5d5b8e-b423-49e3-9e63-c789a117f531.PNG)

Dialogue Holder is a Text Area UI that holds sentences
You can crate any amount of Dialogue Holder for dialogue in different places

Set Dialogue Holders References in any order (this indices will be important DialogueTrigger Script) in Dialogue Manager



For **fast write** just call trigger dialogue when one dialogue was running


https://user-images.githubusercontent.com/72511237/211646481-b7fa5b97-a34e-4e3d-9341-d65022f0c013.mp4





### Dialogue Trigger
![DialogueTrigger](https://user-images.githubusercontent.com/72511237/211634918-9666d56d-0444-45ec-9d6b-f1c69b78be2f.PNG)

Create Dialogue Scriptable Object for your custom dialogues and assign Dialogue List in order
If you want add custom dialogue at runtime call SetDialogue() func

```C#
    /// <summary>
    ///   <para> Sets the given dialogue order in --dialogueList-- </para>
    /// </summary>
    /// <param name="newDialogue"> Dialogue Scriptable Object to be added </param>
    /// <param name="effectDialogueHolderIndex"> Dialogue Holder (in Dialogue Manager) to be affected </param>
    /// <param name="addDialogueIndex"> Order in --dialogueList-- </param>

    public void SetDialogue(Dialogue newDialogue, int effectDialogueHolderIndex, int addDialogueIndex = -1)
    {
        if (addDialogueIndex == -1)
        {
            dialogueList.Insert(dialogueList.Count, newDialogue);
        }
        else
        {
            dialogueList.Insert(addDialogueIndex, newDialogue);
        }
        
        targetDialogueHolderIndex.Add(effectDialogueHolderIndex);
    }
```

Target Dialogue Holder index represent which Dialogue Holder you want to use


### GeneralControllers

![generalControllers](https://user-images.githubusercontent.com/72511237/211647556-ff01b9b1-db9b-4ec1-b487-3c86b100417b.PNG)

Each general controllers contains dictionary for
- word -> image
- word -> color
- word -> effect mapping

Whole game when specific "word" mentioned custom image, color or effect applied

ex: word -> TMP_Sprite_Asset
https://user-images.githubusercontent.com/72511237/211649149-daf030f2-0cc9-4792-bfce-4b17e49c0438.mp4




## Licanse 





