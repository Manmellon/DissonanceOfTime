using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Components")]
    public Rigidbody _rigidbody;
    public Animator _animator;
    public AudioSource _audioSource;
    public ParticleSystem _particleSystem;

    [Header("Entity")]
    [SerializeField]private bool _isFreezed;

    [HideInInspector]public Vector3 lastVelocity;

    public bool isFreezed
    {
        get { return _isFreezed; }
        set
        {
            _isFreezed = value;
            if (value)
            {
                FreezeTimeAction();
            }
            else
            {
                UnFreezeTimeAction();
            }
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
        if (_rigidbody)
        {
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
        if (_rigidbody)
        {
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
        }

        if (_particleSystem)
        {
            _particleSystem.Play();
        }

        if (_animator)
            _animator.speed = 1;
    }
}
