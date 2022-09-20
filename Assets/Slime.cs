using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    Stat _stat;
    
    PlayerStat _playerStat;

    void Start()
    {
        _stat = GetComponent<Stat>();
        _playerStat = player.GetComponent<PlayerStat>();
    }
    private void Update()
    {
        if (_stat.Hp <= 0)
        {
            Destroy(this.gameObject);
            _playerStat.Exp += 100;
        }
    }
    
}
