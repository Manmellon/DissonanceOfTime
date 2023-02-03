using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Rigidbody _rigidbody;

    public bool isFreezed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void FreezeTime()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;

        isFreezed = true;
    }

    public virtual void UnFreezeTime()
    {
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;

        isFreezed = false;
    }
}
