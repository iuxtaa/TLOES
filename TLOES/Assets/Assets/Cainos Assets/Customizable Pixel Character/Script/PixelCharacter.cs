using UnityEngine;
using Cainos.LucidEditor;

namespace Cainos.CustomizablePixelCharacter
{
    //script used to control the character's appearance.
    public class PixelCharacter : MonoBehaviour
    {
        public static PixelCharacter instance;

        #region - REFERENCE -
        [FoldoutGroup("Reference")] public Animator animator;
        [Space]
        [FoldoutGroup("Reference")] public Renderer hat;
        [FoldoutGroup("Reference")] public Renderer hair;
        [FoldoutGroup("Reference")] public Renderer hairClipped;
        [FoldoutGroup("Reference")] public Renderer eye;
        [FoldoutGroup("Reference")] public Renderer eyeBase;
        [FoldoutGroup("Reference")] public Renderer facewear;
        [FoldoutGroup("Reference")] public Renderer cloth;
        [FoldoutGroup("Reference")] public Renderer skirt;
        [FoldoutGroup("Reference")] public Renderer pants;
        [FoldoutGroup("Reference")] public Renderer socks;
        [FoldoutGroup("Reference")] public Renderer shoes;
        [FoldoutGroup("Reference")] public Renderer shoesFront;
        [FoldoutGroup("Reference")] public Renderer back;
        [FoldoutGroup("Reference")] public Renderer expression;
        [FoldoutGroup("Reference")] public Renderer body;
        [Space]
        [FoldoutGroup("Reference")] public Transform rigHead;
        [FoldoutGroup("Reference")] public Transform rigNeck;
        [FoldoutGroup("Reference")] public Transform rigPelvis;
        [FoldoutGroup("Reference")] public Transform rigSpine1;
        [FoldoutGroup("Reference")] public Transform rigSpine2;
        [FoldoutGroup("Reference")] public Transform rigUpperArmL;
        [FoldoutGroup("Reference")] public Transform rigHandL;
        [FoldoutGroup("Reference")] public Transform rigUpperArmR;
        [FoldoutGroup("Reference")] public Transform rigHandR;
        [FoldoutGroup("Reference")] public Transform rigWeapon;
        [Space]
        [FoldoutGroup("Reference")] public Transform weaponSlot;
        #endregion

        #region - CUSTOMIZATION -
        //parameters for tweaking the character's appearance

        //hat material
        [FoldoutGroup("Customization"), ShowInInspector]
        public Material HatMaterial
        {
            get
            {
                if (!hat) return null;
                return hat.sharedMaterial;
            }
            set
            {
                if (!hat) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(hat, "Modify Hat Material");
                #endif

                hat.sharedMaterial = value;
            }
        }

        //hair material
        [FoldoutGroup("Customization"), ShowInInspector]
        public Material HairMaterial
        {
            get
            {
                if (!hair) return null;
                return hair.sharedMaterial;
            }
            set
            {
                if (!hair) return;
                if (!hairClipped) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObjects(new Object[] { hair, hairClipped }, "Modify Hair Material");
                #endif

                hair.sharedMaterial = value;
                hairClipped.sharedMaterial = value;
            }
        }

        //eye material
        [FoldoutGroup("Customization"), ShowInInspector]
        public Material EyeMaterial
        {
            get
            {
                if (!eye) return null;
                return eye.sharedMaterial;
            }
            set
            {
                if (!eye) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(eye, "Modify Eye Material");
                #endif

                eye.sharedMaterial = value;
            }
        }

        [FoldoutGroup("Customization"), ShowInInspector]
        public Material EyeBaseMaterial
        {
            get
            {
                if (!eyeBase) return null;
                return eyeBase.sharedMaterial;
            }
            set
            {
                if (!eyeBase) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(eyeBase, "Modify Eye Base Material");
                #endif

                eyeBase.sharedMaterial = value;
            }
        }

        [FoldoutGroup("Customization"), ShowInInspector]
        public Material FacewearMaterial
        {
            get
            {
                if (!facewear) return null;
                return facewear.sharedMaterial;
            }
            set
            {
                if (!facewear) return;

                #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(facewear, "Modify Facewear Material");
                #endif

                facewear.sharedMaterial = value;
            }
        }

        [FoldoutGroup("Customization"), ShowInInspector]
        public Material ClothMaterial
        {
            get
            {
                if (!cloth) return null;
                return cloth.sharedMaterial;
            }
            set
            {
                if (!cloth) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(cloth, "Modify Cloth Material");
                #endif

                cloth.sharedMaterial = value;
                skirt.sharedMaterial = value;
            }
        }

        [FoldoutGroup("Customization"), ShowInInspector]
        public Material PantsMaterial
        {
            get
            {
                if (!pants) return null;
                return pants.sharedMaterial;
            }
            set
            {
                if (!pants) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObjects(new Object[] { pants, skirt }, "Modify Pants Material");
                #endif

                pants.sharedMaterial = value;
            }
        }

        [FoldoutGroup("Customization"), ShowInInspector]
        public Material SocksMaterial
        {
            get
            {
                if (!socks) return null;
                return socks.sharedMaterial;
            }
            set
            {
                if (!socks) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(socks, "Modify Socks Material");
                #endif

                socks.sharedMaterial = value;
            }
        }

        [FoldoutGroup("Customization"), ShowInInspector]
        public Material ShoesMaterial
        {
            get
            {
                if (!shoes) return null;
                return shoes.sharedMaterial;
            }
            set
            {
                if (!shoes) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(shoes, "Modify Shoes Material");
                #endif

                shoes.sharedMaterial = value;
                shoesFront.sharedMaterial = value;
            }
        }

        //[FoldoutGroup("Customization"), ShowInInspector]
        //public Material ShoesFrontMaterial
        //{
        //    get
        //    {
        //        if (!shoesFront) return null;
        //        return shoesFront.sharedMaterial;
        //    }
        //    set
        //    {
        //        if (!shoesFront) return;

        //        #if UNITY_EDITOR
        //            UnityEditor.Undo.RecordObject(shoesFront, "Modify Shoes Front Material");
        //        #endif

        //        shoesFront.sharedMaterial = value;
        //    }
        //}

        [FoldoutGroup("Customization"), ShowInInspector]
        public Material BackMaterial
        {
            get
            {
                if (!back) return null;
                return back.sharedMaterial;
            }
            set
            {
                if (!back) return;

                #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(back, "Modify Back Material");
                #endif

                back.sharedMaterial = value;
            }
        }

        [FoldoutGroup("Customization"), ShowInInspector]
        public Material BodyMaterial
        {
            get
            {
                if (!body) return null;
                return body.sharedMaterial;
            }
            set
            {
                if (!body) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(body, "Modify Body Material");
                #endif

                body.sharedMaterial = value;
            }
        }


        //to hide part of the hair when wearing curtain hat
        [FoldoutGroup("Customization"), ShowInInspector]
        public bool ClipHair
        {
            get { return clipHair; }
            set
            {
                clipHair = value;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObjects(new Object[] { hair, hairClipped }, "Toggle Clip Hair");
                #endif

                if (hideHair)
                {
                    hair.enabled = false;
                    hairClipped.enabled = false;
                }
                else
                {
                    hair.enabled = !clipHair;
                    hairClipped.enabled = clipHair;
                }
            }
        }
        [SerializeField, HideInInspector]
        private bool clipHair;

        //to hide part of the hair when wearing curtain hat
        [FoldoutGroup("Customization"), ShowInInspector]
        public bool HideHair
        {
            get { return hideHair; }
            set
            {
                hideHair = value;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObjects(new Object[] { hair, hairClipped }, "Toggle Hide Hair");
                #endif

                if (hideHair)
                {
                    hair.enabled = false;
                    hairClipped.enabled = false;
                }
                else
                {
                    hair.enabled = !clipHair;
                    hairClipped.enabled = clipHair;
                }
            }
        }
        [SerializeField, HideInInspector]
        private bool hideHair;


        //whether to display the shoes in front of the pants
        [FoldoutGroup("Customization"), ShowInInspector]
        public bool ShoesInFront
        {
            get { return shoesInFront; }
            set
            {
                shoesInFront = value;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObjects(new Object[] { shoes, shoesFront }, "Toggle Shoes In Front");
                #endif

                if (shoesInFront) shoesFront.enabled = true;
                else shoesFront.enabled = false;
            }
        }
        [SerializeField, HideInInspector]
        private bool shoesInFront;

        //the interval for the character to blink, random between x and y
        [FoldoutGroup("Customization")]
        public Vector2 blinkInterval = new Vector2(0.5f, 5.0f);

        #endregion

        #region - SYNC WEAPON SLOT -

        private void SyncWeaponSlot()
        {
            weaponSlot.transform.position = rigWeapon.transform.position;
            weaponSlot.transform.rotation = rigWeapon.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f);
        }

        #endregion

        #region  - RUNTIME -

        //set the ramp texture of hair material
        //only use this for changing hair color in runtime
        public Texture HairRampTexture
        {
            get { return hairRampTexture; }
            set
            {
                hairRampTexture = value;
                MPBHair.SetTexture("_RampTex", hairRampTexture);

                hair.SetPropertyBlock(MPBHair);
                hairClipped.SetPropertyBlock(MPBHair);
            }
        }
        private Texture hairRampTexture;

        //the current weapon
        public Weapon Weapon
        {
            get
            {
                if (weaponSlot.childCount <= 0) return null;
                return weaponSlot.GetChild(0).GetComponent<Weapon>();
            }
        }

        //character facing  1:facing right   -1:facing left
        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public int Facing
        {
            get { return facing; }
            set
            {
                if (value == 0) return;
                facing = Mathf.RoundToInt(Mathf.Sign(value));

                animator.transform.localScale = new Vector3(1.0f, 1.0f, facing);
                weaponSlot.transform.localScale = new Vector3(1.0f, 1.0f, facing);

                Vector3 pos = animator.transform.localPosition;
                pos.x = 0.064f * -facing;
                animator.transform.localPosition = pos;
            }
        }
        private int facing = 1;

        // is the character dead? if dead, plays dead animation.
        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsDead
        {
            get { return isDead; }
            set
            {
                if ( isDead == value ) return;
                isDead = value;

                animator.SetBool("IsDead", isDead);
                DetachWeapon();
            }
        }
        private bool isDead;

        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsEyeCloed
        {
            get { return isEyeCloed; }
            set
            {
                if ( isEyeCloed == value ) return;
                isEyeCloed = value;

                animator.SetBool("IsEyeClosed", isEyeCloed);
            }
        }
        private bool isEyeCloed;

        //the character's expression
        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public ExpressionType Expression
        {
            get { return _expression; }
            set
            {
                _expression = value;

                animator.SetInteger("Expression", (int)_expression);
            }
        }
        private ExpressionType _expression = ExpressionType.Normal;


        //when character get injured from front or back
        [FoldoutGroup("Runtime"), HorizontalGroup("Runtime/Injure"), Button, DisableInEditMode]
        public void InjuredFront()
        {
            animator.SetTrigger("InjuredFront");
        }

        [FoldoutGroup("Runtime"), HorizontalGroup("Runtime/Injure"), Button, DisableInEditMode]
        public void InjuredBack()
        {
            animator.SetTrigger("InjuredBack");
        }

        //detach weapon to a separated object from the character
        //the weapon needs a Collider2D and Rigidbody2D component attached to it
        //return the detached weapon game object

        [FoldoutGroup("Runtime"), HorizontalGroup("Runtime/Weapon"), Button, DisableInEditMode]
        public GameObject DetachWeapon()
        {
            if (weaponSlot.childCount <= 0) return null;
            GameObject weapon = weaponSlot.GetChild(0).gameObject;

            var c = weapon.GetComponent<Collider2D>();
            if (!c) return null;
            c.isTrigger = false;

            var r = weapon.GetComponent<Rigidbody2D>();
            if (!r) return null;
            r.bodyType = RigidbodyType2D.Dynamic;

            weapon.transform.parent = null;

            return weapon;
        }

        //clear out everything in weapon slot

        [FoldoutGroup("Runtime"), HorizontalGroup("Runtime/Weapon"), Button, DisableInEditMode]
        public void ClearWeapon()
        {
            for (int i = 0; i < weaponSlot.childCount; i++)
            {
                var w = weaponSlot.GetChild(i);
                Destroy(w.gameObject);
            }
        }

        //instantiate a new weapon into weapon slot
        public void AddWeapon(GameObject weaponPrefab, bool clearOld = true)
        {
            if (clearOld) ClearWeapon();
            if (weaponPrefab == null) return;

            var weapon = Instantiate(weaponPrefab);
            weapon.transform.parent = weaponSlot;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            weapon.transform.localScale = Vector3.one;
        }
        #endregion

        #region - OTHER -

        private MaterialPropertyBlock MPBHair
        {
            get
            {
                if (mpbHair == null) mpbHair = new MaterialPropertyBlock();
                return mpbHair;
            }
        }
        private MaterialPropertyBlock mpbHair;

        public enum ExpressionType
        {
            Normal,
            Injured,
            Dead,
            Shocked,
            Happy,
            Sad,
            Shy,
            Sick,
            CatFace
        }

        #endregion

        #region - BLINK -

        private float blinkTimer;

        private void BlinkUpdate()
        {
            if (isDead) return;

            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0.0f)
            {
                blinkTimer = Random.Range(blinkInterval.x, blinkInterval.y);
                if (blinkTimer < 0.1f) blinkTimer = 0.1f;

                if (Expression == ExpressionType.Normal || Expression == ExpressionType.Shy)
                    animator.SetTrigger("Blink");
            }
        }

        #endregion

        #region - UNITY CALLBACKS - 
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if (Application.isPlaying == false) return;

            blinkTimer = Random.Range(blinkInterval.x, blinkInterval.y);
            if (blinkTimer < 0.1f) blinkTimer = 0.1f;

            animator.SetFloat("CycleOffset", Random.value);
        }

        private void Update()
        {
            BlinkUpdate();
        }

        private void FixedUpdate()
        {
            SyncWeaponSlot();
        }

        #endregion

        #region - HELPER FUNCTIONS -
        public void CopyFrom ( PixelCharacter other )
        {
            HatMaterial = other.HatMaterial;
            HairMaterial = other.HairMaterial;
            EyeMaterial = other.EyeMaterial;
            EyeBaseMaterial = other.EyeBaseMaterial;
            FacewearMaterial = other.FacewearMaterial;
            ClothMaterial = other.ClothMaterial;
            PantsMaterial = other.PantsMaterial;
            SocksMaterial = other.SocksMaterial;
            ShoesMaterial = other.ShoesMaterial;
            BackMaterial = other.BackMaterial;
            BodyMaterial = other.BodyMaterial;
            ClipHair = other.ClipHair;
            HideHair = other.HideHair;
            ShoesInFront = other.ShoesInFront;

            ClearWeapon();
            if (other.Weapon) AddWeapon(other.Weapon.gameObject);

            var controller = GetComponent<PixelCharacterController>();
            var controllerOther = other.GetComponent<PixelCharacterController>();
            if (controller && controllerOther)
            {
                controller.attackAction = controllerOther.attackAction;
                controller.attackActionMelee = controllerOther.attackActionMelee;

                controller.projectileSpeed = controllerOther.projectileSpeed;
                controller.projectilePrefab = controllerOther.projectilePrefab;
            }

        }

        #endregion
    }
}
