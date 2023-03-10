using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
    public Camera player_camera;

    public Animator _animator;

    public AudioSource _audioSource;
    public AudioClip _walkAudio;
    public AudioClip _runAudio;

    [Header("Settings")]
    public float mouseSensitivity;

    public float interactionRange;
    public LayerMask interactionMask;

    public float mass = 1.0f;

    public float walkSpeed = 4.0f;
    public float airSpeed;
    public float runMultiplier = 2.0f;
    public float crouchMultiplier = 0.5f;

    public Vector3 fallingVelocity;
    //public float gravity = -9.8f;
    public float jumpHeight;

    private float xRotation;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public bool isGrounded;
    public bool isCrouch;

    public bool isInWind;

    public bool isShooting;

    public float minHoldDistance = 1.0f;
    public float maxHoldDistance = 4.0f;//Must be less than interaction range

    public float curHoldDistance;
    public Quaternion startHoldItemRotation;
    public Quaternion startHoldPlayerRotation;

    public Entity holdingItem;
    public bool wasItemIsKinematic;
    public bool wasItemUseGravity;
    public int wasItemLayer;

    public Entity curTarget;

    [Header("Gun")]
    public List<Entity> freezedByGun = new List<Entity>();
    public GameObject beam;
    public LayerMask gunLayers;

    [Header("Physics")]
    public Vector3 impact;

    public static Player singleton;

    void Awake()
    {
        if (singleton == null) singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (UI.singleton.isPaused) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) && !isInWind;

        //????
        //We do it, so gravity not will be accumulate too much when standing on ground
        if (isGrounded && fallingVelocity.magnitude > Physics.gravity.normalized.magnitude)
        {
            fallingVelocity = Physics.gravity.normalized;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;

        float resultSpeed = isGrounded? walkSpeed : airSpeed;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
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
            /*else if (isCrouch)
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
            }*/
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

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            isGrounded = false;

            //fallingVelocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            fallingVelocity = -Mathf.Sqrt(jumpHeight * 2f * Physics.gravity.magnitude) * Physics.gravity.normalized;

            //jumpTimer = 1;
            //anim.SetBool("Jumping", true);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Vector3 impactDir = (-Physics.gravity.normalized + move.normalized) / 2;
                AddImpact(impactDir.normalized, 0.1f);
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

        if (holdingItem == null)
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

                    if (entity.isDraggable)
                    {
                        string text = "??? - ????? " + entity.itemName;
                        if (entity is ChronoPillar)
                            text += "\nCtrl + ??? - ?????, ??????? ??? ?????";
                        UI.singleton.SetDescriptionText(text);
                    }
                    else if (entity is SwitchingEntity s && s.switchByClick)
                    {
                        UI.singleton.SetDescriptionText("??? - ???????????? " + entity.itemName);
                    }
                    else
                    {
                        UI.singleton.SetDescriptionText("");
                    }
                }
                else
                {
                    UI.singleton.SetDescriptionText("");
                }

                if (Input.GetMouseButtonDown(0) && entity)
                {
                    if (entity.isDraggable)
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
                        startHoldItemRotation = entity.transform.rotation;
                        startHoldPlayerRotation = player_camera.transform.rotation;

                        if (entity is ChronoPillar pillar)
                        {
                            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                                pillar.ResetAllConnections();
                        }
                    }
                    else if (entity is SwitchingEntity switching && switching.switchByClick)
                    {
                        switching.isOn = !switching.isOn;
                    }
                }

            }
            else
            {
                UI.singleton.SetDescriptionText("");
                if (curTarget)
                {
                    if (curTarget._outline)
                        curTarget._outline.enabled = false;
                    curTarget = null;
                }
            }
        }
        else //if hold some item
        {
            MoveHoldingItem();

            //If holding Pillar, need raycast to check if target other pillar here
            RaycastHit pillarHit;

            if (holdingItem is ChronoPillar pillar)
            {
                if (Physics.Raycast(player_camera.transform.position, player_camera.transform.forward, out pillarHit, 1000.0f, interactionMask))
                {
                    Entity entity = pillarHit.transform.GetComponentInParent<Entity>();
                    if (entity && entity is ChronoPillar otherPillar)
                    {
                        entity._outline.enabled = true;

                        curTarget = entity;

                        UI.singleton.SetDescriptionText("??? - ????????????/??????????? ");
                    }
                    else
                    {
                        UI.singleton.SetDescriptionText("??? - ??????? " + holdingItem.itemName);
                        if (curTarget)
                        {
                            if (curTarget._outline)
                                curTarget._outline.enabled = false;
                            curTarget = null;
                        }
                    }

                }
                else
                {
                    UI.singleton.SetDescriptionText("??? - ??????? " + holdingItem.itemName);
                    if (curTarget)
                    {
                        if (curTarget._outline)
                            curTarget._outline.enabled = false;
                        curTarget = null;
                    }
                }
                
                if (Input.GetMouseButtonDown(0))
                {
                    if (curTarget && curTarget is ChronoPillar otherPillar)
                    {
                        pillar.ProcessConnection(otherPillar);
                    }
                    else
                        Drop();
                }
            }
            else
            {
                UI.singleton.SetDescriptionText("??? - ??????? " + holdingItem.itemName);
                if (curTarget)
                {
                    if (curTarget._outline)
                        curTarget._outline.enabled = false;
                    curTarget = null;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Drop();
                }
            }
        }

        List<Entity> newFreezing = new List<Entity>();

        if (Input.GetMouseButtonDown(1))
        {
            _animator.SetBool("Shoot", true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            _animator.SetBool("Shoot", false);
            isShooting = false;
        }

        if (Input.GetMouseButton(1) && isShooting)
        {
            beam.SetActive(true);

            RaycastHit[] hits;

            hits = Physics.SphereCastAll(beam.transform.position, 0.02f, beam.transform.right, gunLayers);

            Debug.DrawRay(beam.transform.position, beam.transform.right, Color.red);

            foreach (var hit in hits)
            {
                Entity entity = hit.collider.gameObject.GetComponentInParent<Entity>();
                if (entity)
                {
                    if (! newFreezing.Contains(entity))
                        newFreezing.Add(entity);
                }
            }
        }
        else
        {
            beam.SetActive(false);
        }

        //Remove unfreezed
        List<Entity> mustUnfreeze = new List<Entity>();

        foreach (var f in freezedByGun)
        {
            if (!newFreezing.Contains(f))
            {
                f.isFreezed = false;
                mustUnfreeze.Add(f);
            }
        }

        foreach (var u in mustUnfreeze)
        {
            freezedByGun.Remove(u);
        }

        //Add new freezed
        foreach (var n in newFreezing)
        {
            if (!freezedByGun.Contains(n))
            {
                n.isFreezed = true;
                freezedByGun.Add(n);
            }
        }

        controller.Move(impact * Time.deltaTime);
        impact = Vector3.Lerp(impact, Vector3.zero, Time.deltaTime * 0.1f);

        if (isGrounded && !isInWind)
            impact = Vector3.zero;
    }

    public void AddImpact(Vector3 dir, float force)
    {
        //dir.Normalize();
        //if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }

    public void MoveHoldingItem()
    {
        if (holdingItem == null) return;

        //holdingItem.transform.Rotate(Vector3.up * mouseX);
        //holdingItem.transform.rotation = player_camera.transform.rotation;
        holdingItem.transform.rotation = (player_camera.transform.rotation * Quaternion.Inverse(startHoldPlayerRotation)) * startHoldItemRotation;

        holdingItem._rigidbody.position = player_camera.transform.position + player_camera.transform.forward * minHoldDistance;

        Vector3 curPos = holdingItem._rigidbody.position;

        Vector3 newPos = (player_camera.transform.position + player_camera.transform.forward * curHoldDistance);
        Vector3 dir = newPos - curPos;
        RaycastHit hit;
        bool wasHit = holdingItem._rigidbody.SweepTest(dir, out hit);
        //Debug.Log(wasHit + " " + hit.collider + " " + hit.distance + " < " + dir.magnitude);
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
    }

    public void Drop()
    {
        if (holdingItem == null) return;

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
