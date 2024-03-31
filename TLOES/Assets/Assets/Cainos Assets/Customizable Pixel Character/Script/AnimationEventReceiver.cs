using UnityEngine;

namespace Cainos.CustomizablePixelCharacter
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        private PixelCharacterController controller;

        private void Awake()
        {
            controller = GetComponentInParent<PixelCharacterController>();
        }

        public void OnFootstep(AnimationEvent evt)
        {
            controller.OnFootstep(evt);
        }


        #region - ATTACK EVENTS

        //the animatorStateInfo.speed here is used to distinguish
        //Attack Action - Arm - speed: 1.0
        //Attack Action - Body - speed: 0.95
        //events from the body layer is not send to the controller to prevent it form being fired twice

        public void OnAttackStart(AnimationEvent evt)
        {
            if (evt.animatorStateInfo.speed < 0.99f) return;

            controller.OnAttackStart();
        }
        public void OnAttackHit(AnimationEvent evt)
        {
            if (evt.animatorStateInfo.speed < 0.99f) return;

            controller.OnAttackHit();
        }
        public void OnAttackCast(AnimationEvent evt)
        {
            if (evt.animatorStateInfo.speed < 0.99f) return;

            controller.OnAttackCast();
        }


        public void OnAttackEnd(AnimationEvent evt)
        {
            if (evt.animatorStateInfo.speed < 0.99f) return;

            controller.OnAttackEnd();
        }

        public void OnThrow()
        {
            controller.OnThrow();
        }

        #endregion

        #region - ARCHERY EVENTS -

        public void OnArrowDraw()
        {
            if (controller) controller.OnArrowDraw();
        }

        public void OnArrowNock()
        {
            if (controller) controller.OnArrowNock();
        }

        public void OnArrowReady()
        {
            if (controller) controller.OnArrowReady();
        }

        public void OnArrowPutBack()
        {
            if (controller) controller.OnArrowPutBack();
        }

        #endregion

        #region - LEDGE CLIMB EVENTS - 

        //when ledge climb animation passes the time this event defines
        //it cannot be cancelled by releasing forward key
        public void OnLedgeClimbLocked()
        {
            if (controller) controller.OnLedgeClimbLocked();
        }

        public void OnLedgeClimbFinised()
        {
            if (controller) controller.OnLedgeClimbFinised();
        }

        #endregion

        #region - LADDER CLIMB EVENTS -

        public void OnLadderEntered()
        {
            if (controller) controller.OnLadderEntered();
        }

        public void OnLadderExited()
        {
            if (controller) controller.OnLadderExited();
        }

        #endregion

        #region - CRAWL EVENTS -

        public void OnCrawlEnter()
        {
            if (controller) controller.OnCrawlEnter();
        }
        public void OnCrawlEntered()
        {
            if (controller) controller.OnCrawlEntered();
        }
        public void OnCrawlExit()
        {
            if (controller) controller.OnCrawlExit();
        }
        public void OnCrawlExited()
        {
            if (controller) controller.OnCrawlExited();
        }



        #endregion

        #region - DODGE EVENTS -
        private void OnDodgeStart()
        {
            controller.OnDodgeStart();
        }

        private void OnDodgeEnd()
        {
            controller.OnDodgeEnd();
        }

        #endregion
    }
}
