using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.AI;
using System.Collections.Generic;
using System;
using Assets.Scripts.States;

public class Enemy : MonoBehaviour
{
    private Animator _animator => GetComponent<Animator>();
    private Character _char => GetComponent<Character>();

    public delegate void OnEnemyDefeated();
    public OnEnemyDefeated onEnemyDefeatedCallback;

    private void Start()
    {
        _char.onNoHealthCallback += Dying;
        _char.onNoHealthCallback += StopMovement;
    }

    private void StopMovement()
    {
        _char.GetComponent<StateMachine>().enabled = false;
    }

    public void HitReaction()
    {
        //hit reaction
    }

    public void Dying()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        _animator.SetFloat("Speed", 0);
        _animator.SetBool("Walking", false);
        _animator.SetTrigger("Dying");
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
        if(onEnemyDefeatedCallback != null)
        {
            onEnemyDefeatedCallback.Invoke();
        }
    }
}

public enum EnemyState
{
    Wander,
    Chase,
    Attack
}
