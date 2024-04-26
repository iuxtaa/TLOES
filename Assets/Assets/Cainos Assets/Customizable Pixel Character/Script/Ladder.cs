using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.CustomizablePixelCharacter
{
    public class Ladder : MonoBehaviour
    {
        public Direction direction = Direction.Right;
        public Transform climbPos;

        public Vector3 TopPos
        {
            get
            {
                return BoxCollider2D.bounds.center + new Vector3( 0.0f, BoxCollider2D.size.y * 0.5f, 0.0f);
            }
        }

        public Vector3 BottomPos
        {
            get
            {
                return BoxCollider2D.bounds.center - new Vector3(0.0f, BoxCollider2D.size.y * 0.5f, 0.0f);
            }
        }

        public Vector3 ClimbPos
        {
            get
            {
                return climbPos.transform.position;
            }
        }

        private BoxCollider2D BoxCollider2D
        {
            get
            {
                if (boxCollider2D == null) boxCollider2D = GetComponent<BoxCollider2D>();
                return boxCollider2D;
            }
        }
        private BoxCollider2D boxCollider2D;

        public enum Direction
        {
            Left = -1,
            Right = 1
        }
    }
}
