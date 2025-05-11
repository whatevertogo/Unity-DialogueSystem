using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface IBranchingDialogue
{
    
    public void ShowDialogueOptions();
    public void HideDialogueOptions();
    public void ChooseOption(string optionKey);
    public void ShowOptionsAfterDialogue();
}