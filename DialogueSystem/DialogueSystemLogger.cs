using UnityEngine;

/// <summary>
/// de'shde'shde'shide的时候没有debug语句
/// </summary> <summary>
/// 对话系统特定logger
/// </summary>
public static class DialogueSystemLogger
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(string message)
    {
        Debug.Log(message);
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
    public static void LogWarning(string message)
    {
        Debug.LogWarning(message);
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
    public static void LogError(string message)
    {
        Debug.LogError(message);
    }

}