using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FanMode { UP, DOWN}

public enum FanForceMode { Impulse, Continous}

public class Fan : SwitchingEntity
{
    [Header("Fan")]
    public Collider triggerCollider;

    public FanMode mode;
    public FanForceMode forceMode;

    [Header("Impulse")]
    public float fanPower;

    [Header("Continous")]
    public float fanSpeed = 10.0f;
    public float maxWindDistance;

    public List<GameObject> objectsInside = new List<GameObject>();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void TurnedOnAction()
    {
        base.TurnedOnAction();

        if (mode == FanMode.UP)
            _animator.SetBool("RotateLeft", true);
        else
            _animator.SetBool("RotateRight", true);
    }

    protected override void TurnedOffAction()
    {
        base.TurnedOffAction();

        if (mode == FanMode.UP)
            _animator.SetBool("RotateLeft", false);
        else
            _animator.SetBool("RotateRight", false);
    }

    protected override void UnFreezeTimeAction()
    {
        base.UnFreezeTimeAction();

        foreach (var go in objectsInside)
        {
            Entity entity = go.GetComponentInParent<Entity>();

            if (entity != null)
            {
                if (entity._collider != null)
                    OnTriggerEnter(entity._collider);
                continue;
            }

            Player player = go.GetComponentInParent<Player>();

            if (player != null)
            {
                if (player.controller != null)
                {
                    OnTriggerEnter(player.controller);
                }

            }
        }


    }

    protected override void FreezeTimeAction()
    {
        base.FreezeTimeAction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOn || _isFreezed) return;

        if (forceMode != FanForceMode.Impulse) return;

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();
            if (player == null) return;

            //player.controller.Move(powerVector);
            //player.fallingVelocity = powerVector;
            //player.fallingVelocity = transform.up * fanSpeed / x;

            //Vector3 impact = transform.up.normalized * fanPower;// / mass;
            //player.controller.Move(impact * Time.deltaTime);

            player.AddImpact(transform.up, fanPower);

            player.isInWind = true;
        }
        else
        {
            Entity entity = other.GetComponentInParent<Entity>();
            if (entity == null) return;

            entity._rigidbody.transform.rotation = Quaternion.identity;
            entity._rigidbody.angularVelocity = Vector3.zero;

            //var acceleration = (entity._rigidbody.velocity - entity.lastVelocity) / Time.fixedDeltaTime;
            //entity.lastVelocity = entity._rigidbody.velocity;

            //entity._rigidbody.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);
            //entity._rigidbody.velocity = powerVector;
            entity._rigidbody.AddForce(transform.up * fanPower, ForceMode.Impulse);

            //entity._rigidbody.velocity = transform.up * fanSpeed / x;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!objectsInside.Contains(other.gameObject))
            objectsInside.Add(other.gameObject);

        if (!isOn || _isFreezed)
        {
            foreach (var go in objectsInside)
            {
                if (other.CompareTag("Player"))
                {
                    Player player = other.GetComponentInParent<Player>();
                    if (player == null) continue;

                    player.isInWind = false;
                }
            }

            //objectsInside.Clear();

            return;
        }


        if (forceMode != FanForceMode.Continous) return;

        float x = Mathf.Clamp(Vector3.Distance(transform.position, other.transform.position), 0, maxWindDistance);

        //float windPower = Physics.gravity.magnitude * x + 25 * (1 - x * x / maxWindDistance / 2);

        //Vector3 powerVector;
        //if (mode == FanMode.UP)
        //    powerVector = windPower * transform.up;
        //else
        //    powerVector = windPower * -transform.up;
            

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();
            if (player == null) return;

            //player.controller.Move(powerVector);
            //player.fallingVelocity = powerVector;
            player.fallingVelocity = transform.up * fanSpeed / x;

            player.isInWind = true;
        }
        else
        {
            Entity entity = other.GetComponentInParent<Entity>();
            if (entity == null) return;

            entity._rigidbody.transform.rotation = Quaternion.identity;
            entity._rigidbody.angularVelocity = Vector3.zero;

            //var acceleration = (entity._rigidbody.velocity - entity.lastVelocity) / Time.fixedDeltaTime;
            //entity.lastVelocity = entity._rigidbody.velocity;

            //entity._rigidbody.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);
            //entity._rigidbody.velocity = powerVector;
            entity._rigidbody.AddForce(transform.up * Physics.gravity.magnitude, ForceMode.Acceleration);

            entity._rigidbody.velocity = transform.up * fanSpeed / x;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInside.Contains(other.gameObject))
            objectsInside.Remove(other.gameObject);

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();
            if (player == null) return;

            player.isInWind = false;
        }
    }
}
