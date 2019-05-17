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

    public Vector3 startPosition;
    public Team Team => _team;
    public StateMachine StateMachine => GetComponent<StateMachine>();
    public Transform Target { get; private set; }

    private Weapon _currentWeapon;
    public float AttackRange { get; private set; }
    public int AttackDamage { get; private set; }
    public float AttackSpeed { get;private set; }

    public void EquipWeapon(Weapon weaponPrefab)
    {
        if(_currentWeapon != null)
        {
            Destroy(_currentWeapon.gameObject);
            _currentWeapon = Instantiate(weaponPrefab, righthand);
            _currentWeapon.transform.localPosition = _currentWeapon.weaponPosition;
            _currentWeapon.transform.localEulerAngles = _currentWeapon.weaponRotation;
        }
        else
        {
            _currentWeapon = Instantiate(weaponPrefab, righthand);
            _currentWeapon.transform.localPosition = _currentWeapon.weaponPosition;
            _currentWeapon.transform.localEulerAngles = _currentWeapon.weaponRotation;
        }
    }

    public void TakeDamage(int damage)
    {
        Managers.Player.ChangeHealth(-damage);
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
        else
        {
            AttackRange = 3f;
            AttackDamage = 3;
            AttackSpeed = 1f;
        }
        startPosition = transform.position;
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
