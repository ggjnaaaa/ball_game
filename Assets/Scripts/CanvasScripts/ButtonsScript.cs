using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//~~~~~~    Скрипт для разных кнопок    ~~~~~~//
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
        Debug.Log(timeElapsed);

        if (timeElapsed >= countdown)
        {
            if (isWin) NextLevel();
            else ReplayLevel();
            isCountingDown = false;
        }
    }

    private void ShowPanel(bool win)
    {
        Debug.Log("OBIHHIBN");
        if (isCountingDown) return;
        Debug.Log("sdfkvm;fokl");

        timeElapsed = 0f;
        isWin = win;
        isCountingDown = true;

        panel.SetActive(true);
        if (win) winMenu.SetActive(true);
        else loseMenu.gameObject.SetActive(true);
    }

    //~~~~~~    Открывает сцену "Menu" (Кнопка "В меню")    ~~~~~~//
    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }

    //~~~~~~    Снова запускает тот же уровень (кнопка "Новая игра" в меню проигрыша)    ~~~~~~//
    public void ReplayLevel()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //~~~~~~    Запускает следующий уровень (кнопка "Следующий уровень" в меню выигрыша)    ~~~~~~//
    public void NextLevel()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            int level = SavesData.CurrentLevel();
            int countLevels = SavesData.CountLevels();

            Debug.Log(level);
            Debug.Log(countLevels);

            if (level < countLevels && level != -1)
                SceneManager.LoadScene(Convert.ToInt32(level + 1));
            else if (level >= countLevels)
            {
                winMenu.SetActive(false);
                finish.SetActive(true);
            }
        }
    }
}
