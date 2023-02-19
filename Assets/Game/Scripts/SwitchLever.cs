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

        if (_animator != null)
            _animator.SetBool("Pressed", true);

        if (isFreezed) return;

        if (influencedSwitchEntity != null)
            influencedSwitchEntity.TurnOn();
    }

    protected override void TurnedOffAction()
    {
        base.TurnedOffAction();

        if (_animator != null)
            _animator.SetBool("Pressed", false);

        if (isFreezed) return;

        if (influencedSwitchEntity != null)
            influencedSwitchEntity.TurnOff();
    }

    protected override void FreezeTimeAction()
    {
        base.FreezeTimeAction();
    }

    protected override void UnFreezeTimeAction()
    {
        base.UnFreezeTimeAction();

        if (isOn && !influencedSwitchEntity.isOn)
            influencedSwitchEntity.TurnOn();
        else if (!isOn && influencedSwitchEntity.isOn)
            influencedSwitchEntity.TurnOff();
    }
}
