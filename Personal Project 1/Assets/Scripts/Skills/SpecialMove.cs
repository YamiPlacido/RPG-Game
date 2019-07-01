using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;

public class SpecialMove : MonoBehaviour
{
    [SerializeField] private string _skillName;

    public string SkillName => _skillName;
    public bool MassDestruction = false;
    [SerializeField] private SkillType _skilltype;
    public SkillType SType => _skilltype;

    public float speed;
    public int damage = 10;
    public float range = 12;
    public Vector3 offsetPos;
    public int xRotate;
    public int yRotate;
    public int zRotate;
    private float _attackDelay = 0.6f;
    public GameObject hitPrefab;
    public Vector3 startPos;

    public enum SkillType
    {
        active, passive
    }

    private void Awake()
    {
        startPos = transform.position;
        if (MassDestruction == true)
        {
            StartCoroutine(TakeEffect());
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, startPos) > range)
        {
            var hitVFX = Instantiate(hitPrefab, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Character>()?.ModifyHealth(-damage);
            DamagePopup.CreatePopupDamage(other.gameObject.GetComponent<Combat>().pfDamagePopup, other.transform.position, damage, false);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, other.transform.position - transform.position, out hit))
            {
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Vector3 pos = hit.point;

                if (hitPrefab != null)
                {
                    var hitVFX = Instantiate(hitPrefab, pos, rot);
                }
            }
            if (MassDestruction == false)
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator TakeEffect()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
