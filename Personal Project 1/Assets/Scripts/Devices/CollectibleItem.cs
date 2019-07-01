using UnityEngine;
using System.Collections;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string _itemName;
    public float radius = 3.5f;

    public string ItemName => _itemName;

    private void OnTriggerEnter(Collider other)
    {
        Managers.Inventory.AddItem(ItemName);
        Destroy(this.gameObject);
    }

    //public void OnMouseDown()
    //{
    //    Transform player = GameObject.FindWithTag("Player").transform;
    //    if (Vector3.Distance(player.position, transform.position) < radius)
    //    {
    //        Vector3 direction = transform.position - player.position;
    //        if (Vector3.Dot(player.forward, direction) > .5f)
    //        {
    //            Managers.Inventory.AddItem(ItemName);
    //            Destroy(this.gameObject);
    //        }
    //    }
    //}
}
