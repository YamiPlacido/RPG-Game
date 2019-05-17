using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]

public class Managers : MonoBehaviour
{
    public static PlayerManager Player { get; private set; }

    private List<IGameManager> _startSequence;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Player = GetComponent<PlayerManager>();

        _startSequence = new List<IGameManager>();

        _startSequence.Add(Player);

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
