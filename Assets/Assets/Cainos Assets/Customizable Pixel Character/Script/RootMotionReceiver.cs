
using UnityEngine;

namespace Cainos.CustomizablePixelCharacter
{
    //getting root motion velocity from animator
    public class RootMotionReceiver : MonoBehaviour
    {
        public Vector2 rootMotionVel;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void OnAnimatorMove()
        {
            rootMotionVel = animator.deltaPosition / Time.deltaTime;
        }
    }
}
