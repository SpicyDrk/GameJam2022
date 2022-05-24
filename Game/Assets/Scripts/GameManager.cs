using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RectTransform hpBar;
    private float hp;
    private float maxHp;

    private void Awake()
    {
        hp = 100;
        maxHp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHp(float hp)
    {
        this.hp = hp;
        hpBar.localScale = new Vector3(hp / maxHp, 1f, 1f);
    }

    public void PlayerDied()
    {

    }
}
