using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : SwitchingEntity
{
    [Header("Platform")]
    public Transform offPosition;
    public Transform onPosition;

    public float speed;

    public float startMovingTime;
    public Vector3 curDirection;

    public Vector3 velocity;

    protected override void Update()
    {
        base.Update();

        _rigidbody.MovePosition(_rigidbody.position + velocity * Time.deltaTime);

        if (isOn)
        {
            if ((_rigidbody.position - offPosition.position).magnitude >= (onPosition.position - offPosition.position).magnitude)
            {
                velocity = _rigidbody.velocity = Vector3.zero;
            }
        }
        else
        {
            if ((_rigidbody.position - onPosition.position).magnitude >= (offPosition.position - onPosition.position).magnitude)
            {
                velocity = _rigidbody.velocity = Vector3.zero;
            }
        }
    }

    protected override void TurnedOnAction()
    {
        base.TurnedOnAction();

        curDirection = onPosition.position - offPosition.position;
        velocity = _rigidbody.velocity = speed * curDirection.normalized;
    }

    protected override void TurnedOffAction()
    {
        base.TurnedOffAction();

        curDirection = offPosition.position - onPosition.position;
        velocity = _rigidbody.velocity = speed * curDirection.normalized;
    }
}
