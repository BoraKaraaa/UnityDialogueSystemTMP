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


### Dialogue Trigger
![DialogueTrigger](https://user-images.githubusercontent.com/72511237/211634918-9666d56d-0444-45ec-9d6b-f1c69b78be2f.PNG)

Create Dialogue Scriptable Object for your custom dialogues and assign Dialogue List in order
If you want add custom dialogue at runtime  

```C#
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

Dialogue Holder is a Text Area that hold dialogues 
You can crate any amount of Dialogue Holder

## Licanse 





