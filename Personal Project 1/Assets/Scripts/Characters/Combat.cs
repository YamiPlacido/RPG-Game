using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;

public class Combat:MonoBehaviour
{
    private Animator _animator => GetComponent<Animator>();
    private float _attackCooldown = 0f;
    private float _attackDelay = 0.6f;

    public Transform pfDamagePopup;

    private void Update()
    {
        _attackCooldown -= Time.deltaTime;
    }

    public void Attack(Transform atkTarget, float atkRange, int atkDamage, float atkSpeed)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray,0.75f,out hit,atkRange) && _attackCooldown <=0)
        {
            _animator.SetTrigger("Attack");
            StartCoroutine(DealDamage(hit, atkDamage, _attackDelay));
            Debug.Log(atkTarget.name + " takes " + atkDamage + " damage");
            bool isCriticalHit = Random.Range(0, 100) < 30;
            DamagePopup.CreatePopupDamage(pfDamagePopup, atkTarget.position, atkDamage,isCriticalHit);
            _attackCooldown = 1f / atkSpeed;
        }
    }

    private IEnumerator DealDamage(RaycastHit hit,int atkDamage, float atkDelay)
    {
        yield return new WaitForSeconds(atkDelay);
        hit.collider.GetComponent<Character>()?.TakeDamage(atkDamage);
    }
}
