using UnityEngine;
using System.Collections;
using Assets.Scripts.States;

public class Weapon : MonoBehaviour
{
    public Vector3 weaponPosition;
    public Vector3 weaponRotation;
    public AnimationClip weaponAnimation;
    public float attackRange;
    public int attackDamage;

    public ParticleSystem trail;
    private BaseState state => GetComponentInParent<StateMachine>().CurrentState;

    private void Awake()
    {
        trail.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(state?.GetType() == typeof(AttackState))
        {
            trail.gameObject.SetActive(true);
        }
        else
        {
            trail.gameObject.SetActive(false);
        }
    }
}
