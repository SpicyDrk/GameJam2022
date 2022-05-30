using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform hpBar, flashBar, expBar, roundBar;
    PlayerController playerCtrl;
    EnemySpawner enemySpawner;
    private float hp;
    private float maxHp;
    [SerializeField] TextMeshProUGUI hpText, expText, roundText, enemiesVanquishedText;

    [SerializeField] TextMeshProUGUI atkVal, atkSpdVal, rangeVal, hpVal;

    [SerializeField] Button atkBtn, atkSpdBtn, rangeBtn, hpButton, restartButton;

    [SerializeField] Image greyScreen, pauseMenuImage;

    private int enemiesVanquished = 0;

    private int levelUps = 1;
    private int roundKillsAdditive = 5;

    private int round = 1;
    private int killsPerRound = 15;
    private int killsForNextRound = 15;
    private int previousKillsNeeded = 0;
    public bool gameIsPaused = true;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerCtrl = player.GetComponent<PlayerController>();
        hp = 100;
        maxHp = 100;
    }
    private void Start()
    {
        atkSpdVal.text = $"{playerCtrl.swingTimer.ToString().PadRight(4,'0')[..4]}";
        enemySpawner = FindObjectOfType(typeof(EnemySpawner)) as EnemySpawner;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        ShowGreyScreen();
        pauseMenuImage.gameObject.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void Resume()
    {
        HideGreyScreen();
        pauseMenuImage.gameObject.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
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
        hpText.text = $"{Mathf.Floor(hp)}/{this.maxHp}";
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

    public void SetExpAsPercent(float expPct) 
    {        
        expText.text = expPct == 1f ? "0%" : $"{Mathf.Floor(expPct*100)}%";
        expBar.localScale = new Vector3(expPct == 1f ? 0f : expPct, 1f, 1f);
    }

    public void SetRoundLevel(int roundLevel)
    {
        roundText.SetText(roundLevel.ToString());
    }

    public void LevelUp()
    {
        levelUps++;
        atkBtn.gameObject.SetActive(true);
        atkSpdBtn.gameObject.SetActive(true);
        rangeBtn.gameObject.SetActive(true);
        hpButton.gameObject.SetActive(true);
    }

    private void DisableButtons()
    {
        levelUps--;
        if (levelUps > 0)
        {
            return;
        }
        atkBtn.gameObject.SetActive(false);
        atkSpdBtn.gameObject.SetActive(false);
        rangeBtn.gameObject.SetActive(false);
        hpButton.gameObject.SetActive(false);
    }

    public void LevelUpAtk()
    {
        playerCtrl.LevelUpAtk();
        atkVal.text = Mathf.Floor(playerCtrl.attackDamage).ToString();
        DisableButtons();
    }
    public void LevelUpAtkSpd()
    {
        playerCtrl.LevelUpAtkSpd();
        atkSpdVal.text = $"{playerCtrl.swingTimer.ToString().PadRight(4, '0')[..4]}";
        DisableButtons();
    }
    public void LevelUpRange()
    {
        playerCtrl.LevelUpRange();
        rangeVal.text = $"{playerCtrl.slashHitBox.transform.localScale.x.ToString().PadRight(4, '0')[..4]}";
        DisableButtons();
    }
    public void LevelUpHp()
    {
        playerCtrl.LevelUpHp();
        hpVal.text = $"{this.maxHp}";
        DisableButtons();
    }

    public void PlayerDied()
    {
        ShowGreyScreen();
        ShowRestartButton();
    }

    private void ShowRestartButton()
    {
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    private void ShowGreyScreen()
    {
        greyScreen.gameObject.SetActive(true);
    }
    private void HideGreyScreen()
    {
        greyScreen.gameObject.SetActive(false);
    }

    public void EnemyKilled()
    {
        enemiesVanquished++;
        enemiesVanquishedText.text = enemiesVanquished.ToString();
        if (enemiesVanquished >= killsForNextRound)
        {
            previousKillsNeeded = enemiesVanquished;
            round++;
            killsForNextRound += killsPerRound + roundKillsAdditive;
            roundText.text = round.ToString();
            enemySpawner.SetRound(round);
        }
        roundBar.localScale = new Vector3((enemiesVanquished - (float)previousKillsNeeded) / (killsForNextRound - (float)previousKillsNeeded), 1f, 1f);
    }

}
