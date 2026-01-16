using UnityEngine;

public class AnimActionFinishedCallback : StateMachineBehaviour {
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var handler = animator.GetComponent<ActionFinishedCallback>();
        if (handler != null) {
            handler.OnFinishedAction(); 
            LogUtils.Log(this, "OnStateExit");
        } else
        {
            LogUtils.LogWarning(this, "Character Animator missing ActionFinishedCallback component");
        }
    }
}