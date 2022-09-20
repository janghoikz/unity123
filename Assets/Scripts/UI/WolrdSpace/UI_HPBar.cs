using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    [SerializeField]
    GameObject target;

    Stat _stat;

    enum GameObjects
    {
        HPBar
    }
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObject));
    }
    private void Start()
    {
        _stat = target.GetComponent<Stat>();
    }
    private void Update()
    {
        transform.position = target.transform.position + new Vector3(0, 2, 0);
        if (target.name == "Undead_Knight")
        { transform.position += new Vector3(0, 1, 0); }
        transform.rotation = Camera.main.transform.rotation;
        float ratio = _stat.Hp / (float)_stat.MaxHp;
        SetHpRatio(ratio);
        if (_stat.Hp <= 0)
        { 
            Destroy(this.gameObject);
        }
    }

    public void SetHpRatio(float ratio)
    { 
        GetComponent<Slider>().value = ratio;
    }
    
}
