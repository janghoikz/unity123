using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    public int _exp;

    public int Exp { get { return _exp; } set { _exp = value; } }

    private void Start()
    {
        _level = 1;
        _hp = 1;
        _maxHp = 100;
        _attack = 20;
        _defense = 10;
        _speed = 10.0f;
        _exp = 0;
    }
    private void Update()
    {
        if (_exp == 100)
        {
            Debug.Log("·¹º§¾÷");
            _level = 2;
            _hp = 150;
            _maxHp = 150;
            _attack = 50;
            _defense = 12;
            _speed = 10.0f;
            _exp = 0;
        }
    }
}
