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

    private void OnTriggerStay(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null && entity.isFreezed) return;

        if (!isOn)
        {
            TurnOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null && entity.isFreezed) return;

        if (isOn)
        {
            TurnOff();
        }
    }
}
