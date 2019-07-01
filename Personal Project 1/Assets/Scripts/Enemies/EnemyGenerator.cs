using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _amount;
    [SerializeField] private float _posRange;

    public int EnemyCount;
    private float _posX;
    private float _posZ;

    private void Start()
    {
        StartSpawn(5f);
    }

    private void StartSpawn(float wait)
    {
        StartCoroutine(EnemySpawn(wait, _amount, _posRange));
    }

    private IEnumerator EnemySpawn(float wait, int amount, float posRange)
    {
        while (EnemyCount < amount)
        {
            _posX = transform.position.x + Random.Range(-posRange, posRange);
            _posZ = transform.position.z + Random.Range(-posRange, posRange);

            yield return new WaitForSeconds(wait);

            GameObject spawn = Instantiate(_enemyPrefab, new Vector3(_posX, 0f, _posZ), Quaternion.identity) as GameObject;

            EnemyCount++;

            spawn.GetComponent<Enemy>().onEnemyDefeatedCallback += UpdateEnemy;
        }
    }

    private void UpdateEnemy()
    {
        EnemyCount--;
        StartSpawn(20f);
    }
}
