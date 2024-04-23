using Cainos.Common;
using UnityEngine;



namespace Cainos.CustomizablePixelCharacter
{
    public class WeaponBow : Weapon
    {
        public Transform rigString;

        public bool IsStringPulled
        {
            get { return isStringPulled; }
            set
            {
                if (isStringPulled == value) return;
                isStringPulled = value;
            }
        }
        private bool isStringPulled;

        public Vector2 StringPullPos
        {
            set
            {
                stringPullPos = transform.InverseTransformPoint(value);
            }
        }
        private Vector2 stringPullPos;

        private Vector2 stringOriginPos;
        private Vector2 targetStringPos;
        private SecondOrderDynamics secondOrderDynamics = new SecondOrderDynamics(4.0f, 0.3f, 5.0f);

        private void Start()
        {
            stringOriginPos = rigString.localPosition;
            secondOrderDynamics.Reset(stringOriginPos);
        }

        private void Update()
        {
            targetStringPos = IsStringPulled ? stringPullPos : stringOriginPos;
            rigString.transform.localPosition = secondOrderDynamics.Update(targetStringPos, Time.deltaTime);
        }
    }
}
