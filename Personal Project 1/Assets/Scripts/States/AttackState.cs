using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.States;

public class AttackState : BaseState
{
    private float _attackReadyTimer;
    private float _attackRange => _char.AttackRange;
    private Character _char;
    private Animator _animator => _char.GetComponent<Animator>();
    private CharacterController _charController => _char.GetComponent<CharacterController>();

    public AttackState(Character character) : base(character.gameObject)
    {
        _char = character;
    }

    public override Type Tick()
    {
        if (_char.Target == null && _char.tag == "Player")
        {
            return typeof(NormalState);
        }
        else if (_char.Target == null && _char.tag == "Enemy")
        {
            return typeof(WanderState);
        }

        _animator.SetFloat("Speed", 0);

        _attackReadyTimer -= Time.deltaTime;

        if(_char.tag == "Player")
        {
            if (Vector3.Distance(transform.position, _char.Target.position) > Managers.Skill.GetLeftAction().range)
            {
                return typeof(ChaseState);
            }
        }
        else if(_char.tag == "Enemy")
        {
            if (Vector3.Distance(transform.position, _char.Target.position) > _char.AttackRange)
            {
                return typeof(ChaseState);
            }
        }



        //float angle = 10;
        //if (Vector3.Angle(transform.forward, _char.Target.position - transform.position) < angle)
        //{
        //    StartCoroutine(EnemyScan());
        //    transform.LookAt(_char.Target.transform);
        //}


        if (_attackReadyTimer <= 0f)
        {
            if(_char.tag == "Player")
            {
                _char.GetComponent<Combat>().Attack(_char.Target, Managers.Skill.GetLeftAction().gameObject, Managers.Skill.GetLeftAction().range, Managers.Skill.GetLeftAction().damage, _char.AttackSpeed);
            }
            else if(_char.tag == "Enemy")
            {
                _char.GetComponent<Combat>().Attack(_char.Target, null, _char.AttackRange, _char.AttackDamage, _char.AttackSpeed);
            }
        }
        return null;
    }

    private IEnumerator EnemyScan()
    {
        yield return new WaitForSeconds(30);
    }
}

