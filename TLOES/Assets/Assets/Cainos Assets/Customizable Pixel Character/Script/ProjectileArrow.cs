using Cainos.LucidEditor;
using UnityEngine;

namespace Cainos.CustomizablePixelCharacter
{

    public class ProjectileArrow : Projectile
    {
        [Space]

        [Tooltip("If the angle between arrow direction and hit surface normal is below this angle, the arrow can insert into the surface.")]
        [FoldoutGroup("Params")] public float insertMaxAngle = 60.0f;

        [Tooltip("Arrow insert depth when hit.")]
        [FoldoutGroup("Params")] public float insertDepth = 0.1f;


        private Vector2 hitVel;

        private bool hasInsertedIntoTarget;
        private float curInsertDepth;

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (HasHit == true) return;
            base.OnCollisionEnter2D(collision);

            if ( Vector2.Angle(collision.contacts[0].normal, -transform.right) < insertMaxAngle)
            {
                hasInsertedIntoTarget = true;

                transform.SetParent(collision.collider.transform, true);
                hitVel = -collision.relativeVelocity;
                Rigidbody2D.simulated = false;
            }
        }

        protected override void Update()
        {
            if (IsLaunched == false) return;

            base.Update();

            if (hasHit == false)
            {
                float angle = Mathf.Atan2(Rigidbody2D.velocity.y, Rigidbody2D.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            if ( hasInsertedIntoTarget)
            {
                if (curInsertDepth < insertDepth)
                {
                    transform.Translate(hitVel * Time.deltaTime, Space.World);
                    curInsertDepth += hitVel.magnitude * Time.deltaTime;
                }


            }
        }
    }
}
