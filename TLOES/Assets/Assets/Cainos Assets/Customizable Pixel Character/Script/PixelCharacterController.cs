using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cainos.LucidEditor;

namespace Cainos.CustomizablePixelCharacter
{
    public class PixelCharacterController : MonoBehaviour
    {

        #region - STATIC PARAMETERS -

        private static readonly float CLIMB_POS_LERP_SPEED = 7.5f;                                  // lerp speed when moving the character to the climb position
        private static readonly float PIXEL_SIZE = 0.03125f;                                        // size of one pixel: 1/32
        private static readonly float SURFACE_ANGLE_LIMIT = 46.0f;                                  // the max angle of a slope the character can stand on, if the slope angle is larger than this, the character will slide down the slope
        private static readonly float CHARACTER_HEIGHT = 1.90625f;
        private static readonly float CHARACTER_WEIGHT = 50.0f;

        private static Vector3 TOP_POS_OFFSET = new(0.0f, PIXEL_SIZE * 60, 0.0f);
        private static readonly float CENTER_POS_OFFSET = PIXEL_SIZE * 30;
        private static readonly float NECK_POS_OFFSET = PIXEL_SIZE * 40;
        private static Vector3 BOTTOM_POS_OFFSET = new(0.0f, PIXEL_SIZE, 0.0f);

        #endregion

        #region - PARAMETERS -
        [FoldoutGroup("Movement")] public LayerMask groundCheckLayerMask;
        [Space]
        [FoldoutGroup("Movement")] public float walkSpeedMax = 2.5f;                           // max walk speed
        [FoldoutGroup("Movement")] public float walkAcc = 10.0f;                               // walking acceleration
        [Space]
        [FoldoutGroup("Movement")] public float runSpeedMax = 5.0f;                            // max run speed
        [FoldoutGroup("Movement")] public float runAcc = 15.0f;                                // running acceleration
        [Space]
        [FoldoutGroup("Movement")] public float crouchSpeedMax = 1.0f;                         // max move speed while crouching
        [FoldoutGroup("Movement")] public float crouchAcc = 8.0f;                              // crouching acceleration
        [Space]
        [FoldoutGroup("Movement")] public float crawlSpeedMax = 1.0f;                          // max move speed while crawling
        [FoldoutGroup("Movement")] public float crawlAcc = 8.0f;                               // crawling acceleration
        [Space]
        [FoldoutGroup("Movement")] public float airSpeedMax = 2.0f;                            // max move speed while in air
        [FoldoutGroup("Movement")] public float airAcc = 8.0f;                                 // air acceleration
        [Space]
        [FoldoutGroup("Movement")] public float groundDrag = 10.0f;                            // braking acceleration (from movement to still) while on ground
        [FoldoutGroup("Movement")] public float airDrag = 0.75f;                               // braking acceleration (from movement to still) while in air
        [Space]
        [FoldoutGroup("Movement")] public bool jumpEnabled = true;
        [FoldoutGroup("Movement"), ShowIf("jumpEnabled")] public float jumpSpeed = 5.0f;                            // speed applied to character when jump
        [FoldoutGroup("Movement"), ShowIf("jumpEnabled")] public float jumpCooldown = 0.2f;                         // time to be able to jump again after landing
        [FoldoutGroup("Movement"), ShowIf("jumpEnabled")] public float jumpTolerance = 0.15f;                       // when the character's air time is less than this value, it is still able to jump
        [FoldoutGroup("Movement"), ShowIf("jumpEnabled")] public float jumpGravityMutiplier = 0.6f;                 // gravity multiplier when character is jumping, should be within [0.0,1.0], set it to lower value so that the longer you press the jump button, the higher the character can jump    
        [FoldoutGroup("Movement"), ShowIf("jumpEnabled")] public float fallGravityMutiplier = 1.3f;                 // gravity multiplier when character is falling, should be equal or greater than 1.0
        [Space]
        [FoldoutGroup("Movement")] public bool dashEnabled = true;
        [FoldoutGroup("Movement"), ShowIf("dashEnabled")] public float dashSpeedStart = 2.5f;
        [FoldoutGroup("Movement"), ShowIf("dashEnabled")] public float dashSpeedMax = 7.0f;
        [FoldoutGroup("Movement"), ShowIf("dashEnabled")] public float dashAcc = 20.0f;
        [FoldoutGroup("Movement"), ShowIf("dashEnabled")] public float dashTime = 1.0f;
        [FoldoutGroup("Movement"), ShowIf("dashEnabled")] public float dashCooldown = 1.0f;
        [Space]
        [FoldoutGroup("Movement")] public bool dodgeEnabled = true;
        [FoldoutGroup("Movement"), ShowIf("dodgeEnabled")] public float dodgeSpeedMul = 1.25f;
        [FoldoutGroup("Movement"), ShowIf("dodgeEnabled")] public float dodgeCooldown = 0.1f;
        [Space]
        [FoldoutGroup("Movement")] public bool ladderClimbEnabled = true;
        [FoldoutGroup("Movement"), ShowIf("ladderClimbEnabled")] public float ladderClimbSpeed = 1.0f;              // laddder climb speed
        [FoldoutGroup("Movement"), ShowIf("ladderClimbEnabled")] public float ladderClimbSpeedFast = 1.5f;          // laddder climb speed when move modifier key is pressed
        [Space]
        [FoldoutGroup("Movement")] public bool ledgeClimbEnabled = true;

        [FoldoutGroup("Attack")] public AttackActionType attackAction = AttackActionType.Swipe;
        [FoldoutGroup("Attack")] public AttackActionMeleeType attackActionMelee = AttackActionMeleeType.Swipe;
        [Space]
        [FoldoutGroup("Attack")] public float throwForce = 10.0f;
        [FoldoutGroup("Attack")] public float throwAngularSpeed = 200.0f;
        [Space]
        [FoldoutGroup("Attack")] public float projectileSpeed = 20.0f;
        [FoldoutGroup("Attack")] public GameObject projectilePrefab;

        #endregion

        #region - EVENTS -

        [FoldoutGroup("Event")] public UnityEvent onFootstep;
        [FoldoutGroup("Event")] public UnityEvent onJump;
        [FoldoutGroup("Event")] public UnityEvent onLand;
        [Space]
        [FoldoutGroup("Event")] public UnityEvent onDodgeStart;
        [FoldoutGroup("Event")] public UnityEvent onDodgeEnd;
        [Space]
        [FoldoutGroup("Event")] public UnityEvent onAttackStart;
        [FoldoutGroup("Event")] public UnityEvent onAttackHit;
        [FoldoutGroup("Event")] public UnityEvent onAttackCast;
        [FoldoutGroup("Event")] public UnityEvent onAttackEnd;
        [FoldoutGroup("Event")] public UnityEvent onBowPull;
        [FoldoutGroup("Event")] public UnityEvent onBowShoot;
        [FoldoutGroup("Event")] public UnityEvent onThrow;

        #endregion

        #region - RUNTIME INPUT PARAMETERS- 
        [FoldoutGroup("Input"), DisableInEditMode] public Vector2 inputMove = Vector2.zero;                    // movement input, x for horizontal, y for vertical, x and y should be in [-1.0, 1.0]                 
        [FoldoutGroup("Input"), DisableInEditMode] public bool inputRun = false;                               // run input
        [FoldoutGroup("Input"), DisableInEditMode] public bool inputDash = false;                              // dash input
        [FoldoutGroup("Input"), DisableInEditMode] public bool inputDodge = false;                             // dodge input
        [FoldoutGroup("Input"), DisableInEditMode] public bool inputCrounch = false;                           // crourch input
        [FoldoutGroup("Input"), DisableInEditMode] public bool inputCrawl = false;                             // crawl input
        [FoldoutGroup("Input"), DisableInEditMode] public bool inputJump = false;                              // jump input
        [FoldoutGroup("Input"), DisableInEditMode] public bool inputAttack = false;                            // attack input
        [FoldoutGroup("Input"), DisableInEditMode] public bool inputMelee = false;                             // melee input
        [FoldoutGroup("Input"), DisableInEditMode] public bool inputLook = false;                              // look input
        [FoldoutGroup("Input"), DisableInEditMode] public Vector2 inputTarget = Vector2.zero;                  // the look at and point at target
        #endregion

        #region - DEBUG PARAMETERS- 
#if UNITY_EDITOR
        [FoldoutGroup("Debug"), LabelText("Draw Ledge Position")] public bool debug_drawLedgePos;
        [FoldoutGroup("Debug"), LabelText("Draw Ledge Raycast")] public bool debug_drawLedgeRaycast;
        [FoldoutGroup("Debug"), LabelText("Draw Ground Check Raycast")] public bool debug_drawGroundCheckRaycast;
        [FoldoutGroup("Debug"), LabelText("Draw Ground Normal")] public bool debug_drawGroundNormal;
        [FoldoutGroup("Debug"), LabelText("Draw Ground Direction")] public bool debug_drawGroundDir;
        [FoldoutGroup("Debug"), LabelText("Draw Crouch Raycast")] public bool debug_drawCrouchRaycast;
        [FoldoutGroup("Debug"), LabelText("Draw Crawl Enter Raycast")] public bool debug_drawCrawlEnterRaycast;
        [FoldoutGroup("Debug"), LabelText("Draw Crawl Exit Raycast")] public bool debug_drawCrawlExitRaycast;
        [FoldoutGroup("Debug"), LabelText("Draw Velocity")] public bool debug_drawVelocity;
        [FoldoutGroup("Debug"), LabelText("Draw Slide Velocity")] public bool debug_drawSlideVelocity;
#endif
        #endregion

        #region - PRIVATE PARAMETERS - 
        private PixelCharacter character;                           // the PixelCharacter script attached the character
        private Animator animator;
        private CapsuleCollider2D capsuleCollider2D;
        private Rigidbody2D rb2d;                                   // the Rigidbody2D component on the character
        private RootMotionReceiver rootMotionReceiver;

        private float gravityScale;
        private float idleTimer;

        private Vector2 startJumpVel;
        private float jumpCdTimer;                                  // timer for jump cooldown

        private float groundLiftSpeed;                              //

        private float getDownPlatformTimer;

        private Vector2 surfaceNormal;
        private Vector2 surfaceDir;
        private Vector2 surfaceDirDown;
        private float surfaceAngle;
        private float surfaceAngleForward;
        private float surfaceSpeedMul = 1.0f;
        #endregion

        #region - RUNTIME PROPERTIES - 

        // is the character dead? if dead, plays dead animation and disable control
        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsDead
        {
            get { return isDead; }
            set
            {
                if (isDead == value) return;
                isDead = value;

                IsDrawingBow = false;
                IsArrowDrawn = false;
                IsStringPulled = false;

                character.IsDead = value;
            }
        }
        private bool isDead;

        //is the character idle
        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsIdle
        {
            get
            {
                return (isMoving == false && IsInAir == false && isClimbingLadder == false && isExitingLadder == false && isClimbingLedge == false);
            }
        }

        //is the character in air
        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsInAir
        {
            get
            {
                return (!isGrounded && !isClimbingLadder && !isExitingLadder && !isClimbingLedge);
            }
        }

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public float AirTimer
        {
            get { return airTimer; }
        }
        private float airTimer;

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsClimbingLedge
        {
            get
            {
                return isClimbingLedge;
            }
        }

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsStandingOnPlatform
        {
            get
            {
                return isStandingOnPlatform;
            }
        }

        #endregion

        #region - PRIVATE HELPER PROPERTIES
        //world position of the character's center
        private Vector3 CenterPos
        {
            get
            {
                return transform.position + Vector3.up * CENTER_POS_OFFSET;
            }
        }

        //world position of the character's neck
        private Vector3 NeckPos
        {
            get
            {
                return transform.position + Vector3.up * NECK_POS_OFFSET;
            }
        }

        //world position of the character collider's top
        private Vector3 TopPos
        {
            get
            {
                return transform.position + TOP_POS_OFFSET;
            }
        }

        //world position of the character collider's bottom
        private Vector3 BottomPos
        {
            get
            {
                return transform.position + BOTTOM_POS_OFFSET;
            }
        }

        private Vector3 ColliderBottomPos
        {
            get
            {
                var pos = capsuleCollider2D.bounds.center;
                pos.y = capsuleCollider2D.bounds.min.y;
                return pos;
            }
        }

        private float ColliderHeight
        {
            get
            {
                return capsuleCollider2D.size.y;
            }
        }

        //raycast parameters for climbing to the ledge the ladder connects to
        private Vector3 LadderExitRaycastPos
        {
            get
            {
                return transform.position + new Vector3(15 * facing, 40, 0) * PIXEL_SIZE;
            }
        }
        private float LadderExitRaycastDis
        {
            get
            {
                return 40 * PIXEL_SIZE;
            }
        }


        //raycast parameters for climbing up ledge
        private Vector3 LedgeClimbRaycastPos
        {
            get
            {
                return transform.position + new Vector3(10 * facing * PIXEL_SIZE, LedgeClimbRaycastHeight, 0);
            }
        }
        private float LedgeClimbRaycastHeight
        {
            get
            {
                return 42 * PIXEL_SIZE;
            }
        }
        private float LedgeClimbRaycastDis
        {
            get
            {
                return 19 * PIXEL_SIZE;
            }
        }

        #endregion

        #region - ATTACK - 

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsDrawingBow
        {
            get { return isDrawingBow; }
            set
            {
                if (isDrawingBow == value) return;
                if (isClimbingLadder) return;
                if (isExitingLadder) return;
                if (isClimbingLedge) return;

                isDrawingBow = value;

                if (isDrawingBow == false)
                {
                    //arrow ready
                    if (isArrowReady && projectile)
                    {
                        //unable to shoot, destroy arrow projectile
                        if (isCrawling || isDead)
                        {
                            Destroy(projectile.gameObject);
                        }
                        //shoot arrow out
                        else
                        {
                            projectile.transform.SetParent(null, true);
                            projectile.transform.localScale = Vector3.one;
                            projectile.IsLaunched = true;
                            projectile.Velocity = projectileSpeed * projectile.transform.right;

                            onBowShoot.Invoke();
                        }
                    }

                    isArrowReady = false;
                    IsStringPulled = false;
                }

                animator.SetBool("IsDrawingBow", isDrawingBow);
            }
        }
        private bool isDrawingBow;

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsStringPulled
        {
            get { return isStringPulled; }
            set
            {
                if (isStringPulled == value) return;
                isStringPulled = value;

                if (isDrawingBow == false) isStringPulled = false;

                weaponBow.IsStringPulled = isStringPulled;
            }
        }
        private bool isStringPulled;

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsArrowDrawn
        {
            get { return isArrowDrawn; }
            set
            {
                isArrowDrawn = value;
                animator.SetBool("IsArrowDrawn", isArrowDrawn);
            }
        }
        private bool isArrowDrawn;

        private Projectile projectile;
        private WeaponBow weaponBow;
        private bool isArrowReady;

        private bool isAttacking;
        private int attackActionIndex;

        public void OnArrowDraw()
        {
            if (weaponBow == null) return;
            if (projectilePrefab == null) return;
            if (isDrawingBow == false) return;

            projectile = Instantiate(projectilePrefab, character.rigHandL).GetComponent<Projectile>();
            projectile.transform.localPosition = Vector3.zero;
            projectile.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            //projectile.transform.localRotation = Quaternion.identity;

            IsArrowDrawn = true;
        }

        public void OnArrowNock()
        {
            IsStringPulled = true;

            onBowPull.Invoke();
        }

        public void OnArrowReady()
        {
            if (isDrawingBow) isArrowReady = true;
        }

        public void OnArrowPutBack()
        {
            IsArrowDrawn = false;

            if (projectile)
            {
                Destroy(projectile.gameObject);
                projectile = null;
            }
        }

        //attack event - throwing the weapon out
        public void OnThrow()
        {
            if (character.Weapon == null) return;

            var weapon = character.DetachWeapon().GetComponent<Rigidbody2D>();

            weapon.velocity = velocity;
            weapon.angularVelocity = -facing * throwAngularSpeed;

            weapon.AddForce(PointAtTargetDirection * throwForce);

            onThrow.Invoke();
        }

        //attack event - cast magic
        public void OnAttackCast()
        {
            if (character.Weapon == null) return;
            if (projectilePrefab == null) return;

            onAttackCast.Invoke();

            if (projectilePrefab == null) return;
            projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = character.Weapon.TipPosition;

            Vector2 dir = IsPointingAtTarget ? PointAtTargetDirection : character.Weapon.transform.right;
            projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Vector2.SignedAngle(Vector2.right, dir));
            projectile.Velocity = projectileSpeed * projectile.transform.right;

            projectile.IsLaunched = true;
        }

        public void ArcheryUpdate()
        {
            if (weaponBow && IsStringPulled) weaponBow.StringPullPos = character.rigHandL.position;
        }

        private void Attack()
        {
            isAttacking = false;
            attackActionIndex = inputMelee ? (int)attackActionMelee : (int)attackAction;

            //archery
            if (attackAction == AttackActionType.Archery)
            {
                if (isCrawling == false && character.Weapon && character.Weapon.TryGetComponent<WeaponBow>(out weaponBow))
                {
                    IsDrawingBow = inputAttack;
                }
                else if (isCrawling)
                {
                    //isAttacking = inputAttack;
                    attackActionIndex = (int)attackActionMelee;
                }
            }
            else if (attackAction == AttackActionType.PointAtTarget || attackAction == AttackActionType.Summon || attackAction == AttackActionType.Throw)
            {
                if (isCrawling) attackActionIndex = (int)attackActionMelee;
            }

            isAttacking = inputAttack || inputMelee;

            //melee attack
            animator.SetInteger("AttackAction", attackActionIndex);
            animator.SetBool("IsAttacking", isAttacking);
        }


        #endregion

        #region - CROUCH -

        private Vector2 CrouchRaycastPosF
        {
            get
            {
                var pos = capsuleCollider2D.bounds.center;
                pos.x += 10 * facing * PIXEL_SIZE;
                pos.y = capsuleCollider2D.bounds.min.y;
                return pos;
            }
        }

        private Vector2 CrouchRaycastPosB
        {
            get
            {
                var pos = capsuleCollider2D.bounds.center;
                pos.x -= 10 * facing * PIXEL_SIZE;
                pos.y = capsuleCollider2D.bounds.min.y;
                return pos;
            }
        }

        private float CrouchRaycastDis
        {
            get
            {
                return COLLIDER_SIZE.y + PIXEL_SIZE;
            }
        }

        public bool CanExitCrouching
        {
            get
            {
                if (isCrouching == false) return true;

                RaycastHit2D hitF = Raycast(CrouchRaycastPosF, Vector2.up, CrouchRaycastDis);
                RaycastHit2D hitB = Raycast(CrouchRaycastPosB, Vector2.up, CrouchRaycastDis);
                return (hitF.collider == null) && (hitB.collider == null);
            }
        }

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsCrouching
        {
            get { return isCrouching; }
        }


        private bool isCrouching;
        private bool shouldCrouch;

        private void Crouch()
        {
            shouldCrouch = inputCrounch;
            if (airTimer > 1.0f) shouldCrouch = false;

            //if there is no enough space to stand, keep crouching
            if (isCrouching)
            {
                shouldCrouch = shouldCrouch || (!CanExitCrouching);
            }

            if (isCrouching != shouldCrouch)
            {
                isCrouching = shouldCrouch;
                colliderDirty = true;
            }
        }
        #endregion

        #region - CRAWL -

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsCrawling
        {
            get
            {
                return isCrawling;
            }
        }

        private Vector2 CrawlExitRaycastPos
        {
            get
            {
                return new Vector2(transform.position.x, transform.position.y + 10 * PIXEL_SIZE);
            }
        }

        private float CrawlExitRaycastDis
        {
            get
            {
                return 37 * PIXEL_SIZE;
            }
        }

        private Vector2 CrawlEnterRaycastPos
        {
            get
            {
                return new Vector2(transform.position.x, transform.position.y + 16 * PIXEL_SIZE);
            }
        }

        private float CrawlEnterRaycastDis
        {
            get
            {
                return 32 * PIXEL_SIZE;
            }
        }

        //check if there is enough space to crawl
        public bool CanEnterCrawling
        {
            get
            {
                if (isCrawling == true) return true;

                RaycastHit2D hitL = Raycast(CrawlEnterRaycastPos, Vector2.left, CrawlEnterRaycastDis);
                RaycastHit2D hitR = Raycast(CrawlEnterRaycastPos, Vector2.right, CrawlEnterRaycastDis);

                float dis = 0.0f;
                if (hitL.collider != null) dis += hitL.distance;
                else dis += 1.0f;
                if (hitR.collider != null) dis += hitR.distance;
                else dis += 1.0f;

                return dis > 0.9f;
            }
        }

        //check if there is enough space to get up from crawling
        public bool CanExitCrawling
        {
            get
            {
                if (isCrawling == false) return true;

                RaycastHit2D hit = Raycast(CrawlExitRaycastPos, Vector2.up, CrawlExitRaycastDis);
                return (hit.collider == null);
            }
        }

        private bool isCrawling;
        private bool shouldCrawl;
        private bool isCrawlEntering;
        private bool isCrawlExiting;

        private void Crawl()
        {
            shouldCrawl = inputCrawl;
            if (airTimer > 1.0f) shouldCrawl = false;

            //if there is no enough space to get up, keep craling
            if (isCrawling)
            {
                shouldCrawl = shouldCrawl || (!CanExitCrawling);
            }


            if (isCrawling != shouldCrawl)
            {
                isCrawling = shouldCrawl;
                colliderDirty = true;

                if (isCrawling) IsDrawingBow = false;
            }

            animator.SetFloat("CrawlSpeedMul", Mathf.Abs(inputMove.x) * moveDir);
        }

        public void OnCrawlEnter()
        {
            isCrawlEntering = true;
        }
        public void OnCrawlEntered()
        {
            isCrawlEntering = false;
        }
        public void OnCrawlExit()
        {
            isCrawlExiting = true;
        }
        public void OnCrawlExited()
        {
            isCrawlExiting = false;
        }


        #endregion

        #region  - LOOK AT TARGET -

        private static readonly Vector2 HEAD_ROT_RANGE = new Vector2(-15.0f, 20.0f);
        private static readonly Vector2 NECK_ROT_RANGE = new Vector2(-15.0f, 15.0f);
        private static readonly Vector2 SPINE2_ROT_RANGE = new Vector2(-15.0f, 15.0f);
        private static readonly Vector2 SPINE1_ROT_RANGE = new Vector2(-15.0f, 15.0f);

        private const float NECK_ROT_PERCENT = 0.5f;
        private const float SPINE2_ROT_PERCENT = 0.3f;
        private const float SPINE1_ROT_PERCENT = 0.3f;
        private const float LOOK_AT_TARGET_LERP_SPEED = 7.5f;

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsLookingAtTarget
        {
            get { return isLookingAtTarget; }
            set
            {
                if (isLookingAtTarget == value) return;
                isLookingAtTarget = value;

                if (isLookingAtTarget == true)
                {
                    targetNeckRot = character.rigNeck.localRotation.eulerAngles.z;
                    targetHeadRot = character.rigHead.localRotation.eulerAngles.z;
                }
            }
        }
        private bool isLookingAtTarget;

        //the look at target override facing
        public int LookAtTargetFacing
        {
            get
            {
                int f = Mathf.RoundToInt(Mathf.Sign(inputTarget.x - character.transform.position.x));
                if (f == 0) f = facing;
                return f;
            }
        }

        //should the look at target also affect the spine rotation
        //only disabled when crawling
        private bool EnableSpineRot
        {
            get
            {
                return !animator.GetBool("IsCrawling");
            }
        }

        private float lookAtTargetPercent;

        private Vector3 spine1Dir;
        private float targetSpine1Rot;

        private Vector3 spine2Dir;
        private float targetSpine2Rot;

        private Vector3 neckDir;
        private float targetNeckRot;

        private Vector3 headDir;
        private float targetHeadRot;

        private Vector3 rot;
        private float pow;

        private float spine1Rot;
        private float spine2Rot;

        private void LookAtTarget()
        {
            //check should turn on looking at target or not
            bool shouldLookAtTarget = false;
            if (IsDrawingBow) shouldLookAtTarget = true;
            if (inputLook) shouldLookAtTarget = true;
            if (inputAttack && attackAction == AttackActionType.PointAtTarget) shouldLookAtTarget = true;
            if (inputAttack && attackAction == AttackActionType.Throw) shouldLookAtTarget = true;
            if (inputAttack && attackAction == AttackActionType.Cast) shouldLookAtTarget = true;
            if (isDead) shouldLookAtTarget = false;
            //if (isDodging) shouldLookAtTarget = false;

            IsLookingAtTarget = shouldLookAtTarget;


            lookAtTargetPercent = Mathf.Lerp(lookAtTargetPercent, isLookingAtTarget ? 1.0f : 0.0f, LOOK_AT_TARGET_LERP_SPEED * Time.deltaTime);

            if (EnableSpineRot)
            {
                //spine1
                //spine1 also use neck dir
                spine1Dir = character.rigNeck.InverseTransformPoint(inputTarget) - character.rigNeck.localPosition;
                spine1Dir.z = 0.0f;
                targetSpine1Rot = Vector2.Angle(Vector2.right, spine1Dir) - 90.0f;
                pow = Mathf.Abs(targetSpine1Rot / 45.0f);
                targetSpine1Rot = Mathf.Clamp(targetSpine1Rot, SPINE1_ROT_RANGE.x, SPINE1_ROT_RANGE.y);

                rot = character.rigSpine1.localRotation.eulerAngles;
                spine1Rot = Mathf.LerpAngle(rot.z, targetSpine1Rot, pow * lookAtTargetPercent * SPINE1_ROT_PERCENT);
                rot.z = spine1Rot;
                character.rigSpine1.localRotation = Quaternion.Euler(rot);

                //spine2
                //spine2 also use neck dir
                spine2Dir = character.rigNeck.InverseTransformPoint(inputTarget) - character.rigNeck.localPosition;
                spine2Dir.z = 0.0f;
                targetSpine2Rot = Vector2.Angle(Vector2.right, spine2Dir) - 90.0f;
                pow = Mathf.Abs(targetSpine2Rot / 45.0f);
                targetSpine2Rot = Mathf.Clamp(targetSpine2Rot, SPINE2_ROT_RANGE.x, SPINE2_ROT_RANGE.y);

                rot = character.rigSpine2.localRotation.eulerAngles;
                spine2Rot = Mathf.LerpAngle(rot.z, targetSpine2Rot, pow * lookAtTargetPercent * SPINE2_ROT_PERCENT);
                rot.z = spine2Rot;
                character.rigSpine2.localRotation = Quaternion.Euler(rot);
            }

            //neck
            neckDir = character.rigNeck.InverseTransformPoint(inputTarget) - character.rigNeck.localPosition;
            neckDir.z = 0.0f;
            targetNeckRot = Vector2.Angle(Vector2.right, neckDir) - 90.0f;
            targetNeckRot = Mathf.Clamp(targetNeckRot, NECK_ROT_RANGE.x, NECK_ROT_RANGE.y);

            rot = character.rigNeck.localRotation.eulerAngles;
            rot.z = Mathf.LerpAngle(rot.z, targetNeckRot, lookAtTargetPercent * NECK_ROT_PERCENT);
            character.rigNeck.localRotation = Quaternion.Euler(rot);

            //head
            headDir = character.rigHead.InverseTransformPoint(inputTarget) - character.rigHead.localPosition;
            headDir.z = 0.0f;
            targetHeadRot = Vector2.Angle(Vector2.right, headDir) - 90.0f;
            targetHeadRot = Mathf.Clamp(targetHeadRot, HEAD_ROT_RANGE.x, HEAD_ROT_RANGE.y);

            rot = character.rigHead.localRotation.eulerAngles;
            rot.z = Mathf.LerpAngle(rot.z, targetHeadRot, lookAtTargetPercent);
            character.rigHead.localRotation = Quaternion.Euler(rot);
        }


        #endregion

        #region - POINT AT TARGET - 

        private const float POINT_AT_TARGET_LERP_SPEED = 7.5f;
        private static readonly Vector2 ARM_ROT_RANGE = new Vector2(-80.0f, 70.0f);


        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsPointingAtTarget
        {
            get { return isPointingAtTarget; }
            set
            {
                if (isPointingAtTarget == value) return;
                isPointingAtTarget = value;

                targetArmRot = 0.0f;
            }
        }
        private bool isPointingAtTarget;

        public Vector2 PointAtTargetDirection
        {
            get
            {
                return (inputTarget - (Vector2)character.rigUpperArmR.position).normalized;
            }
        }

        private float targetArmRot;

        private float pointAtTargetPercent;

        private void PointAtTarget()
        {
            bool shouldPointAtTarget = false;
            if (IsDrawingBow) shouldPointAtTarget = true;
            if (inputAttack && attackAction == AttackActionType.PointAtTarget) shouldPointAtTarget = true;
            if (inputAttack && attackAction == AttackActionType.Throw) shouldPointAtTarget = true;
            if (inputAttack && attackAction == AttackActionType.Cast) shouldPointAtTarget = true;
            if (isCrawling) shouldPointAtTarget = false;
            if (isDodging) shouldPointAtTarget = false;
            if (isDead) shouldPointAtTarget = false;

            IsPointingAtTarget = shouldPointAtTarget;


            pointAtTargetPercent = Mathf.Lerp(pointAtTargetPercent, isPointingAtTarget ? 1.0f : 0.0f, POINT_AT_TARGET_LERP_SPEED * Time.deltaTime);

            targetArmRot = Vector2.Angle(Vector2.up, PointAtTargetDirection) - 90.0f;

            targetArmRot = Mathf.Clamp(targetArmRot, ARM_ROT_RANGE.x, ARM_ROT_RANGE.y);
            targetArmRot = targetArmRot + spine1Rot + spine2Rot + character.rigPelvis.transform.localRotation.eulerAngles.z;

            rot = character.rigUpperArmR.rotation.eulerAngles;
            rot.z += Mathf.LerpAngle(0.0f, targetArmRot, pointAtTargetPercent);
            character.rigUpperArmR.rotation = Quaternion.Euler(rot);

            if (IsDrawingBow)
            {
                rot = character.rigUpperArmL.rotation.eulerAngles;
                rot.z += Mathf.LerpAngle(0.0f, targetArmRot, pointAtTargetPercent);
                character.rigUpperArmL.rotation = Quaternion.Euler(rot);
            }
        }

        #endregion

        #region - MOVE -

        private static readonly float MOVE_THRESHOLD = 0.05f;                  // when move input value is bigger than this then it is considered there is a movement input
        private static readonly float AIR_VELOCITY_Y_LIMIT = 20.0f;

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsMoving
        {
            get { return isMoving; }
        }

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsRunning
        {
            get { return isRunning; }
        }


        private bool isMoving;                                      // is the character moving
        private bool isRunning;                                     // is the character running
        private Vector2 velocity;                                   // current velocity
        private int moveDir = 1;                                    // move direction, 1: forward, -1:backward
        private float moveBlend;                                    // current move blend, for blending idle, walk, run animation, lerps to target move blend on frame update
        private float targetMoveBlend;                              // target move blend

        private void MoveCheck()
        {
            //set isMoving and isRunning
            isMoving = (Mathf.Abs(inputMove.x) > MOVE_THRESHOLD);
            if (isMoving == false) return;

            isRunning = inputRun;

            //disallow running backward
            if (IsLookingAtTarget && Mathf.Sign(inputMove.x) != facing) isRunning = false;

            //is the character moving backward
            moveDir = (Mathf.Sign(inputMove.x) * facing) == 1 ? 1 : -1;
        }

        private void Move()
        {
            if (isClimbingLadder) return;
            if (isClimbingLedge) return;

            //set acceleration and max speed base on condition
            float acc = 0.0f;
            float max = 0.0f;
            float dragAcc = 0.0f;

            if (isGrounded)
            {
                acc = isRunning ? runAcc : walkAcc;
                max = isRunning ? runSpeedMax : walkSpeedMax;
                dragAcc = groundDrag;

                if (isCrouching)
                {
                    acc = crouchAcc;
                    max = crouchSpeedMax;
                }
                if (isCrawling || isCrawlEntering || isCrawlExiting)
                {
                    acc = crawlAcc;
                    max = crawlSpeedMax;
                }
                if (isDashing)
                {
                    acc = dashAcc;
                    max = dashSpeedMax;
                }

                //limit max speed base on surface angle
                float targetSurfaceSpeedMul = Mathf.Sin(Mathf.Min(surfaceAngleForward, 90.0f) * Mathf.Deg2Rad);
                if (targetSurfaceSpeedMul < 1.0f) surfaceSpeedMul = Mathf.Lerp(surfaceSpeedMul, targetSurfaceSpeedMul, 1.0f * Time.fixedDeltaTime);
                else surfaceSpeedMul = 1.0f;
                max *= surfaceSpeedMul;
            }
            else
            {
                acc = airAcc;
                max = airSpeedMax;
                dragAcc = airDrag * Mathf.Abs(velocity.x);

                if (isCrouching)
                {
                    acc = crouchAcc;
                    max = crouchSpeedMax;
                    dragAcc = groundDrag;
                }
                if (isCrawling || isCrawlEntering || isCrawlExiting)
                {
                    acc = crawlAcc;
                    max = crawlSpeedMax;
                    dragAcc = groundDrag;
                }
            }

            if (isDead) inputMove.x = 0.0f;
            bool shouldMove = Mathf.Abs(inputMove.x) > MOVE_THRESHOLD;

            //speed limit
            //on ground
            if (isGrounded)
            {
                float speed = velocity.magnitude;
                if (Mathf.Abs(inputMove.x) > MOVE_THRESHOLD && speed > max) speed = Mathf.MoveTowards(speed, max, dragAcc * Time.fixedDeltaTime);
                if (Mathf.Abs(inputMove.x) <= MOVE_THRESHOLD) speed = Mathf.MoveTowards(speed, 0.0f, dragAcc * Time.fixedDeltaTime);

                velocity = velocity.normalized * speed;
            }
            //in air, set limit to x and y direction separately
            else
            if ( IsInAir)
            {
                float speedX = Mathf.Abs(velocity.x);
                if (speedX > max) speedX = Mathf.MoveTowards(speedX, max, dragAcc * Time.fixedDeltaTime);
                velocity.x = Mathf.Sign(velocity.x) * speedX;

                float speedY = Mathf.Abs(velocity.y);
                if (speedY > AIR_VELOCITY_Y_LIMIT) speedY = AIR_VELOCITY_Y_LIMIT;
                velocity.y = Mathf.Sign(velocity.y) * speedY;
            }

            //force moving when dashing
            if (isDashing) shouldMove = true;

            //cancel shouldMove if the max speed limit is reached at left or right direction
            if (shouldMove)
            {
                if (inputMove.x > 0.0f && velocity.x > max) shouldMove = false;
                else
                if (inputMove.x < 0.0f && velocity.x < -max) shouldMove = false;
            }

            //apply acceleration
            if (shouldMove)
            {
                if (isGrounded)
                {
                    velocity += acc * moveDir * Time.fixedDeltaTime * surfaceDir;
                }
                else
                {
                    velocity += acc * facing * moveDir * Time.fixedDeltaTime * Vector2.right;
                }
            }
        }

        private void UpdateMoveBlend()
        {
            if (isMoving)
            {
                targetMoveBlend = 1.0f;
                if (isRunning) targetMoveBlend = 3.0f;
            }
            else
            {
                targetMoveBlend = 0.0f;
            }

            moveBlend = Mathf.Lerp(moveBlend, targetMoveBlend, 7.0f * Time.deltaTime);
        }

        #endregion

        #region - DASH - 

        private bool isDashing;
        private float dashTimer;
        private float dashCdTimer;

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsDashing
        {
            get
            {
                return isDashing;
            }
        }

        public void Dash()
        {
            if (dashEnabled == false || isDead || isPointingAtTarget || isPointingAtTarget )
            {
                isDashing = false;
                return;
            }

            if (dashTimer > 0.0f)
            {
                dashTimer -= Time.deltaTime;
                if (dashTimer < 0.0f) isDashing = false;
            }
            if (dashCdTimer > 0.0f) dashCdTimer -= Time.deltaTime;

            if (inputDash == false) return;
            inputDash = false;

            if (isGrounded == false || isCrouching == true)
            {
                inputDash = false;
                return;
            }

            if (dashCdTimer > 0.01f) return;


            velocity.x = dashSpeedStart * facing;
            rb2d.velocity = velocity;

            isDashing = true;
            dashTimer = dashTime;
            dashCdTimer = dashCooldown;
        }

        #endregion

        #region - DODGE -

        private float dodgeCdTimer;
        private int dodgeDir;                   //dodge direction   1: front  -1:back
        private int dodgeFacing;

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsDodging
        {
            get
            {
                return isDodging;
            }
            set
            {
                if (isDodging == value) return;
                isDodging = value;

                //ender dodging
                if (isDodging)
                {
                    if (isDashing) isDashing = false;

                    dodgeCdTimer = dodgeCooldown;
                    dodgeFacing = facing;
                }
                else
                {
                    //if dodge into a place where there is only space for crawling
                    if (CanExitCrawling) isCrawling = true;
                }

                colliderDirty = true;

                animator.SetBool("IsDodging", isDodging);
                animator.SetInteger("DodgeDir", dodgeDir);
            }
        }
        private bool isDodging;

        public void OnDodgeStart()
        {
            onDodgeStart.Invoke();
        }

        public void OnDodgeEnd()
        {
            IsDodging = false;

            onDodgeEnd.Invoke();
        }

        private void DodgeCheck()
        {
            if (isDodging)
            {
                if (airTimer > 0.2f) IsDodging = false;
            }

            if (dodgeEnabled == false) return;
            if (isCrawling || isClimbingLedge || isEnteringLadder || isExitingLadder ) return;


            if (IsDodging == false && dodgeCdTimer > 0.01f)
            {
                dodgeCdTimer -= Time.deltaTime;
                return;
            }

            if (airTimer > 0.1f) return;

            if (inputDodge == false) return;
            inputDodge = false;

            if (Mathf.Abs(inputMove.x) < MOVE_THRESHOLD) return;

            //set dodge dir
            if (inputMove.x * facing > 0) dodgeDir = 1;
            else dodgeDir = -1;

            IsDodging = true;
        }

        private void DodgeUpdate()
        {
            //snap character to ground better when dodging
            if ( isDodging && airTimer > 0.01f)
            {
                velocity.y -= 25.0f * Time.fixedDeltaTime;
            }
        }

        #endregion

        #region - GET DOWN PLATFORM -
        //when player is standing on a one-way platform and has down direction input
        //get down this platform
        private void GetDownPlatform()
        {
            if (IsStandingOnPlatform == false) return;
            if (inputMove.y >= -MOVE_THRESHOLD) return;

            getDownPlatformTimer += Time.deltaTime;

            if (getDownPlatformTimer > 0.1f)
            {
                getDownPlatformTimer = 0.0f;
                foreach (var c in standingColliders)
                {
                    if (c.gameObject.TryGetComponent<Platform>(out _)) ignoredPlatforms.Add(c);
                }

                animator.SetTrigger("GetDownPlatform");
            }
        }

        private void RevertIgnoredPlatforms()
        {
            var checkList = new List<Collider2D>();
            checkList.Add(Raycast(GroundRaycastPosF, Vector2.down, GroundRaycastDis, false, false).collider);
            checkList.Add(Raycast(GroundRaycastPosM, Vector2.down, GroundRaycastDis, false, false).collider);
            checkList.Add(Raycast(GroundRaycastPosB, Vector2.down, GroundRaycastDis, false, false).collider);

            for (int i = 0; i < ignoredPlatforms.Count; i++)
            {
                if (checkList.Contains(ignoredPlatforms[i]) == false) ignoredPlatforms.RemoveAt(i);
            }
        }
        #endregion

        #region - JUMP -

        //the actual jump cooldown, used for settings a minimal jump cooldown value, as it can not be too small
        private float JumpCoolDown
        {
            get { return Mathf.Max(0.05f, jumpCooldown); }
        }

        private void StartJumpCheck()
        {
            if (jumpEnabled == false || IsDead) return;

            //disable jump while crawling or dodging
            if (isCrawling || isCrawlEntering || isCrawlExiting || isDodging )
            {
                jumpCdTimer = 0.0f;
                return;
            }

            //jump cooldown
            if (IsInAir == false && jumpCdTimer < JumpCoolDown) jumpCdTimer += Time.deltaTime;

            //start jump
            if (inputJump && jumpCdTimer >= JumpCoolDown)
            {
                //jump from ground
                //also able to jump within air time tolerance
                if (isGrounded || (0 < airTimer && airTimer <= jumpTolerance))
                {
                    IsGrounded = false;
                    IsClimbingLadder = false;

                    jumpCdTimer = 0.0f;

                    //mix surface normal to jump direction
                    Vector2 jumpDir = Vector2.up;
                    float surfaceNormalMix = Mathf.Lerp(0.0f, 1.0f, surfaceAngle / 90.0f);
                    jumpDir = Vector2.Lerp(Vector2.up, surfaceNormal, surfaceNormalMix).normalized;
                    startJumpVel = jumpSpeed * jumpDir;
                }

                //jump from ladder
                if (IsClimbingLadder)
                {
                    IsGrounded = false;
                    isEnteringLadder = false;
                    isExitingLadder = false;
                    IsClimbingLadder = false;

                    jumpCdTimer = 0.0f;

                    //mix ladder direction or move direction to jump direction
                    Vector2 jumpDir = Vector2.up;
                    if (Mathf.Abs(inputMove.x) < MOVE_THRESHOLD)
                    {
                        jumpDir += new Vector2((int)ladder.direction, 0.0f) * 0.25f;
                    }
                    else
                    {
                        jumpDir += new Vector2(Mathf.Sign(inputMove.x), 0.0f) * 0.5f;
                    }
                    jumpDir = jumpDir.normalized;
                    startJumpVel = jumpSpeed * jumpDir;


                }

                //jump while entering or exiting climbing 
                if ( isEnteringLadder || isExitingLadder)
                {
                    IsGrounded = false;
                    isEnteringLadder = false;
                    isExitingLadder = false;
                    IsClimbingLadder = false;

                    jumpCdTimer = 0.0f;

                    Vector2 jumpDir = Vector2.up;
                    jumpDir += new Vector2(-facing, 0.0f) * 0.5f;
                    jumpDir = jumpDir.normalized;

                    startJumpVel = jumpSpeed * jumpDir;

                }

                //jump while climbing ledge
                if ( isClimbingLedge || ledgeClimbLocked )
                {
                    IsGrounded = false;
                    isClimbingLedge = false;
                    ledgeClimbLocked = false;
                    jumpCdTimer = 0.0f;

                    Vector2 jumpDir = Vector2.up;
                    jumpDir += new Vector2(-facing, 0.0f) * 0.5f;
                    jumpDir = jumpDir.normalized;

                    startJumpVel = jumpSpeed * jumpDir;
                }
            }

        }

        private void JumpUpdate()
        {
            if (jumpEnabled == false) return;

            //apply start jump vel
            if (startJumpVel.magnitude > 0.01f)
            {
                Vector2 jumpDir = startJumpVel.normalized;
                float dot = Vector2.Dot(velocity, jumpDir);
                if (dot < 0) velocity -= dot * jumpDir;

                velocity += startJumpVel;
                if (velocity.y > startJumpVel.y * 1.25f) velocity.y = startJumpVel.y * 1.25f;
                startJumpVel = Vector2.zero;

                //apply jump force to standing collider
                Vector2 force = jumpSpeed * CHARACTER_WEIGHT * Physics2D.gravity / standingColliders.Count;
                for (int i = 0; i < standingColliders.Count; i++)
                {
                    if (standingColliders[i].attachedRigidbody) standingColliders[i].attachedRigidbody.AddForceAtPosition(force, standingPosList[i]);
                }

                //event
                onJump.Invoke();
            }


            //jumping up with continuous jump input
            //set jump gravity so that the longer the jump key is pressed, the higher the character can jump
            if ( IsInAir )
            {
                if (inputJump && velocity.y > 0)
                {
                    velocity.y += Physics2D.gravity.y * (jumpGravityMutiplier - 1.0f) * Time.fixedDeltaTime;
                }
                //jumping up without input
                else if (velocity.y > 0.01f)
                {
                    velocity.y += Physics2D.gravity.y * (fallGravityMutiplier - 1.0f) * Time.fixedDeltaTime;
                }
            }
        }

        #endregion

        #region - LADDER - 

        // the ladder the character is climbing, if there is;      
        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public Ladder Ladder
        {
            get
            {
                return ladder;
            }
        }
        private Ladder ladder;

        //is the character climbing ladder
        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsClimbingLadder
        {
            get
            {
                return isClimbingLadder;
            }
            set
            {
                if (isClimbingLadder == value) return;
                isClimbingLadder = value;

                //set ladder climb z pos
                if (isClimbingLadder)
                {
                    Vector3 pos = transform.position;
                    ladderExitPosZ = pos.z;
                    pos.z = ladder.ClimbPos.z;
                    transform.position = pos;
                }
                else
                {
                    Vector3 pos = transform.position;
                    pos.z = ladderExitPosZ;
                    transform.position = pos;
                }
            }
        }
        private bool isClimbingLadder;

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsEnteringLadder
        {
            get { return isEnteringLadder; }
        }
        private bool isEnteringLadder;

        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsExitingLadder
        {
            get { return isExitingLadder; }
        }
        private bool isExitingLadder;


        private float climbingSpeedMul;
        private bool hasReachedLadderTop;
        private bool hasReachedLadderBottom;
        private float ladderEnterHeight;
        private float ladderExitHeight;                              //y distance from the button of the character to the surface when exiting from ladder
        private float ladderToAirTimer;
        private float ladderExitPosZ;                                 // origin position z of the character, used for resetting the position z after exiting the ladder

        private void LadderClimb()
        {
            if (isDead) return;

            if (isExitingLadder)
            {
                gravityScale = 0.0f;
            }

            if (isClimbingLadder == false) return;

            gravityScale = 0.0f;
            velocity.x = 0.0f;

            //handle climb movement
            velocity.x = 0.0f;
            if (Mathf.Abs(inputMove.y) > MOVE_THRESHOLD)
            {
                velocity.y = ( inputRun ? ladderClimbSpeedFast : ladderClimbSpeed) * Mathf.Sign(inputMove.y);
                climbingSpeedMul = (velocity.y / ladderClimbSpeed);
            }
            else
            {
                velocity.y = 0.0f;
                climbingSpeedMul = 0.0f;
            }


            //reach top of the ladder
            hasReachedLadderTop = (NeckPos.y >= ladder.TopPos.y);
            if (hasReachedLadderTop)
            {
                if (velocity.y > 0.0f)
                {
                    velocity.y = 0.0f;
                    climbingSpeedMul = 0.0f;
                }
            }

            //reach top bottom of the ladder
            //using 2 pixel size as tolerance
            hasReachedLadderBottom = (Mathf.Abs (BottomPos.y - ladder.BottomPos.y) < PIXEL_SIZE * 2.0f);
            if (hasReachedLadderBottom)
            {
                if (velocity.y < 0.0f)
                {
                    velocity.y = 0.0f;
                    climbingSpeedMul = 0.0f;
                }
            }

            //move character to the cimbing position defined by the ladder
            Vector3 pos = transform.position;
            pos.x = Mathf.Lerp(pos.x, ladder.ClimbPos.x, Time.fixedDeltaTime * CLIMB_POS_LERP_SPEED);
            transform.position = pos;

            //as entering ladder animation's root motion somehow don't work
            //manual add down input
            if (isEnteringLadder)
            {
                velocity.y = (isRunning ? ladderClimbSpeedFast : ladderClimbSpeed) * -1.0f;
                climbingSpeedMul = (velocity.y / ladderClimbSpeed);
            }
        }

        private void LadderExitCheck()
        {
            if (IsDead) return;
            if (IsClimbingLadder == false) return;
            if (isEnteringLadder) return;

            //reach ladder top, has up input or has forward input
            if ((hasReachedLadderTop && inputMove.y > MOVE_THRESHOLD) ||
                (Mathf.Abs(inputMove.x) > MOVE_THRESHOLD && (inputMove.x * facing) > 0))
            {
                //check for surface the character can climb onto
                RaycastHit2D hit = Raycast(LadderExitRaycastPos, Vector2.down, LadderExitRaycastDis);
                bool hasHit = (hit.collider != null);

                //have climbable surface
                if (hasHit)
                {
                    isExitingLadder = true;
                    IsClimbingLadder = false;
                    ladderExitHeight = hit.point.y - transform.position.y;
                }
            }

            //reach ladder bottom, grounded, have down input
            if (hasReachedLadderBottom && inputMove.y < -MOVE_THRESHOLD && isGrounded)
            {
                IsClimbingLadder = false;
            }

            //special case, from ladder to air
            //reach ladder bottom, not grounded, down input persist more than 0.5s
            if ( hasReachedLadderBottom && isGrounded == false && inputMove.y < -MOVE_THRESHOLD )
            {
                ladderToAirTimer += Time.deltaTime;
                if (ladderToAirTimer > 0.5f)
                {
                    ladderToAirTimer = 0.0f;
                    ladder = null;
                    IsClimbingLadder = false;

                    animator.SetTrigger("GetDownPlatform");
                }
            }
        }

        private void LadderEnterCheck()
        {
            if (IsClimbingLadder) return;
            if (ladder == null) return;
            if (Mathf.Abs(inputMove.x) > MOVE_THRESHOLD) return;
            if (isDrawingBow) return;
            if (isCrawling || isCrawlEntering || isCrawlExiting) return;

            //climb up to ladder
            if (inputMove.y > MOVE_THRESHOLD && NeckPos.y < ladder.TopPos.y && CenterPos.y > ladder.BottomPos.y) IsClimbingLadder = true;

            //climb down to ladder, may needs ladder enter animation
            if (inputMove.y < -MOVE_THRESHOLD && BottomPos.y > ladder.BottomPos.y)
            {
                IsClimbingLadder = true;
                ladderEnterHeight = LadderExitRaycastPos.y - ladder.TopPos.y;
                if (ladderEnterHeight > -Mathf.Epsilon && ladderEnterHeight < 41 * PIXEL_SIZE)
                {
                    isEnteringLadder = true;
                }
            }
        }

        public void OnLadderEntered()
        {
            isEnteringLadder = false;
        }

        public void OnLadderExited()
        {
            isExitingLadder = false;
        }

        #endregion

        #region - LEDGE CLIMB -

        public Collider2D LedgeCollider
        {
            get { return ledgeCollider; }
        }


        private bool isClimbingLedge;                               //is the character climbing ledge
        private bool ledgeClimbLocked;
        private float ledgeHeight;
        private Vector2 ledgePos;
        private Collider2D ledgeCollider;
        private Vector2 ledgeNormal;
        private bool ledgeAvailable;

        private void LedgeClimbCheck()
        {
            if (isDead || ledgeClimbEnabled == false || isClimbingLadder || isExitingLadder || isDrawingBow || isCrawling || isDodging)
            {
                isClimbingLedge = false;
                return;
            }

            //ledge climb enter check 
            if (isClimbingLedge == false)
            {
                if (Mathf.Abs(inputMove.x) < MOVE_THRESHOLD || Mathf.Sign(inputMove.x) != facing) return;

                //check for climbable ledge
                RaycastHit2D hit = Raycast(LedgeClimbRaycastPos, Vector2.down, LedgeClimbRaycastDis, true);
                ledgePos = hit.point;
                ledgeNormal = hit.normal;
                ledgeCollider = hit.collider;

                //check up direction for enough space to climb
                bool UpAvailable = true;
                if (ledgeCollider != null)
                {
                    Vector2 upCheckPos = ledgePos - new Vector2(0.0f, PIXEL_SIZE);
                    RaycastHit2D hitUp = Raycast(upCheckPos, Vector2.up, 32 * PIXEL_SIZE, true);
                    UpAvailable = (hitUp.collider == null);
                }

                ledgeAvailable = (ledgeCollider != null) && UpAvailable;

                //start climbing ledge
                if (ledgeAvailable && isClimbingLedge == false)
                {
                    isClimbingLedge = true;
                    ledgeHeight = LedgeClimbRaycastHeight - hit.distance;

                    rb2d.velocity = Vector2.zero;
                }

                ledgeClimbLocked = false;
            }
            //ledge climb exit check 
            else
            {

                if (Mathf.Abs(inputMove.x) < MOVE_THRESHOLD && ledgeClimbLocked == false )
                {
                    isClimbingLedge = false;
                    return;
                }

                if (Mathf.Abs(inputMove.x) >= MOVE_THRESHOLD && Mathf.Sign(inputMove.x) != facing)
                {
                    isClimbingLedge = false;
                    return;
                }

                if ( transform.position.y > ledgePos.y )
                {
                    isClimbingLedge = false;
                    return;
                }
            }
        }

        private void LedgeClimb()
        {
            if ( isClimbingLedge)
            {
                //update ledge pos and character pos in case the ledge collider is moving
                RaycastHit2D hit = Raycast(ledgePos + new Vector2 ( 0.0f, PIXEL_SIZE), Vector2.down, PIXEL_SIZE * 4.0f, true);
                if ( hit.collider )
                {
                    transform.position += (Vector3)(hit.point - ledgePos);

                    ledgePos = hit.point;

                }
                //if the ledge collider is no more, cancel ledge climbing
                else
                {
                    isClimbingLedge = false;
                }    
            }
        }

        public void OnLedgeClimbLocked()
        {
            ledgeClimbLocked = true;
        }

        public void OnLedgeClimbFinised()
        {
            isClimbingLedge = false;
            ledgeClimbLocked = false;
        }

        #endregion

        #region - GROUND CHECK & GROUND LIFT

        private static readonly float LAND_EVENT_THRESHOLD = 3.0f;      //the velocity y threshold to triger the onLand event

        //returns the middle collider the character is standing on
        public Collider2D StandingCollider
        {
            get
            {
                if (standingColliders.Count <= 0) return null;
                return standingColliders[0];
            }
        }

        private float groundYPos;                                       // ground y world position
        private List<Collider2D> standingColliders;                     // collider the character is standing on
        private List<Vector2> standingPosList;
        private List<Collider2D> ignoredPlatforms;
        private bool isStandingOnPlatform;                              // is the character standing on a one-way platform


        //raycast parameters for ground check
        private Vector3 GroundRaycastPosF
        {
            get
            {
                return GroundRaycastPosM + new Vector3( (isCrawling ? 15 * PIXEL_SIZE : 6 * PIXEL_SIZE) * facing , 0.0f, 0.0f);
            }
        }
        private Vector3 GroundRaycastPosM
        {
            get
            {
                Vector3 pos = capsuleCollider2D.bounds.center;
                pos.y = transform.position.y + GroundRaycastDis - PIXEL_SIZE;
                return pos;
            }
        }
        private Vector3 GroundRaycastPosB
        {
            get
            {
                return GroundRaycastPosM + new Vector3((isCrawling ? 15 * PIXEL_SIZE : 6 * PIXEL_SIZE) * -facing, 0.0f, 0.0f);
            }
        }
        private float GroundRaycastDis
        {
            get
            {
                return 18 * PIXEL_SIZE;
            }
        }

        // is the character on ground
        [FoldoutGroup("Runtime"), ShowInInspector, ReadOnly]
        public bool IsGrounded
        {
            get { return isGrounded; }
            private set
            {
                if (isGrounded == value) return;
                isGrounded = value;

                //ground event
                if (isGrounded && velocity.y < -LAND_EVENT_THRESHOLD) onLand.Invoke();
            }
        }
        private bool isGrounded;


        //check if the character is on ground
        private void GroundCheck()
        {
            //get raycast results
            RaycastHit2D raycastHitL = Raycast(GroundRaycastPosF, Vector2.down, GroundRaycastDis, false, true);
            RaycastHit2D raycastHitM = Raycast(GroundRaycastPosM, Vector2.down, GroundRaycastDis, false, true);
            RaycastHit2D raycastHitR = Raycast(GroundRaycastPosB, Vector2.down, GroundRaycastDis, false, true);

            bool hit = raycastHitL.collider || raycastHitM.collider || raycastHitR.collider;
            groundYPos = Mathf.Max(raycastHitL.point.y, raycastHitM.point.y, raycastHitR.point.y);

            //set standing collider and position
            standingPosList.Clear();
            standingColliders.Clear();
            if (raycastHitM.collider != null)
            {
                standingColliders.Add(raycastHitM.collider);
                standingPosList.Add(raycastHitM.point);
            }
            if (raycastHitL.collider != null)
            {
                standingColliders.Add(raycastHitL.collider);
                standingPosList.Add(raycastHitL.point);
            }
            if (raycastHitR.collider != null)
            {
                standingColliders.Add(raycastHitR.collider);
                standingPosList.Add(raycastHitR.point);
            }

            //check is on platform
            bool onPlatform = false;
            if (standingColliders.Count > 0) onPlatform = standingColliders[0].gameObject.TryGetComponent<Platform>(out _);
            for ( int i = 1; i < standingColliders.Count;i++ )
            {
                onPlatform = onPlatform && standingColliders[i].gameObject.TryGetComponent<Platform>(out _);
            }
            isStandingOnPlatform = onPlatform;

            //check grounded
            if (hit && groundYPos >= transform.position.y - PIXEL_SIZE )
            {
                IsGrounded = true;

                RevertIgnoredPlatforms();
            }
            else
            {
                IsGrounded = false;
            }
            gravityScale = isGrounded ? 0.0f : 1.0f;

            //caculate surface normal, direction and slope angle
            //find the one closed to Vector2.up
            Vector2 normal = Vector2.up;
            if (isGrounded)
            {
                normal = raycastHitL.normal;
                float angle = 90.0f;
                var angleL = Vector2.Angle(raycastHitL.normal, Vector2.up);
                if (raycastHitL.collider && angleL < angle)
                {
                    angle = angleL;
                    normal = raycastHitL.normal;
                }
                var angleM = Vector2.Angle(raycastHitM.normal, Vector2.up);
                if (raycastHitM.collider && angleM < angle)
                {
                    angle = angleM;
                    normal = raycastHitM.normal;
                }
                var angleR = Vector2.Angle(raycastHitR.normal, Vector2.up);
                if (raycastHitR.collider && angleR < angle)
                {
                    angle = angleR;
                    normal = raycastHitR.normal;
                }
            }
            surfaceNormal = normal;

            surfaceDir = RotateVector2(surfaceNormal, -90.0f * facing);
            if (surfaceDir.y > 0) surfaceDirDown = -surfaceDir;
            else surfaceDirDown = surfaceDir;
            surfaceAngle = Vector2.Angle(surfaceNormal, Vector2.up);
            surfaceAngleForward = Vector2.Angle(surfaceDir, Vector2.up);

            //cancel velocity parts that is perpendicular to ground surface
            if (isGrounded)
            {
                float dot = Vector2.Dot(velocity, surfaceNormal);
                if (dot < 0) velocity -= dot * surfaceNormal;
            }
        }

        //keep character above ground
        private void GroundLift()
        {
            if (isGrounded == false) return;
            if (isClimbingLadder) return;
            if (isExitingLadder) return;
            if (isClimbingLedge) return;

            //legacy ground lift by setting transform position
            //Vector3 pos = transform.position;
            //pos.y = Mathf.Lerp(pos.y, groundYPos , 7.5f * Time.fixedDeltaTime);
            //transform.position = pos;

            //the speed to lift the character on ground
            groundLiftSpeed = Mathf.Max(0.0f, groundYPos - transform.position.y) * 7.5f;

            //apply force to standing rigidbody
            Vector2 force = CHARACTER_WEIGHT * Physics2D.gravity / standingColliders.Count;
            for ( int i = 0; i < standingColliders.Count; i++ )
            {
                if ( standingColliders[i].attachedRigidbody) standingColliders[i].attachedRigidbody.AddForceAtPosition(force, standingPosList[i]);
            }
        }

        #endregion

        #region - SLIDE -

        private static readonly float SLIDE_SPEED_MAX = 5.0f;                                       // the max speed the character slide down a slope it can not stand on

        private bool isSliding;
        private float slideSpeed;
        private Vector3 slideVel;

        //slide down slope if the angle is too steep
        private void Slide()
        {
            isSliding = true;

            if (isGrounded == false) isSliding = false;
            if (isClimbingLedge) isSliding = false;
            if (surfaceAngle < SURFACE_ANGLE_LIMIT) isSliding = false;

            if (isSliding == false)
            {
                slideSpeed = Mathf.Lerp(slideSpeed, 0.0f, 2.0f * Time.deltaTime);
                return;
            }

            float targetSlideSpeed = Mathf.Lerp(0.0f, 1.0f, surfaceAngle / 90.0f) * SLIDE_SPEED_MAX;
            slideSpeed = Mathf.Lerp(slideSpeed, targetSlideSpeed, 2.0f * Time.deltaTime);
            slideVel = Mathf.Lerp(0.0f, 1.0f, surfaceAngle / 90.0f) * slideSpeed * surfaceDirDown;
            transform.position += slideVel * Time.fixedDeltaTime;
        }
        #endregion

        #region - FACING -

        private int facing = 1;

        private void UpdateFacing()
        {
            //dodge
            if (isDodging)
            {
                facing = dodgeFacing;
                return;
            }

            //look at target
            if (IsLookingAtTarget && !isClimbingLadder && !isEnteringLadder && !isExitingLadder)
            {
                facing = LookAtTargetFacing;
                return;
            }

            //ladder
            if ( isClimbingLadder || isEnteringLadder || isExitingLadder)
            {
                if (ladder) facing = -(int)ladder.direction;
                return;
            }

            //move
            if (Mathf.Abs(inputMove.x) > MOVE_THRESHOLD )
            {
                facing = Mathf.RoundToInt(Mathf.Sign(inputMove.x));
                return;
            }
        }

        #endregion

        #region - COLLIDER - 

        private static Vector2 COLLIDER_OFFSET = new(0.0f, 1.15625f);
        private static Vector2 COLLIDER_SIZE = new(0.375f, 1.3125f);
        private static Vector2 COLLIDER_OFFSET_CROUCH = new(0.0f, 1.0f);
        private static Vector2 COLLIDER_SIZE_CROUCH = new(0.375f, 1.0f);
        private static Vector2 COLLIDER_OFFSET_CRAWL = new(0.1875f, 0.5f);
        private static Vector2 COLLIDER_SIZE_CRAWL = new(1.0f, 0.5f);

        private bool colliderDirty;

        private void UpdateCollider()
        {
            if (colliderDirty)
            {
                Vector2 size = COLLIDER_SIZE;
                Vector2 offset = COLLIDER_OFFSET;

                if (isCrawling || isDodging )
                {
                    size = COLLIDER_SIZE_CRAWL;
                    offset = COLLIDER_OFFSET_CRAWL;
                }
                else if (isCrouching)
                {
                    size = COLLIDER_SIZE_CROUCH;
                    offset = COLLIDER_OFFSET_CROUCH;
                }

                capsuleCollider2D.size = size;
                capsuleCollider2D.offset = offset;
                capsuleCollider2D.direction = isCrawling ? CapsuleDirection2D.Horizontal : CapsuleDirection2D.Vertical;

                colliderDirty = false;
            }

            if ( isCrawling)
            {
                Vector2 offset = COLLIDER_OFFSET_CRAWL;
                offset.x *= facing;
                capsuleCollider2D.offset = offset;
            }
        }

        #endregion

        #region - ROOT MOTION AND ANIMATOR - 

        private void ApplyRootMotion()
        {
            //root motion from dodging
            if ( isDodging)
            {
                if (Mathf.Abs(rootMotionReceiver.rootMotionVel.magnitude) > MOVE_THRESHOLD)
                {
                    velocity.x = rootMotionReceiver.rootMotionVel.x * dodgeSpeedMul;
                    if (velocity.y > 0) velocity.y = 0.0f;
                }
            }

            //root motion form ladder entering & exiting, ledge cimbing
            if ( isExitingLadder || isExitingLadder || isClimbingLedge  )
            {
                if (Mathf.Abs(rootMotionReceiver.rootMotionVel.magnitude) > MOVE_THRESHOLD) velocity = rootMotionReceiver.rootMotionVel;
            }
        }

        private void UpdateAnimator()
        {
            if (isDead) return;

            animator.SetBool("IsMoving", isMoving);
            animator.SetFloat("MoveBlend", moveBlend);
            animator.SetBool("IsRunning", isRunning);
            animator.SetBool("IsDashing", isDashing);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetBool("IsCrouching", isCrouching);
            animator.SetBool("IsCrawling", isCrawling);
            animator.SetFloat("VelocityX", Mathf.Abs(velocity.x));
            animator.SetFloat("VelocityY", velocity.y);
            animator.SetFloat("MoveDir", moveDir);

            animator.SetBool("IsClimbingLadder", isClimbingLadder);
            animator.SetFloat("ClimbingSpeedMul", climbingSpeedMul);

            animator.SetBool("IsClimbingLedge", isClimbingLedge);
            if ( isClimbingLedge) animator.SetFloat("LedgeHeight", ledgeHeight);

            animator.SetBool("IsEnteringLadder", isEnteringLadder);
            animator.SetBool("IsExitingLadder", isExitingLadder);
            if ( isEnteringLadder ) animator.SetFloat("LadderEnterHeight", ladderEnterHeight);
            if ( isExitingLadder) animator.SetFloat("LadderExitHeight", ladderExitHeight);

            character.Facing = facing;
        }

        #endregion

        #region - UNITY CALLBACKS - 

        private void Awake()
        {
            character = GetComponent<PixelCharacter>();
            animator = GetComponentInChildren<Animator>();
            rb2d = GetComponent<Rigidbody2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            rootMotionReceiver = GetComponentInChildren<RootMotionReceiver>();

            standingColliders = new List<Collider2D>();
            standingPosList = new List<Vector2>();
            ignoredPlatforms = new List<Collider2D>();
        }

        private void Start()
        {
            capsuleCollider2D.offset = COLLIDER_OFFSET;
            capsuleCollider2D.size = COLLIDER_SIZE;
        }

        private void Update()
        {
            //air timer
            if (IsInAir) airTimer += Time.deltaTime;
            else airTimer = 0.0f;

            //idle timer
            if (IsIdle) idleTimer += Time.deltaTime;
            else idleTimer = 0.0f;

            MoveCheck();
            Dash();
            DodgeCheck();
            UpdateMoveBlend();
            StartJumpCheck();
            LadderEnterCheck();
            LadderExitCheck();
            LedgeClimbCheck();
            Crouch();
            Crawl();
            GetDownPlatform();
            Attack();
            UpdateFacing();
            ArcheryUpdate();
            UpdateCollider();

            UpdateAnimator();
        }

        private void FixedUpdate()
        {
            velocity = rb2d.velocity - Vector2.up * groundLiftSpeed;
            gravityScale = 1.0f;

            GroundCheck();
            GroundLift();
            Move();
            Slide();
            JumpUpdate();
            LadderClimb();
            LedgeClimb();
            DodgeUpdate();

            ApplyRootMotion();

            rb2d.gravityScale = gravityScale;
            rb2d.velocity = velocity + Vector2.up * groundLiftSpeed;
        }

        private void LateUpdate()
        {
            LookAtTarget();
            PointAtTarget();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<Ladder>(out Ladder ladder))
            {
                if (this.ladder == null) this.ladder = ladder;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<Ladder>(out Ladder ladder))
            {
                if (this.ladder == ladder)
                {
                    this.ladder = null;
                    isClimbingLadder = false;
                }
            }
        }

        #endregion

        #region - EVENT HANDLING -

        //footstep event has a string parameter telling which animation this event is from.
        public void OnFootstep( AnimationEvent evt )
        {
            if (airTimer > 0.1f ) return;

            if (evt.animatorClipInfo.weight < 0.49f) return;

            if (IsClimbingLedge)
            {
                if (evt.stringParameter == "Ledge Climb") onFootstep.Invoke();
                return;
            }

            if (IsCrawling)
            {
                if (evt.stringParameter == "Crawl" ) onFootstep.Invoke();
                return;
            }

            if ( IsCrouching )
            {
                if (evt.stringParameter == "Crouch" ) onFootstep.Invoke();
                return;
            }

            if (evt.stringParameter == "Walk" && moveBlend > 0.1f && moveBlend < 1.1f)
            {
                onFootstep.Invoke();
                return;
            }
            if (evt.stringParameter == "Run" && moveBlend > 1.1f)
            {
                onFootstep.Invoke();
                return;
            }
            if (evt.stringParameter == "Dash")
            {
                onFootstep.Invoke();
                return;
            }
            if (evt.stringParameter == "Ladder Climb" || evt.stringParameter == "Ladder Exit")
            {
                onFootstep.Invoke();
                return;
            }
        }
        
        public void OnAttackStart ()
        {
            onAttackStart.Invoke();
        }

        public void OnAttackHit()
        {
            onAttackHit.Invoke();
        }

        public void OnAttackEnd()
        {
            onAttackEnd.Invoke();
        }

        #endregion

        #region - GIZMOS - 
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //ledge position
            if ( debug_drawLedgePos )
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(ledgePos, 0.1f);
            }

            //ledge raycast
            if (debug_drawLedgeRaycast)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(LedgeClimbRaycastPos, LedgeClimbRaycastPos - new Vector3(0.0f, LedgeClimbRaycastDis, 0.0f));
            }

            //ground check normal
            if (debug_drawGroundCheckRaycast)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(GroundRaycastPosF, GroundRaycastPosF - new Vector3(0.0f, GroundRaycastDis, 0.0f));
                Gizmos.DrawLine(GroundRaycastPosM, GroundRaycastPosM - new Vector3(0.0f, GroundRaycastDis, 0.0f));
                Gizmos.DrawLine(GroundRaycastPosB, GroundRaycastPosB - new Vector3(0.0f, GroundRaycastDis, 0.0f));
            }

            //ground normal
            if (debug_drawGroundNormal && isGrounded)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position - Vector3.forward, transform.position + new Vector3(surfaceNormal.x, surfaceNormal.y) * 2.0f - Vector3.forward);
            }

            //ground direction
            if (debug_drawGroundDir && isGrounded)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position - Vector3.forward, transform.position + new Vector3(surfaceDir.x, surfaceDir.y) * 2.0f - Vector3.forward);
            }

            //crouch raycast
            if (debug_drawCrouchRaycast)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(CrouchRaycastPosF, CrouchRaycastPosF + Vector2.up * CrouchRaycastDis);
                Gizmos.DrawLine(CrouchRaycastPosB, CrouchRaycastPosB + Vector2.up * CrouchRaycastDis);
            }
            //crawl enter raycast
            if (debug_drawCrawlEnterRaycast)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(CrawlEnterRaycastPos, CrawlEnterRaycastPos + Vector2.left * CrawlEnterRaycastDis);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(CrawlEnterRaycastPos, CrawlEnterRaycastPos + Vector2.right * CrawlEnterRaycastDis);
            }

            //crawl exit raycast
            if (debug_drawCrawlExitRaycast)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(CrawlExitRaycastPos, CrawlExitRaycastPos + Vector2.up * CrawlExitRaycastDis);
            }

            //velocity
            if ( debug_drawVelocity)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position - Vector3.forward, transform.position - Vector3.forward + new Vector3(velocity.x, velocity.y, 0.0f));
            }

            //slide velocity
            if (debug_drawSlideVelocity && isSliding)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position - Vector3.forward, transform.position - Vector3.forward + new Vector3(slideVel.x, slideVel.y, 0.0f));
            }

        }
        #endif
        #endregion

        #region - MISC & HELPER FUNCTIONS -
        private RaycastHit2D Raycast( Vector2 origin, Vector2 direction, float distance, bool ignorePlatform = false , bool skipIgnoredPlatforms = true )
        {        
            RaycastHit2D raycastHit = new RaycastHit2D()
            {
                point = origin + direction * distance
            };

            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance, groundCheckLayerMask);

            if (hits.Length <= 0) return raycastHit;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider == null) continue;
                if (hits[i].collider.gameObject == gameObject) continue;
                if (hits[i].collider.isTrigger) continue;
                if (hits[i].fraction < Mathf.Epsilon) continue;
                if (hits[i].collider.usedByEffector)
                {
                    if (ignorePlatform) continue;
                    if (Vector2.Dot(hits[i].transform.up, hits[i].normal) < 0) continue;
                }
                if ( skipIgnoredPlatforms && ignoredPlatforms.Contains(hits[i].collider)) continue;
                if ( Vector3.Distance( hits[i].point, origin ) < Vector3.Distance(raycastHit.point, origin))
                {
                    raycastHit = hits[i];
                }
            }

            return raycastHit;
        }

        private Vector2 RotateVector2( Vector2 v, float d)
        {
            float sin = Mathf.Sin(d * Mathf.Deg2Rad);
            float cos = Mathf.Cos(d * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;

            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);

            return v.normalized;
        }

        public enum AttackActionType
        {
            None = 0,

            Swipe = 1,
            Stab = 2,

            PointAtTarget = 11,
            Summon = 12,
            Throw = 13,
            Cast = 14,

            Archery = 21
        }

        public enum AttackActionMeleeType
        {
            None = 0,

            Swipe = 1,
            Stab = 2
        }
    }
    #endregion
}



