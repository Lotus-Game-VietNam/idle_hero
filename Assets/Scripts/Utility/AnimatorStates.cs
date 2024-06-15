using Sirenix.OdinInspector;
using UnityEngine;


namespace Lotus.CoreFramework
{
    public class AnimatorStates : MonoBehaviour
    {
        [Title("Object Reference")]
        public Animator[] animators = null;

        [Title("State Debug")]
        public AnimationStates currentState;

        [Title("Parameter")]
        public string[] parameter = null;


        private AnimationEvents _events = null;
        public AnimationEvents events => this.TryGetComponent(ref _events);



        private string currentTrigger = "";




        public void Initialized()
        {
            transform.localPosition = transform.localEulerAngles = Vector3.zero;
        }


        public void OnStateEnter(AnimationEvent state)
        {
            if (currentState == (AnimationStates)state.intParameter)
                return;

            if (!string.IsNullOrEmpty(currentTrigger))
                ResetTrigger(currentTrigger);

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
                SetFloat("Speed", 0);

            if (!state.IsMovement())
                SetFloat("Speed", 0);

            if (state.IsAttack())
                SetBlend(state, AnimationStates.NormalAttack, "AttackType");

            string param = GetParam(state);

            if (!string.IsNullOrEmpty(currentTrigger))
                ResetTrigger(currentTrigger);

            currentTrigger = param;
            currentState = state;

            SetTrigger(param);
        }


        public void SetTrigger(string param)
        {
            foreach (var ator in animators)
                ator.SetTrigger(param);
        }

        public void SetFloat(string param, float value)
        {
            foreach (var ator in animators)
                ator.SetFloat(param, value);
        }

        public void ResetTrigger(string param)
        {
            foreach (var ator in animators)
                ator.ResetTrigger(param);
        }

        public void SetSpeed(float speed)
        {
            foreach (var ator in animators)
                ator.speed = speed;
        }

        public void PlayAnimation(string stateName)
        {
            foreach (var ator in animators)
                ator.Play(stateName);
        }


        public void SetBlend(AnimationStates state, AnimationStates firstBlend, string param)
        {
            float value = (int)state - (int)firstBlend;
            SetFloat(param, value);
        }

        public void Rebind()
        {
            foreach (var ator in animators)
                ator.Rebind();
        }


        private string GetParam(AnimationStates state) => parameter[(int)state];
    }
}

