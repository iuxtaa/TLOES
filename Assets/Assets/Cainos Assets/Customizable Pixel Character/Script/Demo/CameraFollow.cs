using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//let camera follow target
namespace Cainos.CustomizablePixelCharacter
{
    public class CameraFollow : MonoBehaviour
    {
        public float lerpSpeed = 1.0f;

        private Vector3 offset;

        private Vector3 targetPos;

        private void Start()
        {
            if (PixelCharacter.instance == null) return;

            offset = transform.position - PixelCharacter.instance.transform.position;
        }

        private void Update()
        {
            if (PixelCharacter.instance == null) return;

            targetPos = PixelCharacter.instance.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }

    }
}
