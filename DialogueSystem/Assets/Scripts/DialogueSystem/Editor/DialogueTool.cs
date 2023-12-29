using Object = UnityEngine.Object;
using System.Collections.Generic;
using UnityEditor.Rendering;
using System.Web.WebPages;
using UnityEditor;
using UnityEngine;
using System;

/* FEATURES
 *
 *  ONCELIKLE REFACTOR ILE BASLA !!!!!!!!
 * 
 * 1_ SO olusturulurken verilen pathe bak ayni isimli bir SO varsa hata ver
 * 2_ UISimpDialogue ve UIDialogue icin uyumlu yap
 * 3_ Preferences dataya kaydedilmeyenleri editlenemez sekilde updatele
 * 4_ Word Specific Effect ve Color vermeyi yap
 */

public class DialogueTool : EditorWindow
{
    private const float WINDOW_MIN_WIDTH = 400;
    private const float WINDOW_MIN_HEIGHT = 400;

    public Font font;
    public int headerFontSize;
    public int textFontSize;

    public int characterAmount;
    public int totalTextAmount;
    public string characterName;
    
    private const int MIN_FONT_SIZE = 10;
    private const int MAX_FONT_SIZE = 20;
    
    private GUIStyle headerGUIStyle;
    private GUIStyle textGUIStyle;
    private GUIStyle buttonGUIStyle;
    private GUIStyle textAreaGUIStyle;
    private GUIStyle characterNameTextArea;

    private Stack<Action> tabStack = new Stack<Action>();
    
    private SerializedObject preferenceSO;
    private SerializedObject dialogueCrateMenuSO;
    private SerializedObject dialogueSentenceSO;
    private SerializedObject dialogueSOnameSO;

    private SerializedProperty characterAmountSP;
    private SerializedProperty dialgueCharacterNamesListSP;
    private SerializedProperty dialgueTextListSP;
    private SerializedProperty dialogueTextCharacterIndexListSP;
    
    private SerializedProperty dialogueSOname;

    private List<GUIStyle> _allGUIStyles;
    
    public List<string> _characterNames;
    public string[] _characterNameEnum;

    public string[] _characterTextKeys =
    {
        "_characterTextWriteSpeeds", "_textEffects", "_textWriteSounds", "_textColors",
        "_textOverwrites"
    };
    
    public List<float> _characterTextWriteSpeeds;
    public List<ETextEffects> _textEffects;
    public List<AudioClip> _textWriteSounds;
    public List<Color> _textColors;
    public List<bool> _textOverwrites;
    
    public string[] _characterCustomTextKeys =
    {
        "_customCharacterTextWriteSpeeds", "_customTextEffects", "_customTextWriteSounds", "_customTextColors",
        "_customTextOverwrites"
    };
    
    public List<float> _customCharacterTextWriteSpeeds;
    public List<ETextEffects> _customTextEffects;
    public List<AudioClip> _customTextWriteSounds;
    public List<Color> _customTextColors;
    public List<bool> _customTextOverwrites;
    
    
    public string[] _characterDefaultTextKeys =
    {
        "_defCharacterTextWriteSpeeds", "_defTextEffects", "_defTextWriteSounds", "_defTextColors",
        "_defTextOverwrites"
    };

    public float[] _defCharacterTextWriteSpeeds = new float[1];
    public ETextEffects[] _defTextEffects = new ETextEffects[1];
    public AudioSource[] _defTextWriteSounds = new AudioSource[1];
    public Color[] _defTextColors = new Color[1];
    public bool[] _defTextOverwrites = new bool[1];
    
    
    public List<string> _dialogueTexts;
    public List<int> selectedCharacterNameIndices;

    public string _dialogueSOname;
    public Dialogue _importDialogue;
    
    private bool showCharacterInfos = false;
    private bool showDialogueTexts = false;

    private DefaultAsset SOfolder;
    
    private Vector2 scrollPosition = Vector2.zero;
    
    private string[] dialogueTypeNames = { "UIBasicDialogue", "UISimpDialogue", "UIDialogue" };
    private Dialogue[] dialogueTypes;

    private UIBasicDialogue uiBasicDialogue;
    private UISimpDialogue uiSimpDialogue;
    private UIDialogue uiDialogue;
    
    private int selectedOption = 0;

    private const string _initialDataPath = "Assets/ScriptableObjects/DialogueSO";
    
    private const string CUSTOM_FONT_KEY = "CUSTOM_FONT_KEY";
    private const string CUSTOM_HEADER_FONT_SIZE = "CUSTOM_HEADER_FONT_SIZE";
    private const string CUSTOM_TEXT_FONT_SIZE = "CUSTOM_TEXT_FONT_SIZE";
    
    private const string SO_FOLDER_PATH_KEY = "SO_FOLDER_PATH_KEY";

    private const string SO_WRITE_SPEED_DEF_VAL = "SO_WRITE_SPEED_DEF_VAL";
    private const string SO_TEXT_EFFECT_DEF_VAL = "SO_TEXT_EFFECT_DEF_VAL";
    private const string SO_TEXT_OVERWRITES_DEF_VAL = "SO_TEXT_OVERWRITES_DEF_VAL";
    
    

    [MenuItem("Tools/DialogueSystem %&d")]
    public static void OpenDialogueTool()
    {
        DialogueTool dialogueTool = GetWindow<DialogueTool>(false, "DialogueCreator");
        dialogueTool.minSize = new Vector2(WINDOW_MIN_WIDTH, WINDOW_MIN_HEIGHT);
        dialogueTool.Show();
    }

    private void OnEnable()
    {
        headerGUIStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            font = font,
            fontSize = 15,
        };

        textGUIStyle = new GUIStyle(EditorStyles.label)
        {
            font = font,
            fontSize = 12,
        };

        buttonGUIStyle = new GUIStyle(EditorStyles.miniButton)
        {
            alignment = TextAnchor.MiddleCenter,
            font = font,
            margin = new RectOffset(0, 0, 10,10),
            fixedHeight = 20,
        };

        textAreaGUIStyle = new GUIStyle(EditorStyles.textArea)
        {
            stretchHeight = true,
            font = font,
        };

        characterNameTextArea = new GUIStyle(EditorStyles.textField)
        {
            font = font,
        };

        _allGUIStyles = new List<GUIStyle>();
        _allGUIStyles.Add(headerGUIStyle);
        _allGUIStyles.Add(textGUIStyle);
        _allGUIStyles.Add(buttonGUIStyle);
        _allGUIStyles.Add(textAreaGUIStyle);

        LoadToolData();
        
        tabStack.Clear();
        tabStack.Push(MenuTab);

        preferenceSO = new SerializedObject(this);
        dialogueCrateMenuSO = new SerializedObject(this);
        dialogueSentenceSO = new SerializedObject(this);
        dialogueSOnameSO = new SerializedObject(this);

        uiBasicDialogue = ScriptableObject.CreateInstance<UIBasicDialogue>();
        uiSimpDialogue = ScriptableObject.CreateInstance<UISimpDialogue>();
        uiDialogue = ScriptableObject.CreateInstance<UIDialogue>();
        
        dialogueTypes = new Dialogue[3];
        dialogueTypes[0] = uiBasicDialogue;
        dialogueTypes[1] = uiSimpDialogue;
        dialogueTypes[2] = uiDialogue;
    }

    private void OnDisable()
    {
        tabStack.Clear();
    }

    private void OnGUI()
    {
        if (tabStack.Count > 0)
        {
            tabStack.Peek().Invoke();
        }
    }

    private void LoadToolData()
    {
        _defTextColors[0] = Color.white;
        
        font = EditorPrefs.HasKey(CUSTOM_FONT_KEY) ? AssetDatabase.LoadAssetAtPath<Font>($"Assets/Fonts/{EditorPrefs.GetString(CUSTOM_FONT_KEY)}.ttf")
            : AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/kongtext.ttf");

        headerFontSize = EditorPrefs.HasKey(CUSTOM_HEADER_FONT_SIZE) ? EditorPrefs.GetInt(CUSTOM_HEADER_FONT_SIZE)
            : headerGUIStyle.fontSize;
        
        textFontSize = EditorPrefs.HasKey(CUSTOM_TEXT_FONT_SIZE) ? EditorPrefs.GetInt(CUSTOM_TEXT_FONT_SIZE) 
            : textGUIStyle.fontSize;
        
        SOfolder = EditorPrefs.HasKey(SO_FOLDER_PATH_KEY) ? 
            AssetDatabase.LoadAssetAtPath<DefaultAsset>(EditorPrefs.GetString(SO_FOLDER_PATH_KEY))
            : AssetDatabase.LoadAssetAtPath<DefaultAsset>(_initialDataPath);

        _defCharacterTextWriteSpeeds[0] = EditorPrefs.HasKey(SO_WRITE_SPEED_DEF_VAL)
            ? EditorPrefs.GetFloat(SO_WRITE_SPEED_DEF_VAL)
            : 0;

        _defTextEffects[0] = EditorPrefs.HasKey(SO_TEXT_EFFECT_DEF_VAL)
            ? (ETextEffects)EditorPrefs.GetInt(SO_TEXT_EFFECT_DEF_VAL)
            : ETextEffects.None;

        _defTextOverwrites[0] = EditorPrefs.HasKey(SO_TEXT_OVERWRITES_DEF_VAL) && EditorPrefs.GetBool(SO_TEXT_OVERWRITES_DEF_VAL);
    }

    private void MenuTab()
    {
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("DIALOGUE CREATOR", headerGUIStyle);
        EditorGUILayout.Space(20);

        GUILayout.BeginVertical("", "window");

        if (GUILayout.Button("Create Dialogue", buttonGUIStyle))
        {
            tabStack.Push(CreateDialogueTab);
        }
        
        if (GUILayout.Button("Create Text Effect", buttonGUIStyle))
        {
            tabStack.Push(CreateTextEffect);
        }
        
        if (GUILayout.Button("Preferences", buttonGUIStyle))
        {
            scrollPosition = Vector2.zero;
            tabStack.Push(PreferenceTab);
        }
        
        GUILayout.EndVertical();   
    }

    private void CreateDialogueTab()
    {
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Create Dialogue", headerGUIStyle);
        EditorGUILayout.Space(20);

        GUILayout.BeginVertical("", "window");

        headerGUIStyle.fontSize = 12;
        GUILayout.Label("Dialogue Types", headerGUIStyle);
        headerGUIStyle.fontSize = EditorPrefs.GetInt(CUSTOM_HEADER_FONT_SIZE);
        
        EditorGUILayout.Space(10);
        
        GUILayout.BeginVertical("", "window");
        
        EditorGUILayout.Space(5);
        selectedOption = GUILayout.SelectionGrid(selectedOption, dialogueTypeNames, 3, buttonGUIStyle);
        EditorGUILayout.Space(5);
        
        EditorGUILayout.BeginHorizontal();
        textGUIStyle.fontStyle = FontStyle.Bold;
        GUILayout.Label("Selected Dialogue Type: ", textGUIStyle, GUILayout.Width(200));
        textGUIStyle.fontStyle = FontStyle.Normal;
        
        GUILayout.Label(dialogueTypeNames[selectedOption], textGUIStyle, GUILayout.Width(150));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        textGUIStyle.fontStyle = FontStyle.Bold;
        GUILayout.Label("USAGE", textGUIStyle);
        textGUIStyle.fontStyle = FontStyle.Normal;
        
        EditorGUILayout.Space(5);
        GUILayout.Label("- Creating Texts", textGUIStyle);
        GUILayout.Label("- Text Effects & Color", textGUIStyle);
        GUILayout.Label("- Text Audios", textGUIStyle);

        switch (selectedOption)
        {
            case 0:
                break;
            case 1:
                GUILayout.Label("- Animations", textGUIStyle);
                break;
            case 2:
                GUILayout.Label("- Animations", textGUIStyle);
                GUILayout.Label("- Sprites", textGUIStyle);
                break;
        }

        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button("Continue with " + dialogueTypeNames[selectedOption], buttonGUIStyle))
        {
            scrollPosition = Vector2.zero;
            tabStack.Push(CustomDialogueSO);
        }
        
        GUILayout.EndVertical();  
        
        CreateBackButton();
        
        GUILayout.EndVertical();   
    }

    private void CreateTextEffect()
    {
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Create Text Effect", headerGUIStyle);
        EditorGUILayout.Space(20);

        GUILayout.BeginVertical("", "window");
        
        GUILayout.Label("COMING SOON", headerGUIStyle);

        CreateBackButton();
        
        GUILayout.EndVertical();  
    }
    
    private void PreferenceTab()
    {
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Preferences", headerGUIStyle);
        EditorGUILayout.Space(15);

        GUILayout.BeginVertical("window");
        
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.BeginVertical("box");
        
        GUILayout.BeginHorizontal();
        
        GUILayout.Label("Select a Folder To Create SO:", GUILayout.Width(200));
        SOfolder = EditorGUILayout.ObjectField(SOfolder, typeof(DefaultAsset), false) as DefaultAsset;

        if (SOfolder != null)
        {
            EditorPrefs.SetString(SO_FOLDER_PATH_KEY, AssetDatabase.GetAssetPath(SOfolder));
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        
        EditorGUILayout.Space(10);
        
        GUILayout.BeginVertical("box");
        
        textGUIStyle.alignment = TextAnchor.MiddleCenter;
        textGUIStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Default SO Values", textGUIStyle);
        textGUIStyle.fontStyle = FontStyle.Normal;
        textGUIStyle.alignment = TextAnchor.MiddleLeft;
        
        EditorGUILayout.Space(5);

        GUILayout.BeginHorizontal();
        
        GUILayout.BeginVertical();
        DialogueTextCustomValueHeaders();
        GUILayout.EndVertical();
        
        GUILayout.BeginVertical();
        DialogueTextCustomValues(0, _characterDefaultTextKeys, 200, true);
        GUILayout.EndVertical();
        
        GUILayout.EndHorizontal();
        
        GUILayout.EndVertical();
        
        EditorGUILayout.Space(10);
        
        GUILayout.BeginVertical("box");
        
        textGUIStyle.alignment = TextAnchor.MiddleCenter;
        textGUIStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("General Text Settings", textGUIStyle);
        textGUIStyle.fontStyle = FontStyle.Normal;
        textGUIStyle.alignment = TextAnchor.MiddleLeft;
        
        EditorGUILayout.Space(5);
        
        preferenceSO.Update();
        
        EditorGUILayout.PropertyField(preferenceSO.FindProperty("font"), true);
        
        int newHeaderFontSize = EditorGUILayout.IntSlider("Header Font Size", headerFontSize, 
            MIN_FONT_SIZE, MAX_FONT_SIZE);
        int newTextFontSize = EditorGUILayout.IntSlider("Text Font Size", textFontSize, 
            MIN_FONT_SIZE, MAX_FONT_SIZE);

        bool madeChange = newHeaderFontSize != headerFontSize || newTextFontSize != textFontSize;
        
        if (madeChange || preferenceSO.ApplyModifiedProperties())
        {
            headerFontSize = newHeaderFontSize;
            textFontSize = newTextFontSize;
            
            EditorPrefs.SetString(CUSTOM_FONT_KEY, font.name);
            EditorPrefs.SetInt(CUSTOM_HEADER_FONT_SIZE, headerFontSize);
            EditorPrefs.SetInt(CUSTOM_TEXT_FONT_SIZE, textFontSize);
               
            foreach (var allGUIStyle in _allGUIStyles)
            {
                allGUIStyle.font = font;
                
                if (allGUIStyle.name.Equals("BoldLabel"))
                {
                    allGUIStyle.fontSize = headerFontSize;
                }
                else
                {
                    allGUIStyle.fontSize = textFontSize;
                }
            }
            
            Repaint();
        }
        GUILayout.EndVertical();
        
        
        GUILayout.EndScrollView();

        if (SOfolder == null)
        {
            CreateEmptyBackButton("Folder Path Uninitialized");
        }
        else
        {
            CreateBackButton(LoadToolData);
        }
        
        GUILayout.EndVertical();   
    }

    private void CustomDialogueSO()
    {
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField($"{dialogueTypeNames[selectedOption]}", headerGUIStyle);
        EditorGUILayout.Space(20);
            
        GUILayout.BeginVertical("window");
        
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        
        dialogueCrateMenuSO.Update();
        GUILayout.BeginVertical("", "box");
        EditorGUILayout.PropertyField(dialogueCrateMenuSO.FindProperty("characterAmount"));
        EditorGUILayout.PropertyField(dialogueCrateMenuSO.FindProperty("totalTextAmount"));
        GUILayout.EndVertical();
        if (dialogueCrateMenuSO.ApplyModifiedProperties())
        {
            if (_characterNames != null)
            {
                int distance = _characterNames.Count - characterAmount;

                if (distance < 0)
                {
                    distance *= -1;

                    for (int i = 0; i < distance; i++)
                    {
                        _characterNames.Add($"_characterNames_{_characterNames.Count}");
                        _characterTextWriteSpeeds.Add(_defCharacterTextWriteSpeeds[0]);
                        _textEffects.Add(_defTextEffects[0]);
                        _textWriteSounds.Add(null);
                        _textColors.Add(_defTextColors[0]);
                        _textOverwrites.Add(_defTextOverwrites[0]);
                    }
                }
                else
                {
                    for (int i = 0; i < distance; i++)
                    {
                        _characterNames.RemoveAt(_characterNames.Count-1);
                        _characterTextWriteSpeeds.RemoveAt(_characterTextWriteSpeeds.Count-1);
                        _textEffects.RemoveAt(_textEffects.Count-1);
                        _textWriteSounds.RemoveAt(_textWriteSounds.Count-1);
                        _textColors.RemoveAt(_textColors.Count-1);
                        _textOverwrites.RemoveAt(_textOverwrites.Count-1);
                    }
                }
            }
            else
            {
                _characterNames = new List<string>(characterAmount);
                
                for (int i = 0; i < characterAmount; i++)
                {
                    _characterNames[i] = $"character_{i}";
                }

                _characterTextWriteSpeeds = new List<float>();
                
                _textEffects = new List<ETextEffects>(characterAmount);
                _textWriteSounds = new List<AudioClip>(characterAmount);
                _textColors = new List<Color>(characterAmount);
                _textOverwrites = new List<bool>(characterAmount);
            }
            
            _characterNameEnum = new string[characterAmount];
            
            if (_dialogueTexts != null)
            {
                int distance = _dialogueTexts.Count - totalTextAmount;

                if (distance < 0)
                {
                    distance *= -1;

                    for (int i = 0; i < distance; i++)
                    {
                        _dialogueTexts.Add("");
                        selectedCharacterNameIndices.Add(0);
                        _customCharacterTextWriteSpeeds.Add(_defCharacterTextWriteSpeeds[0]);
                        _customTextEffects.Add(_defTextEffects[0]);
                        _customTextWriteSounds.Add(null);
                        _customTextColors.Add(_defTextColors[0]);
                        _customTextOverwrites.Add(_defTextOverwrites[0]);
                    }
                }
                else
                {
                    for (int i = 0; i < distance; i++)
                    {
                        _dialogueTexts.RemoveAt(_dialogueTexts.Count-1);
                        selectedCharacterNameIndices.RemoveAt(selectedCharacterNameIndices.Count-1);
                        _customCharacterTextWriteSpeeds.RemoveAt(_customCharacterTextWriteSpeeds.Count-1);
                        _customTextEffects.RemoveAt(_customTextEffects.Count-1);
                        _customTextWriteSounds.RemoveAt(_customTextWriteSounds.Count-1);
                        _customTextColors.RemoveAt(_customTextColors.Count-1);
                        _customTextOverwrites.RemoveAt(_customTextOverwrites.Count-1);
                    }
                }
            }
            else
            {
                selectedCharacterNameIndices = new List<int>(totalTextAmount);
                _dialogueTexts = new List<string>(totalTextAmount);

                _customCharacterTextWriteSpeeds = new List<float>(totalTextAmount);
                _customTextEffects = new List<ETextEffects>(totalTextAmount);
                _customTextWriteSounds = new List<AudioClip>(totalTextAmount);
                _customTextColors = new List<Color>(totalTextAmount);
                _customTextOverwrites = new List<bool>(totalTextAmount);
            }
        }

        GUILayout.Space(5);

        dialogueSentenceSO.Update();

        if (characterAmount > 0)
        {
            showCharacterInfos = EditorGUILayout.Foldout(showCharacterInfos, "Show Characters Info");
        }

        if (showCharacterInfos)
        {
            GUILayout.BeginHorizontal("box");
            for (int i = 0; i < characterAmount; i++)
            {
                
                GUILayout.BeginVertical("box");
                if (i == 0)
                {
                    textGUIStyle.fontStyle = FontStyle.Bold;
                    textGUIStyle.stretchWidth = false;
                    textGUIStyle.fixedWidth = position.width / 5;
                    GUILayout.TextField("Character Names: ", textGUIStyle);
                    DialogueTextCustomValueHeaders();
                    textGUIStyle.stretchWidth = true;
                    textGUIStyle.fontStyle = FontStyle.Normal;
                }
                
                GUILayout.EndVertical();
                
                GUILayout.BeginVertical();
                dialgueCharacterNamesListSP = dialogueSentenceSO.FindProperty("_characterNames");
                dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).stringValue =
                    EditorGUILayout.TextArea(dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).stringValue,
                        characterNameTextArea, GUILayout.Width(200));

                _characterNameEnum[i] = dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).stringValue;


                DialogueTextCustomValues(i, _characterTextKeys, 200);
                
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }


        if (totalTextAmount > 0)
        {
            showDialogueTexts = EditorGUILayout.Foldout(showDialogueTexts, "Show Dialogue Texts");
        }

        if (showDialogueTexts)
        {
            GUILayout.BeginVertical("box");
            for (int i = 0; i < totalTextAmount; i++)
            {
                GUILayout.BeginHorizontal();
                dialogueTextCharacterIndexListSP = dialogueSentenceSO.FindProperty("selectedCharacterNameIndices");
                dialogueTextCharacterIndexListSP.GetArrayElementAtIndex(i).intValue = EditorGUILayout.Popup("Select Character",
                    selectedCharacterNameIndices[i], _characterNameEnum);
                
                textGUIStyle.fontStyle = FontStyle.Bold;

                if (i == 0)
                {
                    textGUIStyle.alignment = TextAnchor.MiddleCenter;
                    EditorGUILayout.LabelField("Custom Dialogue Text Values", textGUIStyle);
                    textGUIStyle.alignment = TextAnchor.MiddleLeft;    
                }
                else
                {
                    EditorGUILayout.LabelField("", textGUIStyle);
                }
                
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                dialgueTextListSP = dialogueSentenceSO.FindProperty("_dialogueTexts");
                dialgueTextListSP.GetArrayElementAtIndex(i).stringValue = EditorGUILayout.TextArea(dialgueTextListSP
                    .GetArrayElementAtIndex(i).stringValue, textAreaGUIStyle);
                
                GUILayout.BeginVertical("box");
                
                textGUIStyle.fontStyle = FontStyle.Bold;
                textGUIStyle.stretchWidth = false;
                textGUIStyle.fixedWidth = position.width / 5;
                DialogueTextCustomValueHeaders();
                textGUIStyle.stretchWidth = true;
                textGUIStyle.fontStyle = FontStyle.Normal;

                GUILayout.EndVertical();
                
                GUILayout.BeginVertical("box");
                DialogueTextCustomValues(i, _characterCustomTextKeys, 200);
                GUILayout.EndVertical();

                textGUIStyle.fontStyle = FontStyle.Normal;
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        dialogueSentenceSO.ApplyModifiedProperties();

        GUILayout.EndScrollView();
        
        GUILayout.BeginHorizontal("box");
        
        dialogueSOnameSO.Update();
        dialogueSOname = dialogueSOnameSO.FindProperty("_dialogueSOname");
        dialogueSOname.stringValue = EditorGUILayout.TextField("Dialogue SO File Name", dialogueSOname.stringValue);
        dialogueSOnameSO.ApplyModifiedProperties();
        
        if (IsCreateFileButtonValid(dialogueSOname.stringValue))
        {
            if (GUILayout.Button($"Create {dialogueTypeNames[selectedOption]} SO"))
            {
                if (IsCreatingFileValid())
                {
                    CreateScriptableObject(selectedOption, dialogueSOname.stringValue);
                }
            }
        }
        else
        {
            GUI.color = Color.gray;
            GUILayout.Button($"Create {dialogueTypeNames[selectedOption]} SO");
            GUI.color = Color.white;
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal("box");
        dialogueSOnameSO.Update();
        dialogueSOname = dialogueSOnameSO.FindProperty("_importDialogue");
        dialogueSOname.objectReferenceValue = EditorGUILayout.ObjectField(dialogueSOname.objectReferenceValue, 
            typeof(Dialogue), false) as Dialogue;
        dialogueSOnameSO.ApplyModifiedProperties();

        if (dialogueSOname.objectReferenceValue != null)
        {
            if (GUILayout.Button($"Change {dialogueSOname.objectReferenceValue}"))
            {
                if (IsChangeFileValid((Dialogue)dialogueSOname.objectReferenceValue))
                {
                    CreateScriptableObject(selectedOption, dialogueSOname.objectReferenceValue.name);
                }
            }
        }
        else
        {
            GUI.color = Color.gray;
            GUILayout.Button($"Change {dialogueSOname.objectReferenceValue}");
            GUI.color = Color.white;
        }
        
        GUILayout.EndHorizontal();

        CreateBackButton();
        
        GUILayout.EndVertical();  
    }

    private void DialogueTextCustomValueHeaders()
    {
        GUILayout.TextField("Text Write Speeds: ", textGUIStyle);
        GUILayout.TextField("Text Effects: ", textGUIStyle);
        GUILayout.TextField("Text Write Sounds: ", textGUIStyle);
        GUILayout.TextField("Text Colors: ", textGUIStyle);
        GUILayout.TextField("Text Overwrites: ", textGUIStyle);
    }
    
    private void DialogueTextCustomValues(int i, string[] characterTextKeys, int customWidth = 200, 
        bool saveEditorPrefs = false)
    {
        dialgueCharacterNamesListSP = dialogueSentenceSO.FindProperty(characterTextKeys[0]);
        dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.FloatField(
            dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).floatValue, GUILayout.Width(customWidth));

        if (saveEditorPrefs)
        {
            EditorPrefs.SetFloat(SO_WRITE_SPEED_DEF_VAL, dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).floatValue);
        }
        
        dialgueCharacterNamesListSP = dialogueSentenceSO.FindProperty(characterTextKeys[1]);
        ETextEffects currentTextEffect = (ETextEffects) Enum.Parse(typeof(ETextEffects),
            dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).GetEnumName<ETextEffects>());
        ETextEffects newTextEffects  = (ETextEffects)EditorGUILayout.EnumPopup(currentTextEffect,
            GUILayout.Width(customWidth));
        dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).SetEnumValue(newTextEffects);

        if (saveEditorPrefs)
        {
            EditorPrefs.SetInt(SO_TEXT_EFFECT_DEF_VAL, dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).enumValueIndex);
        }
        
        dialgueCharacterNamesListSP = dialogueSentenceSO.FindProperty(characterTextKeys[2]);
                
        dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).objectReferenceValue = 
            EditorGUILayout.ObjectField("", dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).objectReferenceValue,
                typeof(AudioSource), true, GUILayout.Width(customWidth));
        
        dialgueCharacterNamesListSP = dialogueSentenceSO.FindProperty(characterTextKeys[3]);
        dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).colorValue = EditorGUILayout.ColorField(
            dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).colorValue, GUILayout.Width(customWidth));
        
        
        dialgueCharacterNamesListSP = dialogueSentenceSO.FindProperty(characterTextKeys[4]);
        dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).boolValue =
            EditorGUILayout.Toggle(dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).boolValue,
                GUILayout.Width(customWidth));

        if (saveEditorPrefs)
        {
            EditorPrefs.SetBool(SO_TEXT_OVERWRITES_DEF_VAL, dialgueCharacterNamesListSP.GetArrayElementAtIndex(i).boolValue);
        }
    }
    
    private void CreateScriptableObject(int selectedSOindex, string fileName)
    {
        string fullPath = $"{EditorPrefs.GetString(SO_FOLDER_PATH_KEY)}/{fileName}.asset";

        Dialogue dialogue = null;

        switch (selectedSOindex)
        {
            case 0:
                dialogue = ScriptableObject.CreateInstance<UIBasicDialogue>();
                CreateAsset(dialogue, fullPath);
                dialogue = AssetDatabase.LoadAssetAtPath<UIBasicDialogue>(fullPath);
                break;
            case 1:
                dialogue = ScriptableObject.CreateInstance<UISimpDialogue>();
                CreateAsset(dialogue, fullPath);
                dialogue = AssetDatabase.LoadAssetAtPath<UISimpDialogue>(fullPath);
                break;
            case 2:
                dialogue = ScriptableObject.CreateInstance<UIDialogue>();
                CreateAsset(dialogue, fullPath);
                dialogue = AssetDatabase.LoadAssetAtPath<UIDialogue>(fullPath);
                break;
        }
        
        // dialogue default values
        dialogue.sentences = _dialogueTexts.ToArray();
        dialogue.characterCounts = selectedCharacterNameIndices.ToArray();
        dialogue.defOverWrites = _textOverwrites[0];
        dialogue.defTextWriteSpeeds = _characterTextWriteSpeeds;
        dialogue.defTextAudios = _textWriteSounds;
        dialogue.defTextEffects = _textEffects;
        dialogue.defDiffColor = _textColors;
        
        switch (selectedSOindex)
        {
            case 0:
   
                break;
            case 1:
                
                break;
            case 2:

                break;
        }
        
        // dialogue custom values
        for (int i = 0; i < totalTextAmount; i++)
        {
            if (_customTextOverwrites[i] != _defTextOverwrites[0])
            {
                if (dialogue.overWrites == null)
                {
                    dialogue.overWrites = new List<OverWriteDic>();
                }
                
                OverWriteDic overWriteDic = new OverWriteDic
                {
                    id = i,
                    overWrite = _customTextOverwrites[i]
                };
                
                dialogue.overWrites.Add(overWriteDic);
            }

            if (_customCharacterTextWriteSpeeds[i] != _defCharacterTextWriteSpeeds[0])
            {
                if (dialogue.textWriteSpeeds == null)
                {
                    dialogue.textWriteSpeeds = new List<TextWriteSpeedDic>();
                }

                TextWriteSpeedDic textWriteSpeedDic = new TextWriteSpeedDic
                {
                    id = i,
                    textWriteSpeed = _customCharacterTextWriteSpeeds[i]
                };
                
                dialogue.textWriteSpeeds.Add(textWriteSpeedDic);
            }

            if (_customTextWriteSounds[i] != null)
            {
                if (dialogue.textAudios == null)
                {
                    dialogue.textAudios = new List<SoundEffectSODic>();
                }

                SoundEffectSODic soundEffectSoDic = new SoundEffectSODic
                {
                    id = i,
                    textAudio = _customTextWriteSounds[i]
                };
                
                dialogue.textAudios.Add(soundEffectSoDic);
            }

            if (_customTextEffects[i] != _defTextEffects[0])
            {
                if (dialogue.textEffects == null)
                {
                    dialogue.textEffects = new List<ETextEffectsDic>();
                }

                ETextEffectsDic textEffectsDic = new ETextEffectsDic
                {
                    id = i,
                    textEffect = _customTextEffects[i]
                };
                
                dialogue.textEffects.Add(textEffectsDic);
            }

            if (_customTextColors[i] != _defTextColors[0])
            {
                if (dialogue.diffColor == null)
                {
                    dialogue.diffColor = new List<DiffColorDic>();
                }

                DiffColorDic diffColorDic = new DiffColorDic
                {
                    id = i,
                    diffColor = _customTextColors[i]
                };
                
                dialogue.diffColor.Add(diffColorDic);
            }
        }
       
    }

    private void CreateAsset(Object crateObject, string path)
    {
        AssetDatabase.CreateAsset(crateObject, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private bool IsCreateFileButtonValid(string dialogueSOname)
    {
        return !dialogueSOname.IsEmpty();
    }

    private bool IsCreatingFileValid()
    {
        if (characterAmount <= 0)
        {
            Debug.LogError("CharacterAmount value not valid");
            return false;
        }

        if (totalTextAmount <= 0)
        {
            Debug.LogError("TotalTextAmount value not valid");
            return false;
        }
        
        return true;
    }
    
    private bool IsChangeFileValid(Dialogue dialogue)
    {
        if (characterAmount <= 0)
        {
            Debug.LogError("CharacterAmount value not valid");
            return false;
        }

        if (totalTextAmount <= 0)
        {
            Debug.LogError("TotalTextAmount value not valid");
            return false;
        }

        if (dialogue.GetType() != dialogueTypes[selectedOption].GetType())
        {
            Debug.LogError("SO types are not same");
            return false;
        }
        
        return true;
    }
    
    private void CreateBackButton(Action action = null)
    {
        if (GUILayout.Button("<- Back", buttonGUIStyle))
        {
            action?.Invoke();
            tabStack.Pop();
        }
    }
    private void CreateEmptyBackButton(string errorMessage)
    {
        GUI.color = Color.gray;
        if (GUILayout.Button("<- Back", buttonGUIStyle))
        {
            Debug.LogError(errorMessage);
        }
        GUI.color = Color.white;
    }
}
