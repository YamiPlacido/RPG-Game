using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.States;

[RequireComponent(typeof(StateMachine))]
public class Character : MonoBehaviour
{
    [SerializeField] Transform righthand;
    [SerializeField] private Team _team;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _currentHealth;

    public event Action<float> OnHealthPctChanged = delegate { };
    public Vector3 startPosition;
    public Team Team => _team;
    public float CurrentHealth => _currentHealth;
    public StateMachine StateMachine => GetComponent<StateMachine>();
    public Transform Target { get; private set; }

    private Weapon _currentWeapon;
    private Animator _animator => GetComponent<Animator>();
    private AnimatorOverrideController _overrideAnimator;
    public float AttackRange { get; private set; }
    public int AttackDamage { get; private set; }
    public float AttackSpeed { get;private set; }

    public delegate void OnNoHealth();
    public OnNoHealth onNoHealthCallback;

    public void EquipWeapon(Weapon weaponPrefab)
    {
        if (_currentWeapon != null)
        {
            Destroy(_currentWeapon.gameObject);
        }

        _currentWeapon = Instantiate(weaponPrefab, righthand);
        _currentWeapon.transform.localPosition = _currentWeapon.weaponPosition;
        _currentWeapon.transform.localEulerAngles = _currentWeapon.weaponRotation;

        if(_overrideAnimator == null)
        {
            _overrideAnimator = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _overrideAnimator;
        }
      
        _overrideAnimator["punching"] = _currentWeapon.weaponAnimation;

        AttackRange = _currentWeapon.attackRange;
        AttackDamage = _currentWeapon.attackDamage;
    }

    public void DropWeapon()
    {
        if (_currentWeapon != null)
        {
            Destroy(_currentWeapon.gameObject);
            _overrideAnimator["punching"] = null;

            AttackRange = 1f;
            AttackDamage = 3;
            AttackSpeed = 1f;
        }
    }

    public void ModifyHealth(int amount)
    {
        _currentHealth += amount;
        if(_currentHealth <= 0)
        {
            onNoHealthCallback?.Invoke();
        }

        float currentHealthPct = (float)_currentHealth / (float)_maxHealth;
        OnHealthPctChanged(currentHealthPct);
        Managers.Player.ChangeHealth(amount);
    }

    public void SetTarget(Transform newTarget)
    {
        Target = newTarget;
    }

    public void RemoveTarget()
    {
        Target = null;
    }

    private void Awake()
    {
        Weapon weapon = GetComponent<Weapon>();
        if(weapon != null)
        {
            AttackRange = GetComponent<Weapon>().attackRange;
            AttackDamage = GetComponent<Weapon>().attackDamage;
        }
        else if(gameObject.tag == "Player")
        {
            AttackRange = 1f;
            AttackDamage = 3;
            AttackSpeed = 1f;
        }
        else if(gameObject.tag == "Enemy")
        {
            AttackRange = 1.5f;
            AttackDamage = 3;
            AttackSpeed = 1f;
        }
        startPosition = transform.position;
        _currentHealth = _maxHealth;
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = null;

        if (gameObject.tag == "Player")
        {
            states = new Dictionary<Type, BaseState>()
            {
                { typeof(NormalState),new NormalState(character:this) },
                { typeof(ChaseState),new ChaseState(character:this) },
                { typeof(AttackState),new AttackState(character:this) }
            };
        }
        else
        {
            states = new Dictionary<Type, BaseState>()
            {
                { typeof(WanderState),new WanderState(character:this) },
                { typeof(ChaseState),new ChaseState(character:this) },
                { typeof(AttackState),new AttackState(character:this) }
            };
        }
       
        this.GetComponent<StateMachine>().SetStates(states);
    }
}

public enum Team
{
    Red,
    Blue
}

