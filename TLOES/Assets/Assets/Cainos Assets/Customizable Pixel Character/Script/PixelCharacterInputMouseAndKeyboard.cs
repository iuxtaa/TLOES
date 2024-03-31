using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using static UnityEngine.EventSystems.StandaloneInputModule;


namespace Cainos.CustomizablePixelCharacter
{
    //to feed the MonsterController input parameters using mouse and keyboard input
    public class PixelCharacterInputMouseAndKeyboard : MonoBehaviour
    {
        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;
        [Space]
        public KeyCode runKey = KeyCode.LeftShift;
        public KeyCode dodgeKey = KeyCode.LeftControl;
        public KeyCode crounchKey = KeyCode.C;
        public KeyCode crawlKey = KeyCode.Z;
        public KeyCode jumpKey = KeyCode.Space;
        [Space]
        public KeyCode attackKey = KeyCode.Mouse0;
        public KeyCode meleeKey = KeyCode.V;
        [Space]
        public KeyCode lookKey = KeyCode.Mouse1;

        private PixelCharacterController controller;

        private Vector2 inputMove;
        private bool inputDodge;
        private bool inputRun;
        private bool inputCrouch;
        private bool inputCrawl;
        private bool inputJump;
        private bool inputAttack;
        private bool inputMelee;
        private bool inputLook;
        private Vector2 inputTarget;

        private int dashKeyDownCounter;
        private float dashKeyDownTimer;
        private int dashDirection;

        private void Awake()
        {
            controller = GetComponent<PixelCharacterController>();
        }

        private void Update()
        {
            bool pointerOverUI = EventSystem.current && EventSystem.current.IsPointerOverGameObject();

            //pointer over ui, cancel any inputs
            if (pointerOverUI)
            {
                inputMove = Vector2.zero;
                inputRun = false;
                inputJump = false;
                inputAttack = false;
                inputMelee = false;
            }
            else
            {
                inputRun = Input.GetKey(runKey);
                inputDodge = Input.GetKey(dodgeKey);
                inputJump = Input.GetKey(jumpKey);
                inputAttack = Input.GetKey(attackKey);
                inputMelee = Input.GetKey(meleeKey);
                inputLook = Input.GetKey(lookKey);

                //crouch
                if (Input.GetKeyDown(crounchKey) && controller.IsInAir == false)
                {
                    if (inputCrawl && controller.CanExitCrawling )
                    {
                        inputCrawl = false;
                        inputCrouch = true;
                    }
                    else
                    {
                        inputCrouch = !inputCrouch;
                    }

                    if (controller.CanExitCrouching == false) inputCrouch = true;
                }

                //crawl
                if (Input.GetKeyDown(crawlKey) && controller.IsInAir == false )
                {
                    inputCrawl = !inputCrawl;
                    if (controller.CanEnterCrawling == false) inputCrawl = false;
                    if (controller.CanExitCrawling == false) inputCrawl = true;
                    if (inputCrawl == false) inputCrouch = false;
                }

                //run or jump input cancels crouch and crawl
                if (Input.GetKeyDown(runKey) || Input.GetKeyDown(jumpKey))
                {
                    inputCrouch = false;
                    inputCrawl = false;
                }

                //staying in air for more than 1s cancels crouch and crawl
                if ( controller.AirTimer > 1.0f)
                {
                    inputCrouch = false;
                    inputCrawl = false;
                }

                //do not allow entering crouch or crawl when climbing ladder
                if (controller.IsClimbingLadder || controller.IsEnteringLadder || controller.IsExitingLadder )
                {
                    inputCrouch = false;
                    inputCrawl = false;
                }

                //when there is run input
                //double left or right direction key to dash
                if (inputRun)
                {
                    if (Input.GetKeyDown(leftKey))
                    {
                        if (dashDirection != -1) dashKeyDownCounter = 0;
                        dashDirection = -1;
                        dashKeyDownCounter++;
                        if (dashKeyDownCounter >= 2)
                        {
                            controller.inputDash = true;
                            dashKeyDownTimer = 0.0f;
                            dashKeyDownCounter = 0;
                        }
                    }
                    if (Input.GetKeyDown(rightKey))
                    {
                        if (dashDirection != 1) dashKeyDownCounter = 0;
                        dashDirection = 1;
                        dashKeyDownCounter++;
                        if (dashKeyDownCounter >= 2)
                        {
                            controller.inputDash = true;
                            dashKeyDownTimer = 0.0f;
                            dashKeyDownCounter = 0;
                        }
                    }
                }

                //move horizontal
                if (Input.GetKey(leftKey)) inputMove.x = -1.0f;
                else if (Input.GetKey(rightKey)) inputMove.x = 1.0f;
                else inputMove.x = 0.0f;

                //move vertical
                if (Input.GetKey(downKey)) inputMove.y = -1.0f;
                else if (Input.GetKey(upKey)) inputMove.y = 1.0f;
                else inputMove.y = 0.0f;

                inputTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            }

            //clear dashKeyDownCounter
            if (dashKeyDownCounter > 0)
            {
                dashKeyDownTimer += Time.deltaTime;
                if (dashKeyDownTimer > 0.5f)
                {
                    dashKeyDownTimer = 0.0f;
                    dashKeyDownCounter -= 1;
                }
            }

            //feed input to controller
            if (controller)
            {
                controller.inputMove = inputMove;
                controller.inputRun = inputRun;
                controller.inputDodge = inputDodge;
                controller.inputCrounch = inputCrouch;
                controller.inputCrawl = inputCrawl;
                controller.inputJump = inputJump;
                controller.inputAttack = inputAttack;
                controller.inputMelee = inputMelee;
                controller.inputLook = inputLook;
                controller.inputTarget = inputTarget;
            }
        }
    }
}
