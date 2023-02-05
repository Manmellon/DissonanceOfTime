using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FanMode { UP, DOWN}
public class Fan : SwitchingEntity
{
    [Header("Fan")]
    public FanMode mode;

    public float fanPower;
    public float maxWindDistance;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //for test
        if (Input.GetKeyDown(KeyCode.E))
        {
            Switch();
        }
    }

    public override void TurnedOnAction()
    {
        base.TurnedOnAction();

        if (mode == FanMode.UP)
            _animator.SetBool("RotateLeft", true);
        else
            _animator.SetBool("RotateRight", true);
    }

    public override void TurnedOffAction()
    {
        base.TurnedOffAction();

        if (mode == FanMode.UP)
            _animator.SetBool("RotateLeft", false);
        else
            _animator.SetBool("RotateRight", false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isOn || isFreezed) return;

        float x = Mathf.Clamp(Vector3.Distance(transform.position, other.transform.position), 0, maxWindDistance);

        float windPower = Physics.gravity.magnitude * x + 25 * (1 - x * x / maxWindDistance / 2);

        Vector3 powerVector;
        if (mode == FanMode.UP)
            powerVector = windPower * transform.up;
        else
            powerVector = windPower * -transform.up;


        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();
            if (player == null) return;

            player.controller.Move(powerVector);
        }
        else
        {
            Entity entity = other.GetComponentInParent<Entity>();
            if (entity == null) return;

            entity._rigidbody.transform.rotation = Quaternion.identity;
            entity._rigidbody.angularVelocity = Vector3.zero;

            /*var acceleration = (entity._rigidbody.velocity - entity.lastVelocity) / Time.fixedDeltaTime;
            entity.lastVelocity = entity._rigidbody.velocity;

            entity._rigidbody.AddForce(powerVector - acceleration, ForceMode.Acceleration);*/
            entity._rigidbody.velocity = powerVector;
        }
    }
}
