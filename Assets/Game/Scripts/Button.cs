using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : SwitchingEntity
{
    [Header("Button")]
    public ButtonCollider buttonCollider;
    public SwitchingEntity influencedSwitchEntity;

    protected override void TurnedOnAction()
    {
        base.TurnedOnAction();

        influencedSwitchEntity.TurnOn();
    }

    protected override void TurnedOffAction()
    {
        base.TurnedOffAction();

        influencedSwitchEntity.TurnOff();
    }

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
    }
}
