using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//~~~~~~    ������ ��� ������ �������� ���� (����� "Menu", ������ � ������� "Main Menu")    ~~~~~~//
public class MainMenuScript : MonoBehaviour
{
    public int? level;
    public Button resumeBtn;  // ������ "���������� ����"
    public Button levelsBtn;  // ������ "������"

    void Start()
    {
        //~~~~~~    ���� ���������� ���, �� �� ������ "���������� ����" � "������" ������ ������    ~~~~~~//
        if (!File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            resumeBtn.interactable = false;
            levelsBtn.interactable = false;
        }
    }

    //~~~~~~    ����� ��� ������ "����� ����"    ~~~~~~//
    public void NewGame()
    {
        // � ���������� � ������ ������ ��������������� ������� 1
        level = 1;
        SavesData.Save(1);

        // ����� ����������������� � �������� "���������� ����" � "������"
        resumeBtn.interactable = false;
        levelsBtn.interactable = false;

        // �������� ������� ������
        SceneManager.LoadScene(Convert.ToInt32(level));
    }

    //~~~~~~    ����� ��� ������ "���������� ����"    ~~~~~~//
    public void Resume()
    {
        // �������� ������������� �����
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            // ���� � ����� ����� �������� ���� ���������� �������, �� ����������� ��������� �������
            level = SavesData.LastOpenedLevel() > SavesData.CountLevels() ? SavesData.CountLevels() : SavesData.LastOpenedLevel();
            SceneManager.LoadScene(Convert.ToInt32(level));
        }
    }

    //~~~~~~    ����� ��� ������ "�����"    ~~~~~~//
    public void Quit()
    {
        if (level != null)
            SavesData.Save(Convert.ToInt32(level));

        Application.Quit();
    }
}
