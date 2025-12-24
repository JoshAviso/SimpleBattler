
using System;
using UnityEngine;

public class LogUtils
{
    public static void Log(String str) { Debug.Log("[INFO]: " + str); } 
    public static void LogWarning(String str) { Debug.LogWarning("[WARN]: " + str); } 
    public static void LogError(String str) { Debug.LogError("[ERROR]: " + str); } 
    public static void Log(MonoBehaviour obj, String str) { Debug.Log($"[INFO]({obj.gameObject.name}): " + str); } 
    public static void LogWarning(MonoBehaviour obj, String str) { Debug.LogWarning($"[WARN]({obj.gameObject.name}): " + str); } 
    public static void LogError(MonoBehaviour obj, String str) { Debug.LogError($"[ERROR]({obj.gameObject.name}): " + str); } 
    public static void Log(UnityEngine.Object obj, String str) { Debug.Log($"[INFO]({obj.name}): " + str); } 
    public static void LogWarning(UnityEngine.Object obj, String str) { Debug.LogWarning($"[WARN]({obj.name}): " + str); } 
    public static void LogError(UnityEngine.Object obj, String str) { Debug.LogError($"[ERROR]({obj.name}): " + str); } 

}