using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    PlayerStat _stat;
    
    private void Start()
    {
        _stat = GameObject.Find("UnityChan").GetComponent<PlayerStat>();
    }

    void Update()   
    {
        if (Input.inputString == (transform.parent.GetComponent<Slot>().num + 1).ToString())    
        {
            //������ ���
            Debug.Log("hp���� ��� ");
            int hp = _stat.MaxHp;
            _stat.Hp = hp;
            Destroy(this.gameObject);
        }
    }
}
