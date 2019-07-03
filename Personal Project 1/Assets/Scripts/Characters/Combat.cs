using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;
using Assets.Scripts.States;
using System;
using Random = UnityEngine.Random;

public class Combat:MonoBehaviour
{
    private Animator _animator => GetComponent<Animator>();
    private Character _char => GetComponent<Character>();
    private CharacterController _charController => GetComponent<CharacterController>();
    private StateMachine _state => GetComponent<StateMachine>();
    private float _attackCooldown = 0f;
    private float _attackDelay = 0.6f;
    private GameObject _fireball;
  
    public Transform pfDamagePopup;

    private void Update()
    {
        _attackCooldown -= Time.deltaTime;
    }

    public void Attack(Transform atkTarget, GameObject atk, float atkRange, int atkDamage, float atkSpeed)
    {
        transform.LookAt(_char.Target);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, atkRange) && _attackCooldown <= 0)
        {
            _animator.SetTrigger("Attack");

            if (atk != null)
            {
                CreateProjectile(atk);

                Rigidbody body = _fireball.GetComponent<Rigidbody>();
                body.AddForce(transform.forward * atk.GetComponent<SpecialMove>().speed); //400
                //Destroy(_fireball, 10.0f);
            }
            else
            {
                bool isCriticalHit = Random.Range(0, 100) < 30;
                DamagePopup.CreatePopupDamage(pfDamagePopup, atkTarget.position, atkDamage, isCriticalHit);
            }

            atkTarget.GetComponent<Enemy>()?.HitReaction();
            StartCoroutine(AttackDelay(hit.collider, atk, atkDamage, _attackDelay));

            _attackCooldown = 1f / atkSpeed;
        }
        Debug.DrawRay(transform.position, transform.forward * atkRange, Color.yellow);
    }

    private void CreateProjectile(GameObject atk)
    {
        _fireball = Instantiate(atk, transform.position + atk.GetComponent<SpecialMove>().offsetPos, transform.rotation) as GameObject;
        _fireball.transform.Rotate(Vector3.up * atk.GetComponent<SpecialMove>().yRotate);
        _fireball.transform.Rotate(Vector3.forward * atk.GetComponent<SpecialMove>().xRotate);
        _fireball.transform.Rotate(Vector3.right * atk.GetComponent<SpecialMove>().zRotate);
    }

    public void SpecialAttack(GameObject atk/*,float atkRange, int atkDamage, float atkSpeed*/)
    {
        transform.LookAt(_char.Target);

        if (Vector3.Distance(transform.position, _char.Target.transform.position) <= /*atkRange*/ 25)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, /*atkRange*/ 25) && _attackCooldown <= 0)
            {
                _animator.SetTrigger("Attack");

                if (atk != null)
                {
                    CreateProjectile(atk);

                    Rigidbody body = _fireball.GetComponent<Rigidbody>();
                    body.AddForce(transform.forward * 400);
                    //Destroy(_fireball, 10.0f);
                }
            }
        }
    }

    public IEnumerator AttackDelay(Collider collider, GameObject atk, int atkDamage, float atkDelay)
    {
        yield return new WaitForSeconds(atkDelay);
        _animator.SetTrigger("Attack");

        if (collider != null && atk == null)
        {
            collider.GetComponent<Character>()?.ModifyHealth(-atkDamage);
        }
    }
}
