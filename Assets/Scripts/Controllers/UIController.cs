using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour 
{
    public static UIController Instance;
    
    public LevelController levelController;

    public GameObject PanelGameplay;
    public GameObject PanelMain;
    public GameObject PanelFail;
    public GameObject PanelWin;
    public GameObject PanelStat;
    public TextMeshProUGUI TextHealth;
    public TextMeshProUGUI TextCount;
    public TextMeshProUGUI TextSpeedBonus;
    public TextMeshProUGUI TextShieldBonus;

    public Transform parentStatPanel;
    public UIStatPanel prefabStatPanel;

    private void Awake()
    {
        Instance = this;
        AllDisable();
        PanelMain.Activate();
    }

    private void Update()
    {
        if (RunnerPlayer.Instance.TimerSpeedShow > 0f)
        {
            TextSpeedBonus.enabled = true;
            TextSpeedBonus.text = $"SPEED:{RunnerPlayer.Instance.TimerSpeedShow.ToString("0")} s";
        }
        else 
        {
            TextSpeedBonus.enabled = false;
        }

        if (RunnerPlayer.Instance.TimerShieldShow > 0f)
        {
            TextShieldBonus.enabled = true;
            TextShieldBonus.text = $"SHIELD:{RunnerPlayer.Instance.TimerShieldShow.ToString("0")} s";
        }
        else
        {
            TextShieldBonus.enabled = false;
        }
    }

    public void ButtonStartLevel()
    {
        levelController.StartLevel();
        RunnerPlayer.Instance.Play();
        AllDisable();
        PanelGameplay.Activate();
    }

    private void AllDisable()
    {
        PanelGameplay.Deactivate();
        PanelMain.Deactivate();
        PanelFail.Deactivate();
        PanelWin.Deactivate();
        PanelStat.Deactivate();
    }

    public void UpdateGameplay()
    {
        TextHealth.text = $"HEALTH:{RunnerPlayer.Instance.Health}";
        TextCount.text = $"BLOCKS:{RunnerPlayer.Instance.CountAllBlocks}";
    }

    public void OpenFail(Dictionary<string, int> stat)
    {
        AllDisable();
        PanelFail.Activate();
        UpdateStat(stat);
    }

    public void OpenWin(Dictionary<string, int> stat)
    {
        AllDisable();
        PanelWin.Activate();
        UpdateStat(stat);
    }

    private void UpdateStat(Dictionary<string, int> stat)
    {
        PanelStat.Activate();
        foreach (Transform child in parentStatPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (var block in stat)
        {
            Instantiate(prefabStatPanel, parentStatPanel).UpdateField(block.Key, block.Value);
        }
    }

    public void ButtonFailRestart()
    {
        ButtonStartLevel();
    }

    public void ButtonFailContinue()
    {
        AllDisable();
        PanelGameplay.Activate();
        RunnerPlayer.Instance.timerSlowly = 2f;
        RunnerPlayer.Instance.Health = 3;
        RunnerPlayer.Instance.RestoreAfterDamage();
    }

    public void ButtonWinNext()
    {
        ButtonStartLevel();
    }
}