
using UnityEngine;

public interface IVoiceDialogue
{
    //对话换行事件
    public void OnDialogueLineChanged(object sender, DialogueControl.DialogueLineChangedEventArgs e);

    // 处理对话结束事件
    public void OnDialogueEnded(object sender, System.EventArgs e);

    // 根据对话行索引播放相应的语音片段
    public void PlayVoice(int lineIndex = 0);
}