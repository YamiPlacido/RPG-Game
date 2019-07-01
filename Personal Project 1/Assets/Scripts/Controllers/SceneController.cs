using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    //[SerializeField] private GameObject _enemyPrefab;

    //private List<GameObject> _enemy;

    //private float _posX;
    //private float _posZ;

    //void Update()
    //{
    //    while(_enemy.Count < 5)
    //    {
    //        StartCoroutine(EnemySpawn());
    //    }
    //}

    //private IEnumerator EnemySpawn()
    //{
    //    _posX = transform.position.x + Random.Range(0, 40);
    //    _posZ = transform.position.z + Random.Range(0, 40);

    //    var spawn = Instantiate(_enemyPrefab, new Vector3(_posX, 0f, _posZ),Quaternion.identity);

    //    _enemy.Add(spawn);

    //    yield return new WaitForSeconds(30);
    //}
}
