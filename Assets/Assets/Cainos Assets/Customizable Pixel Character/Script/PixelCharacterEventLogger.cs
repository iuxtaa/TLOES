using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.CustomizablePixelCharacter
{

    public class PixelCharacterEventLogger : MonoBehaviour
    {
        private PixelCharacterController controller;

        private void Awake()
        {
            controller = GetComponent<PixelCharacterController>();

            controller.onFootstep.AddListener(()=>{ Debug.Log("Footstep");});
            controller.onJump.AddListener(() => { Debug.Log("Jump"); });
            controller.onLand.AddListener(() => { Debug.Log("Land"); });

            controller.onAttackStart.AddListener(() => { Debug.Log("Attack Start"); });
            controller.onAttackHit.AddListener(() => { Debug.Log("Attack Hit"); });
            controller.onAttackEnd.AddListener(() => { Debug.Log("Attack End"); });
            controller.onAttackCast.AddListener(() => { Debug.Log("Attack Cast"); });

            controller.onDodgeStart.AddListener(() => { Debug.Log("Dodge Start"); });
            controller.onDodgeEnd.AddListener(() => { Debug.Log("Dodge End"); });
        }
    }
}
