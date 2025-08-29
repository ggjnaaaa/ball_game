using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//~~~~~~    ������ ��� ������ ������    ~~~~~~//
public class ButtonsScript : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerScript.OnPlayerWin += StartWinTimer;
        DeadZoneScript.OnPlayerLose += StartLoseTimer;
    }

    private void OnDisable()
    {
        PlayerScript.OnPlayerWin -= StartWinTimer;
        DeadZoneScript.OnPlayerLose -= StartLoseTimer;
    }

    public GameObject finish;
    public GameObject winMenu;
    public GameObject panel;
    public GameObject loseMenu;

    public Text WinTimer;
    public Text LoseTimer;

    public void StartWinTimer() => ShowPanel(true);
    public void StartLoseTimer() => ShowPanel(false);

    private const float countdown = 5f;
    private float timeElapsed = 0f;
    private bool isCountingDown = false;
    private bool isWin = false;

    void Update()
    {
        if (!isCountingDown) return;

        timeElapsed += Time.deltaTime;
        float displayTime = Mathf.Ceil(countdown - timeElapsed);

        WinTimer.text = $"Next level in: {displayTime}";
        LoseTimer.text = $"Replay in: {displayTime}";

        if (timeElapsed >= countdown)
        {
            if (isWin) NextLevel();
            else ReplayLevel();
            isCountingDown = false;
        }
    }

    private void ShowPanel(bool win)
    {
        if (isCountingDown) return;

        timeElapsed = 0f;
        isWin = win;
        isCountingDown = true;

        GameObject obj;
        if (win) obj = winMenu;
        else obj = loseMenu;

        var mac = panel.GetComponent<MenuAnimatorController>();
        mac.Show();
        mac = obj.GetComponent<MenuAnimatorController>();
        mac.Show();
    }

    //~~~~~~    ��������� ����� "Menu" (������ "� ����")    ~~~~~~//
    public void ToMenu()
    {
        SceneLoader.LoadMenu();
    }

    //~~~~~~    ����� ��������� ��� �� ������� (������ "����� ����" � ���� ���������)    ~~~~~~//
    public void ReplayLevel()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            SceneLoader.ReloadLevel();
        }
    }

    //~~~~~~    ��������� ��������� ������� (������ "��������� �������" � ���� ��������)    ~~~~~~//
    public void NextLevel()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            int level = SavesData.CurrentLevel();
            int countLevels = SavesData.CountLevels();

            if (level < countLevels && level != -1)
            {
                SceneLoader.LoadNextLevel();
                SavesData.Save(level);
            }
            else if (level >= countLevels)
            {
                winMenu.SetActive(false);
                finish.SetActive(true);
            }
        }
    }
}
