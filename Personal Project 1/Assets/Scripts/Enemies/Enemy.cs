using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.AI;
using System.Collections.Generic;
using System;
using Assets.Scripts.States;

public class Enemy : MonoBehaviour
{
    public Team Team => _team;
    [SerializeField] private Team _team;
    [SerializeField] private LayerMask _layerMask;

    public float moveSpeed = 6.0f;
    public float roamRadius = 5.0f;
    public float turnSpeed = 1f;
    private float _attackRange = 3f;
    private float _rayDistance = 5.0f;
    private float _stoppingDistance = 1.5f;

    private Vector3 _startPosition;
    private Vector3 _destination;
    private Quaternion _desiredRotation;
    private Vector3 _direction;
    private Character _target;
    private EnemyState _currentState;

    private Animator _animator;
    private CharacterController _charController;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _charController = GetComponent<CharacterController>();
        _startPosition = transform.position;
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        switch (_currentState)
        {
            case EnemyState.Wander:
                {
                    if (NeedsDestination())
                    {
                        GetDestination();
                    }

                    //transform.rotation = _desiredRotation;
                    transform.rotation = Quaternion.Slerp(transform.rotation, _desiredRotation, turnSpeed);

                    movement = Vector3.forward * moveSpeed;

                    movement = transform.TransformDirection(movement);

                    _animator.SetFloat("Speed", movement.sqrMagnitude);

                    var rayColor = IsPathBlock() ? Color.red : Color.green;
                    Debug.DrawRay(transform.position, _direction * _rayDistance, rayColor);

                    while (IsPathBlock())
                    {
                        Debug.Log("Path Blocked");
                        GetDestination();
                    }


                    var targetToAggro = CheckForAggro();
                    if (targetToAggro != null)
                    {
                        _target = targetToAggro.GetComponent<Character>();
                        _currentState = EnemyState.Chase;
                    }

                    break;
                }
            case EnemyState.Chase:
                {
                    if (_target == null)
                    {
                        _currentState = EnemyState.Wander;
                        return;
                    }

                    transform.LookAt(_target.transform);
                    //transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                    movement = Vector3.forward * moveSpeed;

                    movement = transform.TransformDirection(movement);

                    if (Vector3.Distance(transform.position, _target.transform.position) < _attackRange)
                    {
                        _currentState = EnemyState.Attack;
                    }
                    break;
                }
            case EnemyState.Attack:
                {
                    if (_target == null)
                    {
                        _currentState = EnemyState.Wander;
                        return;
                    }

                    if (Vector3.Distance(transform.position, _target.transform.position) > _attackRange)
                    {
                        _currentState = EnemyState.Chase;
                    }

                    Debug.Log("Attack");

                    break;
                }

        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);
        movement *= Time.deltaTime;
        _charController.Move(movement);
    }

    private bool IsPathBlock()
    {
        Ray ray = new Ray(transform.position, _direction);
        var hit = Physics.RaycastAll(ray, _rayDistance, _layerMask);
        return hit.Any();
    }

    private void GetDestination()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * roamRadius;
        randomDirection += _startPosition;

        _destination = new Vector3(randomDirection.x, 1f, randomDirection.z);

        _direction = Vector3.Normalize(_destination - transform.position);
        _direction = new Vector3(_direction.x, 0f, _direction.z);
        _desiredRotation = Quaternion.LookRotation(_direction);
    }

    private bool NeedsDestination()
    {
        if (_destination == Vector3.zero)
        {
            return true;
        }

        var distance = Vector3.Distance(transform.position, _destination);
        if (distance <= _stoppingDistance)
        {
            return true;
        }

        return false;
    }

    Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

    private Transform CheckForAggro()
    {
        float aggroRadius = 5f;

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = transform.position;
        for (int i = 0; i < 24; i++)
        {
            //if(Physics.Raycast(pos,direction,out hit, aggroRadius))
            if (Physics.SphereCast(pos, 0.75f, direction, out hit, aggroRadius))
            {
                GameObject hitObject = hit.transform.gameObject;
                var character = hitObject.GetComponent<Character>();
                if (character != null)
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                    return character.transform;
                }
                else
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(pos, direction * hit.distance, Color.white);
            }
            direction = stepAngle * direction;
        }

        return null;
    }
}

public enum Team
{
    Red,
    Blue
}

public enum EnemyState
{
    Wander,
    Chase,
    Attack
}
