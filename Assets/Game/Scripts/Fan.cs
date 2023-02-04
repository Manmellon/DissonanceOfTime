using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FanMode { UP, DOWN}
public class Fan : SwitchingEntity
{
    [Header("Fan")]
    public FanMode mode;

    public float fanPower;

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

        //_animator.Play("Idle");
        if (mode == FanMode.UP)
            _animator.SetBool("RotateLeft", false);
        else
            _animator.SetBool("RotateRight", false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isOn || isFreezed) return;

        Vector3 powerVector;
        if (mode == FanMode.UP)
            powerVector = fanPower * transform.up;
        else
            powerVector = fanPower * -transform.up;


        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();

            player.controller.Move(powerVector);
        }
        else
        {
            Entity entity = other.GetComponentInParent<Entity>();
            if (entity == null) return;

            
            entity._rigidbody.transform.rotation = Quaternion.identity;
            entity._rigidbody.angularVelocity = Vector3.zero;

            entity._rigidbody.AddForce(powerVector, ForceMode.Impulse);
        }
    }
}
