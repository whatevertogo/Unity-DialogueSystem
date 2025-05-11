# Unity Dialogue System Documentation

[中文](README.CN_DialogueSystem.md)

## System Introduction and Architecture

This is a modular Unity dialogue system based on MVC architecture. The system employs interface segregation and controller patterns to separate dialogue flow control from specific implementations, achieving high extensibility and maintainability.

### System Features

- **MVC Architecture**: Logic and UI decoupling through Control-View separation
- **Interface-based Design**: Feature extension through IBranchingDialogue and IVoiceDialogue interfaces
- **Data-Driven**: Dialogue data management using ScriptableObject-based DialogueSO
- **Event System**: Complete dialogue lifecycle events (start, end, line change, etc.)
- **Automated Components**: RequireComponent attributes ensure automatic addition of necessary components

### Core Components Detailed

#### DialogueControl (Core Control Class)
```csharp
public class DialogueControl : MonoBehaviour
{
    // Core Events
    public event EventHandler OnDialogueStarted;
    public event EventHandler OnDialogueEnded;
    public event EventHandler<DialogueLineChangedEventArgs> OnDialogueLineChanged;

    // Core Methods
    public void ShowDialogue()
    public void ShowNextLine()
    public void SetDialogueSO(DialogueSO)
    public void GoDialogueSOToLine(DialogueSO, int)
}
```

#### DialogueController (Base Controller)
```csharp
public class DialogueController : MonoBehaviour
{
    protected DialogueControl dialogueControl;
    
    public virtual void StartDialogue()
    public virtual void SkipDialogue()
}
```

#### DialogueSO (Data Container)
```csharp
[CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public string characterName;
    public Sprite Character_Image;
    public List<string> dialoguelinesList;
}
```

## System Structure

The system consists of the following core components:

### Data Layer
- **DialogueSO** - Dialogue data container (ScriptableObject)
  - Stores dialogue content and character information
  - Supports character names, portraits, and dialogue text

### Control Layer
- **DialogueControl** - Core control class
  - Manages dialogue flow and content display
  - Provides an event system for monitoring dialogue states
  
- **DialogueController** - Dialogue controller base class
  - Provides common functionality and interfaces
  - Can be inherited to implement different types of dialogue controllers
  
  Derived controllers:
  - **LinearDialogueController** - Linear dialogue implementation
  - **BranchingDialogueController** - Branching dialogue implementation
  - **VoiceDialogueController** - Voice dialogue implementation

  To maintain a good code structure, use IBranchingDialogue for branching dialogues and IVoiceDialogue for voice dialogues. If you need both, you can implement both interfaces. You can customize the interface or directly inherit from the DialogueController class to implement your own dialogue controller. It is recommended to add interfaces to maintain a good code structure.

### View Layer
- **DialogueControlView** - User interface control
  - Responsible for the visual presentation of dialogue content
  - Implements typewriter effect and user interaction

## System Configuration

### 1. Creating Dialogue Data

1. In the project panel, right-click -> Create -> Scriptable Objects -> DialogueSO
2. Set dialogue content:
   - Character name
   - Character portrait (optional)
   - List of dialogue lines

### 2. Scene Hierarchy Setup

```
Hierarchy:
├── DialogueSystem
│   ├── DialogueControl       (Add DialogueControl.cs)
│   └── DialogueControlView   (Add DialogueControlView.cs)
└── Canvas
    └── DialoguePanel         (UI panel, set as DialogueControlView's dialogue panel)
        └── Button            (Dialogue button, set as DialogueControlView's button)
            └── Text (TMP)    (Dialogue text, set as DialogueControlView's text component)
```

### 3. Component Configuration

1. Assign the created DialogueSO to the `dialogue_SO` field of DialogueControl
2. Assign DialogueControlView to the `dialogueView` field of DialogueControl
3. Configure UI component references for DialogueControlView:
   - Dialogue panel
   - Next line button
   - Dialogue text component
   - Typing speed and other parameters

### 4. Adding Dialogue Controllers

Based on your requirements, choose to add one (or more) of the following:

- **LinearDialogueController**: Regular sequential dialogue
- **BranchingDialogueController**: Branching dialogue with options
- **VoiceDialogueController**: Dialogue with voice acting
- **CustomDialogueController**: Custom dialogue controller (you can write your own!)

## Core Component Details

### DialogueSO

Dialogue data container that stores dialogue content and character information:

```csharp
public class DialogueSO : ScriptableObject
{
    public string characterName;        // Character name 1
    public string characterName2;       // Character name 2
    public Sprite Character_Image;      // Character portrait 1
    public Sprite Character_Image2;     // Character portrait 2
    public List<string> dialoguelinesList; // List of dialogue lines
}
```

### DialogueControl

Core control class that manages dialogue flow:

Important public methods:
- `ShowDialogue()` - Display dialogue
- `ShowNextLine()` - Show next line of dialogue
- `SetDialogueSO(DialogueSO)` - Switch dialogue data
- `GoDialogueSOToLine(DialogueSO, int)` - Jump to a specific dialogue line
- `SkipDialogue()` - Skip current dialogue

Important events:
- `OnDialogueStarted` - Dialogue start event
- `OnDialogueEnded` - Dialogue end event
- `OnDialogueLineChanged` - Dialogue line change event

### DialogueControlView

UI control class responsible for the visual presentation of dialogues:

Important public methods:
- `ShowDialoguePanel()` - Show dialogue panel
- `HideDialoguePanel()` - Hide dialogue panel
- `TypeDialogueLine(string)` - Display text with typewriter effect
- `CompleteCurrentLine()` - Immediately complete current typewriter effect
- `SetTypingSpeed(float)` - Set typing speed

## Dialogue Controllers

### Linear Dialogue (LinearDialogueController)

The most basic form of dialogue, displays dialogue content in sequence:

```csharp
// Add LinearDialogueController to an object in the scene
public class LinearDialogueController : DialogueController
{
    // Automatically start dialogue in Start()
    private void Start()
    {
        StartDialogue();  
    }
}
```

### Branching Dialogue (BranchingDialogueController)

Provides multiple dialogue branch options:

```csharp
public class BranchingDialogueController : DialogueController
{
    // Need to configure dialogue options
    [SerializeField] private List<DialogueOption> branchOptionsList;
    [SerializeField] private GameObject optionsPanel; 
    [SerializeField] private GameObject optionButtonPrefab;
    [SerializeField] private Transform optionsContainer;
    
    // Important methods
    public void ShowDialogueOptions()  // Display dialogue options
    public void ChooseOption(string)   // Select dialogue branch
}
```

### Voice Dialogue (VoiceDialogueController)

Dialogue with synchronized voice playback:

```csharp
[RequireComponent(typeof(AudioSource))]
public class VoiceDialogueController : DialogueController
{
    [SerializeField] private AudioSource audioSource;
    
    [Serializable]
    public class DialogueAudioClip
    {
        public int lineIndex;          // Dialogue line index
        public AudioClip audioClip;     // Corresponding voice clip
    }
    
    [SerializeField] private List<DialogueAudioClip> dialogueAudioClips; // Mapping between dialogue lines and voice clips
    private Dictionary<int, AudioClip> DialogueClipDictionary = new(); // Dictionary for quick lookup of voice clips
    
    // Important methods
    // PlayVoice(int lineIndex) - Play voice for a specific index
    // OnDialogueLineChanged - Subscribe to dialogue line change events to play voice
}
```

Usage:
1. Add VoiceDialogueController to a game object
2. Ensure the game object has an AudioSource component (will be added automatically if missing)
3. Configure dialogue line indices and corresponding voice clips in the Inspector
4. The system will automatically play the voice clip for the corresponding index when a dialogue line changes

## Advanced Usage
(joke)

1. **Custom dialogue line templates**: You can create custom dialogue line templates to implement more complex dialogue content.
2. **Extension of dialogue line templates**: You can extend dialogue line templates to implement more complex dialogue content.
3. **Combination of dialogue line templates**: You can combine multiple dialogue line templates to implement more complex dialogue content.
4. **Combination and extension of dialogue line templates**: You can combine and extend dialogue line templates to implement more complex dialogue content.
5. **Combination of combinations and extensions of dialogue line templates**: You can combine, extend, and combine multiple dialogue line templates to implement more complex dialogue content.
(smiley face)(hahaha,just joke,all depends yourself)

### Dialogue Event Listening

Implement custom behavior by subscribing to events:

```csharp
// Listen for dialogue start and end events
dialogueControl.OnDialogueStarted += OnDialogueStarted;
dialogueControl.OnDialogueEnded += OnDialogueEnded;

// Listen for dialogue line change events (especially useful for voice playback)
dialogueControl.OnDialogueLineChanged += OnDialogueLineChanged;
```

### Branching Dialogue Configuration

Configure branching dialogue options in the Inspector:

1. Create a DialogueSO for each branch
2. Add options in the BranchingDialogueController
3. Set option text, key value, and corresponding DialogueSO

### Automatic Option Display

In branching dialogues, you can automatically display options after dialogue ends:

```csharp
// Add this in Awake or Start:
if (dialogueControl != null)
{
    dialogueControl.OnDialogueEnded += (sender, args) => {
        ShowDialogueOptions();
    };
}
```

## Complete Usage Example

1. **Create dialogue data**
   - Create multiple DialogueSO resources
   - Fill in dialogue content and character information

2. **Configure the scene**
   - Add Canvas and dialogue UI elements
   - Create DialogueSystem object and add components

3. **Set up controllers**
   - Add appropriate dialogue controllers
   - Configure necessary references and parameters

4. **Start dialogue**
   - Call `StartDialogue()` via a trigger or other means

5. **Handle dialogue end**
   - Subscribe to the `OnDialogueEnded` event
   - Implement post-dialogue logic

## Common Issues

### Branch options not displaying

Check:
- optionsPanel is correctly set
- optionButtonPrefab contains Button and TextMeshProUGUI components
- ShowDialogueOptions() method has been called

### Dialogue not proceeding automatically

Check:
- nextLineDelay value is set to a positive number
- delayNextWord parameter is correctly passed to TypeDialogueLine

## Extending the System

You can extend the system by inheriting from existing controllers or directly modifying the source code:

- Add new dialogue controller types
- Extend DialogueControlView to support more UI effects
- Modify DialogueSO to store more complex dialogue structures

This way, we can create a well-organized and useful dialogue system!
