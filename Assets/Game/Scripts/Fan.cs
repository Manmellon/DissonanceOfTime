using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FanMode { UP, DOWN}

public enum FanForceMode { Impulse, Continous}

[Serializable]
public struct ContainingObject
{
    public GameObject gameObject;
    public bool tookImpulse;
}

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

    public List<ContainingObject> objectsInside = new List<ContainingObject>();

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

        for (int i = 0; i < objectsInside.Count; i++)
        {
            var go = objectsInside[i];

            Entity entity = go.gameObject.GetComponentInParent<Entity>();

            if (entity != null)
            {
                if (entity._collider != null && !go.tookImpulse)
                    OnTriggerEnter(entity._collider);
                continue;
            }

            Player player = go.gameObject.GetComponentInParent<Player>();

            if (player != null)
            {
                Debug.Log("Player in fan: tookImpulse = " + go.tookImpulse);
                if (player.controller != null && !go.tookImpulse)
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

        bool contains = false;
        int founded = -1;
        
        for (int i = 0; i < objectsInside.Count; i++)
        {
            var go = objectsInside[i];

            if (go.gameObject.Equals(other.gameObject))
            {
                contains = true;
                founded = i;
                break;
            }
        }

        if (!contains)
        {
            Debug.Log("Enter not conatins");
            var co = new ContainingObject();
            co.gameObject = other.gameObject;
            co.tookImpulse = true;
            objectsInside.Add(co);
        }
        else
        {
            var co = objectsInside[founded];
            co.tookImpulse = true;
            objectsInside[founded] = co;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        bool contains = false;
        foreach (var go in objectsInside)
        {
            if (go.gameObject.Equals(other.gameObject))
            {
                contains = true;
                break;
            }
        }
        //if (!objectsInside.Contains(other.gameObject))
        //   objectsInside.Add(other.gameObject);
        if (!contains)
        {
            Debug.Log("New object inside");
            var co = new ContainingObject();
            co.gameObject = other.gameObject;
            objectsInside.Add(co);
        }


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
        foreach (var go in objectsInside)
        {
            if (go.gameObject.Equals(other.gameObject))
            {
                objectsInside.Remove(go);
                break;
            }
        }
        //if (objectsInside.Contains(other.gameObject))
        //    objectsInside.Remove(other.gameObject);

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();
            if (player == null) return;

            player.isInWind = false;
        }
    }
}
