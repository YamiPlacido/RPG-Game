using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;

public class PlayerManager : MonoBehaviour,IGameManager
{
    public ManagerStatus status { get; set; }

    public int Health => _health;
    public int MaxHealth => _maxHealth;

    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;

    private NetworkService _network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Player manager starting...");

        _network = service;

        UpdateHealth(50, 100);

        status = ManagerStatus.Started;
    }

    public void UpdateHealth(int health, int maxHealth)
    {
        this._health = health;
        this._maxHealth = maxHealth; 
    }

    public void ChangeHealth(int value)
    {
        _health += value;
        if(_health < 0)
        {
            _health = 0;
        }

        if(_health == 0)
        {
            //player loses
        }
    }

    public void Respawn()
    {
        UpdateHealth(50, 100);
    }

}
