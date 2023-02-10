using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Components")]
    public Rigidbody _rigidbody;
    public Collider _collider;
    public Animator _animator;
    public AudioSource _audioSource;
    public ParticleSystem _particleSystem;

    [Header("Entity")]
    public bool isDraggable;
    public bool resetRotationAfterDrop;
    [SerializeField]private bool _isFreezed;

    private bool unfreezeUseGravity;
    private bool unfreezeIsKinematic;

    public bool isFreezed
    {
        get { return _isFreezed; }
        set
        {
            if (value && !_isFreezed)
            {
                FreezeTimeAction();
            }
            else if (!value && _isFreezed)
            {
                UnFreezeTimeAction();
            }
            _isFreezed = value;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FreezeTimeAction()
    {
        Debug.Log("FreezeAction");
        if (_rigidbody)
        {
            unfreezeIsKinematic = _rigidbody.isKinematic;
            unfreezeUseGravity = _rigidbody.useGravity;

            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
        }

        if (_particleSystem)
        {
            _particleSystem.Pause();
        }

        if (_animator)
            _animator.speed = 0;
    }

    protected virtual void UnFreezeTimeAction()
    {
        Debug.Log("UnFreezeAction");
        if (_rigidbody)
        {
            _rigidbody.useGravity = unfreezeUseGravity;
            _rigidbody.isKinematic = unfreezeIsKinematic;
        }

        if (_particleSystem)
        {
            _particleSystem.Play();
        }

        if (_animator)
            _animator.speed = 1;
    }
}
