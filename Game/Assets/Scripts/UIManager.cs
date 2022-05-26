using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform hpBar, flashBar, expBar;
    PlayerController playerCtrl;
    private float hp;
    private float maxHp;
    private float currentExp;
    private float expToLevel;
    [SerializeField] TextMeshProUGUI hpText, expText, roundText;

    [SerializeField] Button atkBtn, atkSpdBtn, rangeBtn, hpButton;
    private int atkLvl, atkSpdLvl, rangeLvl, hpLvl;

    private void Awake()
    {
        currentExp = 0;
        expToLevel = 100;
        hp = 100;
        maxHp = 100;
        atkLvl = 1;
        atkSpdLvl = 1;
        rangeLvl = 1;
        hpLvl = 1;
    }

    void Update()
    {
        
    }

    public void SetHp(float hp, float? maxHp = null)
    {
        if (maxHp is not null)
        {
            this.maxHp = (float)maxHp;
        }
        StartCoroutine(HpFlashAnimation(this.hp, hp));
        this.hp = hp;
        hpBar.localScale = new Vector3(hp / this.maxHp, 1f, 1f);
        hpText.text = $"{hp}/{maxHp}";
    }

    private IEnumerator HpFlashAnimation(float hpStart, float hpEnd)
    {
        var startTime = 0f;
        var runTime = 1f;
        while (startTime < runTime)
        {
            flashBar.localScale = Vector3.Lerp(new Vector3(hpStart / maxHp, 1f, 1f), new Vector3(hpEnd / maxHp, 1f, 1f), startTime / runTime);
            startTime += Time.deltaTime;
            yield return null;
        }

        flashBar.localScale = new Vector3(hpEnd / maxHp, 1f, 1f);
        yield return null;    
    }

    public void SetExpAsPercent(float expPct) {
        
        expText.text = expPct == 1f ? "0%" : $"{Mathf.Floor(expPct*100)}%";
        expBar.localScale = new Vector3(expPct == 1f ? 0f : expPct, 1f, 1f);
    }

    public void SetRoundLevel(int roundLevel)
    {
        roundText.SetText(roundLevel.ToString());
    }

    public void LevelUp()
    {
        atkBtn.enabled = true;
        atkSpdBtn.enabled = true;
        rangeBtn.enabled = true;
        hpButton.enabled = true;
    }

    private void LevelUpAtk()
    {
        playerCtrl.LevelUpAtk();
        
    }
    private void LevelUpAtkSpd()
    {
        playerCtrl.LevelUpAtkSpd();
    }
    private void LevelUpRange()
    {
        playerCtrl.LevelUpRange();
    }
    private void LevelUpHp()
    {
        playerCtrl.LevelUpHp();
    }

    public void PlayerDied()
    {

    }
}
