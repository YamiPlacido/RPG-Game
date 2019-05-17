using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class WanderState : BaseState
    {
        private Vector3? _destination;
        private float stopDistance = 1f;
        private float turnSpeed = 0.2f;
        private LayerMask _layerMask => _char.GetComponent<StateMachine>()._layerMask;
        private float _rayDistance = 3f;
        private Quaternion _desiredRotation;
        private Vector3 _direction;
        private Character _char;
        private Animator _animator => _char.GetComponent<Animator>();
        private Vector3 _startPosition => _char.GetComponent<Character>().startPosition;
        private CharacterController _charController => _char.GetComponent<CharacterController>();

        public WanderState(Character character) : base(character.gameObject)
        {
            _char = character;
        }

        public override Type Tick()
        {
            Vector3 movement = Vector3.zero;

            var chaseTarget = CheckForAggro();
            if (chaseTarget != null)
            {
                _char.SetTarget(chaseTarget);
                return typeof(ChaseState);
            }

            if (_destination.HasValue == false || Vector3.Distance(transform.position, _destination.Value) <= stopDistance)
            {
                FindRandomDestination();
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, _desiredRotation, turnSpeed);

            if (IsForwardBlocked())
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, _desiredRotation, 0.2f);
            }
            else
            {
                movement = Vector3.forward * 6f; ;     //moveSpeed = 6f

                movement = transform.TransformDirection(movement);
            }

            Debug.DrawRay(transform.position, _direction * _rayDistance, Color.red);
            while (IsPathLocked())
            {
                FindRandomDestination();
                Debug.Log("WALL!");
            }

            _animator.SetFloat("Speed", movement.sqrMagnitude);
            movement *= Time.deltaTime;
            _charController.Move(movement);

            return null;
        }

        private bool IsPathLocked()
        {
            Ray ray = new Ray(transform.position, _direction);
            return Physics.SphereCast(ray, 0.5f, _rayDistance, _layerMask);

            //Ray ray = new Ray(transform.position, _direction);
            //var hit = Physics.RaycastAll(ray, _rayDistance, _layerMask);
            //return hit.Any();
        }

        private bool IsForwardBlocked()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            return Physics.SphereCast(ray, 0.5f, _rayDistance, _layerMask);

            //Ray ray = new Ray(transform.position, transform.forward);
            //var hit = Physics.RaycastAll(ray, _rayDistance, _layerMask);
            //return hit.Any();
        }

        private void FindRandomDestination()
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 5.0f; //roamRadius = 5.0f
            randomDirection += _startPosition;

            _destination = new Vector3(randomDirection.x, 1f, randomDirection.z);

            _direction = Vector3.Normalize(_destination.Value - transform.position);
            _direction = new Vector3(_direction.x, 0f, _direction.z);
            _desiredRotation = Quaternion.LookRotation(_direction);
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
}
