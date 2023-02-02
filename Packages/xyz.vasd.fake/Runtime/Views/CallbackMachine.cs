using UnityEngine;

namespace Xyz.Vasd.Fake.Views
{
    public class CallbackMachine : StateMachineBehaviour
    {
        public string ExitCallback = "callback_open";

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(ExitCallback, true);
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            animator.SetBool(ExitCallback, true);
        }
    }
}
