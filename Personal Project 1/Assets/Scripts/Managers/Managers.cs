using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(SkillManager))]

public class Managers : MonoBehaviour
{
    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get;private set; }
    public static SkillManager Skill { get; private set; }

    private List<IGameManager> _startSequence;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();
        Skill = GetComponent<SkillManager>();

        _startSequence = new List<IGameManager>();

        _startSequence.Add(Player);
        _startSequence.Add(Inventory);
        _startSequence.Add(Skill);

        StartCoroutine(StartupManager());
    }

    private IEnumerator StartupManager()
    {
        NetworkService service = new NetworkService();

        foreach (var manager in _startSequence)
        {
            manager.Startup(service);
        }

        yield return null;

        int numModules = _startSequence.Count;
        int numReady = 0;

        while(numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach (var manager in _startSequence)
            {
                if(manager.status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }

            if(numReady > lastReady)
            {
                Debug.Log("Progress: " + numReady + "/" + numModules);
            }
            yield return null;
        }
        Debug.Log("All managers started up");
    }
}
