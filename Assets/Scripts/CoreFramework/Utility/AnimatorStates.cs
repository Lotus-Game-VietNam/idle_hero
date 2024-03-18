using Sirenix.OdinInspector;
using UnityEngine;


namespace Lotus.CoreFramework
{
    public class AnimatorStates : MonoBehaviour
    {
        [Title("State Debug")]
        public AnimationStates currentState;

        [Title("Parameter")]
        public string[] parameter = null;



        private Animator _ator = null;
        public Animator Ator => this.TryGetComponent(ref _ator);


        private AnimationEvents _events = null;
        public AnimationEvents events => this.TryGetComponent(ref _events);


        public bool ApplyRootMotion
        {
            get => Ator.applyRootMotion;
            set => Ator.applyRootMotion = value;
        }


        private string currentTrigger = "";




        public void Initialized()
        {
            transform.localPosition = transform.localEulerAngles = Vector3.zero;
        }



        public void OnStateEnter(AnimationEvent state)
        {
            if (currentState == (AnimationStates)state.intParameter)
                return;

            if (!string.IsNullOrEmpty(currentTrigger) && !currentTrigger.Equals(state.stringParameter))
                Ator.ResetTrigger(currentTrigger);

            currentTrigger = state.stringParameter;
            currentState = (AnimationStates)state.intParameter;
        }



        public void ChangeState(AnimationStates state)
        {
            if (currentState == state)
                return;

            if (Utilities.IsMovement(state))
                SetBlend(state, AnimationStates.Idle, "Speed");
            else
                Ator.SetFloat("Speed", 0);

            if (Utilities.IsAttack(state))
                SetBlend(state, AnimationStates.NormalAttack, "AttackType");

            Ator.SetTrigger(GetParam(state));
        }



        private void SetBlend(AnimationStates state, AnimationStates firstBlend, string param)
        {
            float value = (int)state - (int)firstBlend;
            Ator.SetFloat(param, value);
        }


        private string GetParam(AnimationStates state) => parameter[(int)state];
    }
}

