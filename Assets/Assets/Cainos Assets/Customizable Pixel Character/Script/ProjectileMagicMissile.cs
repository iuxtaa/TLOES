using Cainos.LucidEditor;
using UnityEngine;

namespace Cainos.CustomizablePixelCharacter
{

    public class ProjectileMagicMissile : Projectile
    {
        [FoldoutGroup("Prefab"), AssetsOnly] public GameObject launchFxPrefab;
        [FoldoutGroup("Prefab"), AssetsOnly] public GameObject hitFxPrefab;

        protected override void OnLaunched()
        {
            base.OnLaunched();

            if (launchFxPrefab)
            {
                var launchFx = Instantiate(launchFxPrefab);
                launchFx.transform.position = transform.position;
                launchFx.transform.rotation = transform.rotation;
            }
        }


        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);

            //generate hit fx
            if (hitFxPrefab)
            {
                var hitFx = Instantiate(hitFxPrefab);
                hitFx.transform.position = transform.position;
                hitFx.transform.rotation = transform.rotation;
            }

            Destroy(gameObject);
        }
    }
}
