using System.Collections;
using UnityEngine;
using System;
using System.Text;
using Unity.VisualScripting;

namespace Munkur
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [Space(5)]
        [SerializeField] private DialogueHolder[] dialogueHolders;

        private DialogueHolder activeDialogueHolder = null;
        public DialogueHolder ActiveDialogueHolder => activeDialogueHolder;

        [SerializeField] private float fastWriteSpeed = 0.04f;

        [Space(15)]
        [SerializeField] private bool isGeneralDialogueColorEnabled = false;
        [SerializeField] private bool isGeneralDialogueImageEnabled = false;
        [SerializeField] private bool isGeneralDialogueEffectEnabled = false;
        
        private bool isCoroutineEnd = true;
        private bool fastWrite = false;

        private int dialogueIndex = 0;
        private int maxSize;

        private string word = String.Empty;
        private int wordCounter = 0;

        public Action<Dialogue> OnStartDialogueActions;
        public Func<RealDialogue, int, RealDialogue> OnCustomDialogueActions;
        public Action OnOneDialogueEndActions;
        public Action OnEndDialogueActions;

        private bool isDialogueStarted = false;
        public bool IsDialogueStarted => isDialogueStarted;

        public bool DialogueStopGame { get; set; } = false;

        private StringBuilder dialogueStringBuilder = new StringBuilder();
        private StringBuilder wordStringBuilder = new StringBuilder();

        private WaitForSecondsRealtime waitForSecondDialogue;
        private WaitForSecondsRealtime waitForSecondFastDialogue;

        private const char SPACE_CHAR = ' ';


        private void Start()
        {
            waitForSecondFastDialogue = new WaitForSecondsRealtime(fastWriteSpeed);
        }

        /// <summary>
        ///   <para> Starting to given dialogue </para>
        /// </summary>
        /// <param name="dialogue"> Dialogue Scriptable Object to be started </param>
        /// <param name="activeTextIndexInScene"> Dialogue Holder (in Dialogue Manager) to be affected </param>
        public void StartDialogue(Dialogue dialogue, int activeTextIndexInScene)
        {
            isDialogueStarted = true;
            maxSize = dialogue.sentences.Length;
            
            SetActiveTextInScene(activeTextIndexInScene);

            OnStartDialogueActions?.Invoke(dialogue);
            StartDialogueCustomActions();

            DisplayNextSentence();
        }
        
        /// <summary>
        ///   <para> Processing given dialogue.
        ///             Each DisplayNextSentence call write one sentence </para>
        /// </summary>
        public void DisplayNextSentence()
        {
            if (isCoroutineEnd == false)
            {
                fastWrite = true;
                return;
            }

            if (dialogueIndex == maxSize)
            {
                EndDialogue();
                return; 
            }

            RealDialogue realDialogue =  OnCustomDialogueActions?.Invoke(null, dialogueIndex);

            if (realDialogue == null)
            {
                Debug.LogError("REAL DIALOGUE VALUE NULL");
            }

            if (realDialogue != null)
            {
                activeDialogueHolder.soundEffect.clip = realDialogue.textAudios[dialogueIndex];
            }

            if (isGeneralDialogueColorEnabled)
            {
                GeneralDialogueColorController.Instance.ClearWordColorList(activeDialogueHolder.dialogueHolderText);
            }

            if (isGeneralDialogueImageEnabled)
            {
                GeneralDialogueImageController.Instance.ClearImages(activeDialogueHolder.dialogueHolderText);
            }

            if (isGeneralDialogueEffectEnabled)
            {
                GeneralDialogueEffectController.Instance.ClearWordEffectList(activeDialogueHolder.dialogueHolderText);
            }
            
            StopAllCoroutines();
            StartCoroutine(TypeSentence(realDialogue));
        }
        
        /// <summary>
        ///   <para> Writes proper sentence char by char </para>
        /// </summary>
        /// <param name="realDialogue"> realDialogue is an Object which is represent a Dialogue </param>
        IEnumerator TypeSentence(RealDialogue realDialogue)
        {
            if (!realDialogue.overWrite[dialogueIndex])
            {
                activeDialogueHolder.dialogueHolderText.text = String.Empty;
            }

            isCoroutineEnd = false;
    
            if (activeDialogueHolder.soundEffect.loop)
            {
                activeDialogueHolder.soundEffect.Play();
            }

            waitForSecondDialogue = new WaitForSecondsRealtime(realDialogue.textWriteSpeeds[dialogueIndex]);

            dialogueStringBuilder.Clear();
            wordStringBuilder.Clear();
            
            //word = String.Empty;
            wordCounter = 0;
            
            foreach (char letter in realDialogue.sentences[dialogueIndex])
            {
                dialogueStringBuilder.Append(letter);
                activeDialogueHolder.dialogueHolderText.SetText(dialogueStringBuilder);

                if (letter != SPACE_CHAR)
                {
                    //word += letter;
                    wordStringBuilder.Append(letter);
                    
                    if (activeDialogueHolder.soundEffect.loop == false)
                    {
                        activeDialogueHolder.soundEffect.Play();
                    }

                    if (fastWrite)
                    {
                        yield return waitForSecondFastDialogue;
                    }
                    else
                    {
                        yield return waitForSecondDialogue;
                    }
                }
                else
                {
                    if (isGeneralDialogueColorEnabled)
                    {
                        GeneralDialogueColorController.Instance.TryToAddColorToWord(wordStringBuilder.ToString(), wordCounter);
                    }

                    if (isGeneralDialogueEffectEnabled)
                    {
                        GeneralDialogueEffectController.Instance.TryToAddEffectToWord(wordStringBuilder.ToString(), wordCounter);
                    }

                    if (isGeneralDialogueImageEnabled)
                    {
                        if (GeneralDialogueImageController.Instance.TryToAddImage(wordStringBuilder.ToString()))
                        {
                            dialogueStringBuilder.Append(
                                GeneralDialogueImageController.Instance.AddImageAfterWord(
                                    activeDialogueHolder.dialogueHolderText, wordStringBuilder.ToString(), wordCounter));
                            activeDialogueHolder.dialogueHolderText.SetText(dialogueStringBuilder);
                        }   
                    }

                    wordCounter++;
                    //word = String.Empty;
                    wordStringBuilder.Clear();
                }

            }
            
            // Check the last word
            //if (word != String.Empty)
            if (wordStringBuilder.ToString() != String.Empty)
            {
                if (isGeneralDialogueColorEnabled)
                {
                    GeneralDialogueColorController.Instance.TryToAddColorToWord(wordStringBuilder.ToString(), wordCounter);
                }

                if (isGeneralDialogueEffectEnabled)
                {
                    GeneralDialogueEffectController.Instance.TryToAddEffectToWord(wordStringBuilder.ToString(), wordCounter);
                }

                if (isGeneralDialogueImageEnabled)
                {
                    if (GeneralDialogueImageController.Instance.TryToAddImage(wordStringBuilder.ToString()))
                    {
                        dialogueStringBuilder.Append(
                            GeneralDialogueImageController.Instance.AddImageAfterWord(
                                activeDialogueHolder.dialogueHolderText, wordStringBuilder.ToString(), wordCounter));
                        activeDialogueHolder.dialogueHolderText.SetText(dialogueStringBuilder);
                    }    
                }

                wordCounter++;
                //word = String.Empty;
                wordStringBuilder.Clear();
            }

            dialogueIndex++;
            fastWrite = false;
            isCoroutineEnd = true;
            EndOneDialogueCustomActions();
            OnOneDialogueEndActions?.Invoke();
        }
        
        /// <summary>
        ///   <para> Writes proper sentence char by char </para>
        /// </summary>
        /// <param name="realDialogue"> realDialogue is an Object which is represent a Dialogue </param>
        private void EndDialogue()
        {
            isDialogueStarted = false;
            dialogueIndex = 0;

            OnEndDialogueActions?.Invoke();
            EndDialogueCustomActions();

        }
        
        /// <summary>
        ///   <para> Activates suitable dialogueHolder using index </para>
        /// </summary>
        /// <param name="index"> index which is represent suitable dialogueHolder in --dialogueHolders-- list  </param>
        private void SetActiveTextInScene(int index)
        {
            if (activeDialogueHolder != null)
            {
                activeDialogueHolder.UnSubsActions();
            }
            
            activeDialogueHolder = dialogueHolders[index];
            activeDialogueHolder.SubsActions();
        }
        
        /// <summary>
        ///   <para> Skips the current dialogue </para>
        /// </summary>
        public void SkipDialogue()
        {
            isCoroutineEnd = true;
            StopAllCoroutines();
            EndDialogue();
        }

        private void StartDialogueCustomActions() { }

        private void EndDialogueCustomActions() { }

        private void EndOneDialogueCustomActions() { }
        
    }
}