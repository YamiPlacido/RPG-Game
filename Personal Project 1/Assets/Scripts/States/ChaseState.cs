using Assets.Scripts.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ChaseState : BaseState
{
    private Character _char;
    private float _attackRange => _char.AttackRange;
    private Animator _animator => _char.GetComponent<Animator>();
    private CharacterController _charController => _char.GetComponent<CharacterController>();

    public ChaseState(Character character) : base(character.gameObject)
    {
        _char = character;
    }

    public override Type Tick()
    {
        Vector3 movement = Vector3.zero;

        if (_char.Target == null && _char.tag == "Player")
        {
            return typeof(NormalState);
        }
        else if (_char.Target == null && _char.tag == "Enemy")
        {
            return typeof(WanderState);
        }

        transform.LookAt(_char.Target);
        //transform.Translate(Vector3.forward * Time.deltaTime * 5f);

        movement = Vector3.forward * 6.0f; //moveSpeed = 6.0f

        movement = transform.TransformDirection(movement);

        _animator.SetFloat("Speed", movement.sqrMagnitude);
        movement *= Time.deltaTime;
        _charController.Move(movement);

        if(_char.tag == "Player")
        {
            if (Vector3.Distance(transform.position, _char.Target.transform.position) <= Managers.Skill.GetLeftAction().range -0.5) //AttackRange = 1f;
            {
                return typeof(AttackState);
            }
        }
        else if(_char.tag == "Enemy")
        {
            if (Vector3.Distance(transform.position, _char.Target.transform.position) <= _char.AttackRange -0.5) //AttackRange = 1f;
            {
                return typeof(AttackState);
            }
        }
            

        return null;
    }
}

