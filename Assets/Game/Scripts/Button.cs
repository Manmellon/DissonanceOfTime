using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : SwitchingEntity
{
    [Header("Button")]
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
}
