using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLever : SwitchingEntity
{
    [Header("Switch Lever")]
    public SwitchingEntity influencedSwitchEntity;

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
}
