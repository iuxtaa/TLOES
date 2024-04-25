using UnityEngine;


namespace Cainos.CustomizablePixelCharacter
{

    public class Weapon : MonoBehaviour
    {
        public Transform tip;

        public Vector3 TipPosition
        {
            get
            {
                if ( tip) return tip.position;
                return transform.position + transform.right;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return transform.rotation;
            }
        }
    }
}
