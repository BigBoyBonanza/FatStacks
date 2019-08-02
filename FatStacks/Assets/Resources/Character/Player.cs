using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthManager healthManager;
    private float DesiredSpeed = 10.0f;
    [HideInInspector]
    public float WalkSpeed = 5f;
    [HideInInspector]
    public float SprintSpeed = 7.5f;
    [HideInInspector]
    public float JumpForce = 5f;
    public GameObject DeadCharacter;
    public GameObject WinCharacter;
    public Text HealthText;
    //public Image HealthBar;


    Rigidbody _Rigidbody;
    CapsuleCollider _Collider;
    public Camera _Camera;
    public Pickup myPickup;
    private bool JumpPressed = false;
    private bool IsFeatherFalling;
    private Vector3 MoveVector;
    private Vector3 PrevMoveVector;
    private float HighestY;
    private int AdjacentColliderCount;
    private bool IsCrouched = false;
    private CrouchState _CrouchState = CrouchState.NotCrouched;
    private int SolidLayerMask;
    enum CrouchState
    {
        Crouched,
        NotCrouched,
        Falling,
        Rising
    }
    static private Scene LatestScene;
    static public bool firstSpawnInScene;
    void Awake()
    {
        //Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Get Components
        _Rigidbody = GetComponent<Rigidbody>();
        _Collider = GetComponent<CapsuleCollider>();
        healthManager = GetComponent<HealthManager>();

        //Get layer mask
        SolidLayerMask = LayerMask.GetMask("Default", "InteractSolid");
    }
    private void Start()
    {
        //Update 'firstSpawnInScene'
        //'firstSpawnInScene' is used by other scripts for initialization.
        if (firstSpawnInScene)
            LatestScene = SceneManager.GetActiveScene();
        firstSpawnInScene = LatestScene != SceneManager.GetActiveScene();
    }
    private void OnCollisionEnter(Collision collision)
    {
        AdjacentColliderCount += 1;
        if (AdjacentColliderCount > 1)
        {
            _Rigidbody.useGravity = false;
        }
        //float point = collision.GetContact(0).point.y; up-to-date
        float point = collision.contacts[0].point.y;
        //Fall damage
        if (point < transform.position.y - 0.9f)
        {
            float impact = (HighestY - transform.position.y);
            float[] threshold = new float[5]
            {
                0.5f,
                1.5f,
                6.5f,
                9.5f,
                18f,
            };
            if (impact > threshold[0])
            {
                //Debug.Log(impact);
            }

            if (threshold[0] <= impact && impact < threshold[1])
            {
                //Debug.Log("Soft Landing");
            }
            else if (threshold[1] <= impact && impact < threshold[2])
            {
                //Debug.Log("Medium Landing");
            }
            else if (threshold[2] <= impact && impact < threshold[3])
            {
                //Hard land sound
                //Debug.Log("Hard Landing");
                int damage = (int)((impact - threshold[2]) / (threshold[3] - threshold[2]) * 10);
                //Debug.Log("Damage Dealt: " + damage);
                healthManager.DealDamage(damage);
            }
            else if (threshold[3] <= impact && impact < threshold[4])
            {
                //Debug.Log("Really Hard Landing");
                int damage = (int)((impact - threshold[2]) / (threshold[3] - threshold[2]) * 10);
                //Debug.Log("Damage Dealt: " + damage);
                healthManager.DealDamage(damage);
            }
            else if (threshold[4] <= impact)
            {
                //Debug.Log("Fatal Landing");
                healthManager.Kill();
            }
            HighestY = transform.position.y;
        }
        else if (point > (transform.position.y + 0.8f))
        {
            Box obj = collision.gameObject.GetComponent<Box>();
            if (obj != null)
            {
                //Possibly Crushed
                int weight = obj.GetStackWeight();
                //Debug.Log(weight);
                int[] threshold = new int[3]
                {
                    1,
                    2,
                    3,
                };
                if (threshold[0] <= weight && weight < threshold[1])
                {
                    //Debug.Log("Small Crush");
                    healthManager.DealDamage(10);
                }
                else if (threshold[1] <= weight && weight < threshold[2])
                {
                    //Debug.Log("Medium Crush");
                    healthManager.DealDamage(15);
                }
                else if (threshold[2] <= weight)
                {
                    //Debug.Log("Crushed");
                    healthManager.Kill();
                }
            }

        }

    }

    private void OnCollisionExit(Collision collision)
    {
        AdjacentColliderCount -= 1;
        if (AdjacentColliderCount <= 1)
        {
            _Rigidbody.useGravity = true;
            //Debug.Log("Fake gravity disengaged");
        }
        if (transform.position.y > HighestY)
        {
            HighestY = transform.position.y;
            Debug.DrawRay(transform.position, Vector3.up, Color.blue, 10f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BoxGrid")
        {
            myPickup.placementGrid = other.GetComponent<Grid>();
        }
    }
    // Update is called once per frame
    private void Update()
    {
        bool grounded = checkGrounded();
        //Get jump input
        if (grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                JumpPressed = true;
            }
        }
        else
        {
            if (HighestY < transform.position.y)
            {
                HighestY = transform.position.y;
                Debug.DrawRay(transform.position, Vector3.up, Color.blue, 10f);
            }
        }

        //Get Sprint
        DesiredSpeed = (Input.GetButton("Sprint")) ? (SprintSpeed) : (WalkSpeed);
        /*
        //Get Falling
        is_feather_falling = (my_rigidbody.velocity.y < 0 && Input.GetButton("Jump"));
        */
        //Get Move Input
        MoveVector.z = Input.GetAxis("Vertical") * DesiredSpeed * Time.fixedDeltaTime;
        MoveVector.x = Input.GetAxis("Horizontal") * DesiredSpeed * Time.fixedDeltaTime;
        MoveVector = transform.localRotation * MoveVector;
        MoveVector = Vector3.Lerp(PrevMoveVector, MoveVector, 0.2f);
        MoveVector = Vector3.ClampMagnitude(MoveVector, DesiredSpeed * Time.fixedDeltaTime);
        //Get Crouch Input
        if (Input.GetButtonDown("Crouch"))
        {
            if (IsCrouched)
            {
                if (!Physics.CheckSphere(transform.position + new Vector3(0f, 0.5f, 0f), _Collider.radius - 0.1f, SolidLayerMask))
                {
                    SetCrouchState(false);
                }
            }
            else
            {
                SetCrouchState(true);
            }

        }
        //Lerp the camera based on crouch
        if (_CrouchState != CrouchState.Crouched && _CrouchState != CrouchState.NotCrouched)
        {
            Vector3 target = Vector3.zero;
            switch (_CrouchState)
            {
                case CrouchState.Rising:
                    target = new Vector3(0f, 0.5f, 0f);
                    break;
                case CrouchState.Falling:
                    target = new Vector3(0f, -0.1f, 0f);
                    break;
            }
            _Camera.transform.localPosition = Vector3.Lerp(_Camera.transform.localPosition, target, 0.1f);
            if (_Camera.transform.localPosition == target)
            {
                switch (_CrouchState)
                {
                    case CrouchState.Rising:
                        _CrouchState = CrouchState.NotCrouched;
                        break;
                    case CrouchState.Falling:
                        _CrouchState = CrouchState.Crouched;
                        break;
                }
            }
        }
        //Locking the cursor
        if (Input.GetKeyDown("escape"))
        {
            //Toggle cursor lock
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ?
                (CursorLockMode.None) :
                (CursorLockMode.Locked);
            Cursor.visible = (Cursor.lockState == CursorLockMode.None);
        }

        //Restarting scene
        if (Input.GetKeyDown(KeyCode.F1))
        {
            //Force character to spwan like they do in the editor
            firstSpawnInScene = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            healthManager.DealDamage(25);
        }
        //Update health bar
        //HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, Health / 100f, 0.1f);
        HealthText.text = healthManager.health.ToString();
    }

    private void FixedUpdate()
    {
        //Do Jump Input
        if (JumpPressed)
        {
            JumpPressed = false;
            Vector3 velocity = _Rigidbody.velocity;
            velocity.y = 0f;
            _Rigidbody.velocity = velocity;
            _Rigidbody.AddForce(new Vector3(0f, JumpForce, 0f), ForceMode.VelocityChange);
            _Rigidbody.drag = 0;
        }
        //Set drag
        _Rigidbody.drag = (IsFeatherFalling) ? (5.0f) : (0.0f);
        //Check for approaching collision
        //RaycastHit hit_info;
        //while (Physics.CapsuleCast(transform.position + _Collider.center + (Vector3.up * _Collider.height * 0.15f),
        //        transform.position + _Collider.center + (Vector3.up * _Collider.height * -0.15f), _Collider.radius,
        //        MoveVector.normalized, out hit_info, MoveVector.magnitude,
        //        SolidLayerMask))
        //{
        //    //If normal has a significant y component, break the while loop
        //    if (hit_info.normal.y > 0.5f)
        //    {
        //        break;
        //    }
        //    Vector3 hit_normal_perpendicular = Quaternion.AngleAxis(90f, Vector3.up) * hit_info.normal;
        //    hit_normal_perpendicular *= (Vector3.Angle(hit_normal_perpendicular, MoveVector.normalized) > 90f) ? (-1) : (1);
            

        //    //Calculate clamp
        //    Vector3 hitDirection = (hit_info.point - transform.position);
        //    hitDirection.y = 0;
        //    hitDirection = hitDirection.normalized;
        //    //Debug.DrawRay(transform.position, hitDirection, Color.magenta, 5f);
        //    //float clamp = Vector3.Dot(MoveVector.normalized, hit_info.normal);
        //    float clamp = Vector3.Dot(hitDirection, MoveVector.normalized);
        //    clamp = (1f - Mathf.Abs(clamp)) * Mathf.Sign(clamp) * 1.2f;

        //    MoveVector = hit_normal_perpendicular * Mathf.Sign(clamp) * MoveVector.magnitude * clamp;
        //    MoveVector.y = 0;

        //    //Debug.Log(move_vector);
        //}
        //Manual Gravity
        if (_Rigidbody.useGravity == false)
        {
            _Rigidbody.AddForce(Vector3.down * 9.81f, ForceMode.Impulse);
        }
        //Translate the player
        _Rigidbody.MovePosition(_Rigidbody.position + MoveVector);
        PrevMoveVector = MoveVector;
    }
    private bool checkGrounded()
    {
        return Physics.CheckSphere(transform.position + new Vector3(0, -1.06f, 0), 0.475f, SolidLayerMask);
    }
    public void SetCrouchState(bool crouched, bool instant = false)
    {
        IsCrouched = crouched;
        if (IsCrouched == false)
        {
            //Check if uncrouching is possible

            if (IsCrouched == false)
            {
                //Set height and center of collider
                _Collider.height = 2;
                _Collider.center = new Vector3
                {
                    x = 0f,
                    y = 0f,
                    z = 0f
                };
                _CrouchState = CrouchState.Rising;
            }
        }
        else
        {
            //Set height of collider
            _Collider.height = 1;
            _Collider.center = new Vector3
            {
                x = 0f,
                y = -0.5f,
                z = 0f
            };
            _CrouchState = CrouchState.Falling;
        }
        if (instant)
        {
            Vector3 target = Vector3.zero;
            switch (_CrouchState)
            {
                case CrouchState.Rising:
                    target = new Vector3(0f, 0.5f, 0f);
                    _CrouchState = CrouchState.NotCrouched;
                    break;
                case CrouchState.Falling:
                    target = new Vector3(0f, -0.1f, 0f);
                    _CrouchState = CrouchState.Crouched;
                    break;
            }
            _Camera.transform.localPosition = target;
        }
    }

    public bool GetCrouched()
    {
        return IsCrouched;
    }
}
