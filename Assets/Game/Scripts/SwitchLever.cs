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

        if (influencedSwitchEntity != null)
            influencedSwitchEntity.TurnOn();

        if (_animator != null)
            _animator.SetBool("Pressed", true);
    }

    protected override void TurnedOffAction()
    {
        base.TurnedOffAction();

        if (influencedSwitchEntity != null)
            influencedSwitchEntity.TurnOff();

        if (_animator != null)
            _animator.SetBool("Pressed", false);
    }
}
