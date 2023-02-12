using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : SwitchingEntity
{
    [Header("Button")]
    public ButtonCollider buttonCollider;
    public Collider pressingCollider;
    public Collider baseCollider;

    public SwitchingEntity influencedSwitchEntity;

    protected override void Start()
    {
        base.Start();

        Physics.IgnoreCollision(pressingCollider, baseCollider);
    }

    protected override void Update()
    {
        base.Update();

        //Physics.BoxCast();
    }

    protected override void TurnedOnAction()
    {
        base.TurnedOnAction();

        influencedSwitchEntity.TurnOn();

        _animator.SetBool("Pressed", true);
    }

    protected override void TurnedOffAction()
    {
        base.TurnedOffAction();

        influencedSwitchEntity.TurnOff();

        _animator.SetBool("Pressed", false);
    }
    /*
        private void OnTriggerStay(Collider other)
        {
            if (buttonCollider.isColliding)
            {
                if (!isOn)
                {
                    TurnOn();
                }
            }
            else
            {
                if (isOn)
                {
                    TurnOff();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!buttonCollider.isColliding && isOn)
            {
                TurnOff();
            }
        }*/

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Button TriggerStay: " + other.gameObject);
        if (!isOn)
        {
            TurnOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOn)
        {
            TurnOff();
        }
    }
}
