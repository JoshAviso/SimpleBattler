using UnityEngine;
using UnityEngine.Events;

public class ActionFinishedCallback : MonoBehaviour
{
    public UnityEvent OnFinishedCallback;
    public void OnFinishedAction(){ OnFinishedCallback?.Invoke(); }
}
