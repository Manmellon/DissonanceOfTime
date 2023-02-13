using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
    public Camera player_camera;

    public AudioSource _audioSource;
    public AudioClip _walkAudio;
    public AudioClip _runAudio;

    [Header("Settings")]
    public float mouseSensitivity;

    public float interactionRange;
    public LayerMask interactionMask;

    public float walkSpeed = 4.0f;
    public float airSpeed;
    public float runMultiplier = 2.0f;
    public float crouchMultiplier = 0.5f;

    [SerializeField]private Vector3 fallingVelocity;
    //public float gravity = -9.8f;
    public float jumpHeight;

    private float xRotation;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public bool isGrounded;
    public bool isCrouch;

    public bool isInWind;

    public float minHoldDistance = 1.0f;
    public float maxHoldDistance = 4.0f;//Must be less than interaction range

    public float curHoldDistance;

    public Entity holdingItem;
    public bool wasItemIsKinematic;
    public bool wasItemUseGravity;
    public int wasItemLayer;

    public Entity curTarget;

    public static Player singleton;

    void Awake()
    {
        if (singleton == null) singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //????
        //We do it, so gravity not will be accumulate too much when standing on ground
        if (isGrounded && fallingVelocity.magnitude > Physics.gravity.normalized.magnitude)
        {
            fallingVelocity = Physics.gravity.normalized;
        }

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            isGrounded = false;

            //fallingVelocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            fallingVelocity = -Mathf.Sqrt(jumpHeight * 2f * Physics.gravity.magnitude) * Physics.gravity.normalized;

            //jumpTimer = 1;
            //anim.SetBool("Jumping", true);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;


        float resultSpeed = isGrounded? walkSpeed : airSpeed;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                resultSpeed *= runMultiplier;
                if (isGrounded)
                {
                    _audioSource.clip = _runAudio;
                }
                else
                {
                    _audioSource.clip = null;
                }

            }
            else if (isCrouch)
            {
                resultSpeed *= crouchMultiplier;
                if (isGrounded)
                {
                    _audioSource.clip = null;
                }
                else
                {
                    _audioSource.clip = null;
                }
            }
            else
            {
                _audioSource.clip = _walkAudio;
            }

            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }
        else
        {
            _audioSource.clip = null;
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouch = !isCrouch;

            if (isCrouch)
            {
                controller.height -= 1f;
                //groundCheck.localPosition -= new Vector3(0, 0.25f, 0);
                //transform.position -= new Vector3(0, 0.25f, 0);

            }
            else
            {
                controller.height += 1f;
                //groundCheck.localPosition += new Vector3(0, 0.25f, 0);
                //transform.position += new Vector3(0, 0.25f, 0);
            }
        }

        controller.Move(move * resultSpeed * Time.deltaTime);

        fallingVelocity += Physics.gravity * Time.deltaTime;
        controller.Move(fallingVelocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;// * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;// * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        //transform.localRotation = Quaternion.EulerAngles(xRotation, 0, 0);
        player_camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * mouseX);

        if (Input.GetMouseButtonDown(0))
        {
            if (holdingItem == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(player_camera.transform.position, player_camera.transform.forward, out hit, interactionRange, interactionMask))
                {
                    Entity entity = hit.transform.GetComponentInParent<Entity>();
                    if (entity && entity.isDraggable)
                    {
                        //entity.transform.SetParent(player_camera.transform);

                        holdingItem = entity;
                        wasItemUseGravity = entity._rigidbody.useGravity;
                        wasItemIsKinematic = entity._rigidbody.isKinematic;
                        wasItemLayer = entity.gameObject.layer;

                        entity._rigidbody.useGravity = false;
                        entity._rigidbody.isKinematic = true;
                        entity.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

                        Physics.IgnoreCollision(controller, entity._collider);

                        curHoldDistance = Mathf.Clamp(Vector3.Distance(player_camera.transform.position, entity.transform.position), minHoldDistance, maxHoldDistance);
                    }
                }
            }
            else
            {
                holdingItem._rigidbody.useGravity = wasItemUseGravity;
                holdingItem._rigidbody.isKinematic = wasItemIsKinematic;
                holdingItem.gameObject.layer = wasItemLayer;

                if (holdingItem.resetRotationAfterDrop)
                    holdingItem.transform.rotation = Quaternion.identity;

                //holdingItem.transform.SetParent(null);

                Physics.IgnoreCollision(controller, holdingItem._collider, false);

                holdingItem = null;
            }
        }

        if (holdingItem != null)
        {
            holdingItem._rigidbody.position = player_camera.transform.position + player_camera.transform.forward * minHoldDistance;

            Vector3 curPos = holdingItem._rigidbody.position;

            Vector3 newPos = (player_camera.transform.position + player_camera.transform.forward * curHoldDistance);
            Vector3 dir = newPos - curPos;
            RaycastHit hit;
            bool wasHit = holdingItem._rigidbody.SweepTest(dir, out hit);
            if (wasHit && hit.distance < dir.magnitude)
            {
                //Debug.Log(hit.collider.gameObject + " " + hit.distance);
                //holdingItem._rigidbody.MovePosition(curPos + dir.normalized * (hit.distance - 0.01f));
                holdingItem._rigidbody.position = curPos + dir.normalized * (hit.distance);
            } 
            else
            {
                //Debug.Log(hit.distance + " " + dir.magnitude);
                //holdingItem._rigidbody.MovePosition(newPos);
                holdingItem._rigidbody.position = newPos;
            }

            if (curTarget)
            {
                if (curTarget._outline)
                    curTarget._outline.enabled = false;
                curTarget = null;
            }

            //If holding Pillar, need raycast to check if target other pillar here

        }
        else //if not holding anything
        {

            RaycastHit interactionHit;
            if (Physics.Raycast(player_camera.transform.position, player_camera.transform.forward, out interactionHit, interactionRange, interactionMask))
            {
                Entity entity = interactionHit.collider.GetComponentInParent<Entity>();

                if (curTarget && curTarget != entity)
                {
                    if (curTarget._outline)
                        curTarget._outline.enabled = false;
                    curTarget = null;
                }

                if (entity != null)
                {
                    if (entity._outline)
                        entity._outline.enabled = true;
                    curTarget = entity;
                }

            }
            else if (curTarget)
            {
                if (curTarget._outline)
                    curTarget._outline.enabled = false;
                curTarget = null;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit[] hits;
            //must use SphereCastAll and maybe sort by distance

            hits = Physics.SphereCastAll(player_camera.transform.position, 1.0f, player_camera.transform.forward);

            foreach (var hit in hits)
            {
                Entity entity = hit.collider.gameObject.GetComponentInParent<Entity>();
                if (entity)
                {
                    entity.isFreezed = !entity.isFreezed;
                }
            }
            
        }
    }
}
