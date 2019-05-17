using UnityEngine;

public class DeviceTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;

    public bool requireKey;

    private void OnTriggerEnter(Collider other)
    {
        if (requireKey)
        {
            return;
        }
        foreach (GameObject target in targets)
        {
            target.SendMessage("Activate");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject target in targets)
        {
            target.SendMessage("Deactivate");
        }
    }
}