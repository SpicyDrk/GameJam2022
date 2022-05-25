using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform hpBar, flashBar, expBar;
    private float hp;
    private float maxHp;
    private float currentExp;
    private float expToLevel;
    [SerializeField] TextMeshProUGUI hpText, expText;

    private void Awake()
    {
        currentExp = 0;
        expToLevel = 100;
        hp = 100;
        maxHp = 100;
    }

    void Update()
    {
        
    }

    public void SetHp(float hp)
    {
        StartCoroutine(HpFlashAnimation(this.hp, hp));
        this.hp = hp;
        hpBar.localScale = new Vector3(hp / maxHp, 1f, 1f);
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

    /**
     * Sets exp bar %
     */
    public void SetExpAsPercent(float expPct) {
        
        expText.text = expPct == 1f ? "0%" : $"{Mathf.Floor(expPct*100)}%";
        expBar.localScale = new Vector3(expPct == 1f ? 0f : expPct, 1f, 1f);
    }

    public void PlayerDied()
    {

    }
}
