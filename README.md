# Unity Dialogue System TMP

Unity Dialogue System using Text Mesh Pro (TMP) for

- Produce quick and easy dialogues
- Produce text with effects and colors

https://user-images.githubusercontent.com/72511237/211632477-5a0d6d56-f0d7-4651-ab35-66455ba4236f.mp4


## Usage

### Dialogue Scriptable Objects

```bash
Crate\ScriptableObjects\UIDialogueScriptableObject
```

![dialogueSO](https://user-images.githubusercontent.com/72511237/211653643-b34e0b7a-bd8d-4e79-b3d8-778a3a8edab2.PNG)

You need to write 
- Sentences
- Character counts for who belongs to which sentences ( starts with 0 !! )
- Dialogue Default Values ( Dialogue Default List's lengths need to be equal character count's max index)
What Dialogue Default Values include:
    - Text Write Speeds
    - Text Audios
    - Text Effects
    - Text Colors


![dialogueSpecificSO](https://user-images.githubusercontent.com/72511237/211653686-131c880c-dfed-4471-b485-ba2069a18744.PNG)

And there is another part for specific values for dialogues (working like dictionaries)
Select specific sentences index and the value you want=




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

**ex:** "Who" -> TMP_Sprite_Asset

https://user-images.githubusercontent.com/72511237/211649467-77d3e549-4dc4-48af-ae9c-420552f30c5f.mp4


Use **TextEffectController** for custom text effects. I already made a few effects like
- Vibration
- Glitch
...

Feel free to add more :)

## Usage Examples


https://user-images.githubusercontent.com/72511237/211654815-e4dd91c1-9b3f-46a8-b4b7-a01da9f9c762.mp4


https://user-images.githubusercontent.com/72511237/211654849-2f320af6-c40f-437f-8c1d-4babde3dfd59.mp4

For our games :P https://hinkirmunkur.itch.io/ 

## References


For text effect and color ideas. They have been a very useful resource for me.

https://www.youtube.com/watch?v=FgWVW2PL1bQ&list=LL&index=2

https://www.youtube.com/watch?v=FgWVW2PL1bQ&list=LL&index=2

I use generic Singleton class from GitHub that does not belong to me
When I got the code, I forgot to save the repo, if the person who belongs to it can reach me, I can add it too :)


## Licanse


[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)


