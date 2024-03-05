using UnityEngine;



namespace Lotus.CoreFramework
{
    public class AnimatorStates : MonoBehaviour
    {
        public AnimationStates currentState;



        private Animator _ator = null;
        public Animator Ator => this.TryGetComponent(ref _ator);


        public bool ApplyRootMotion
        {
            get => Ator.applyRootMotion;
            set => Ator.applyRootMotion = value;
        }


        private string currentTrigger = "";




        public void OnStateEnter(AnimationEvent state)
        {
            if (currentState == (AnimationStates)state.intParameter)
                return;

            if (!string.IsNullOrEmpty(currentTrigger) && !currentTrigger.Equals(state.stringParameter))
                Ator.ResetTrigger(currentTrigger);

            currentTrigger = state.stringParameter;
            currentState = (AnimationStates)state.intParameter;
        }





    }
}

