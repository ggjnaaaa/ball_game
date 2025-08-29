using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

//~~~~~~    ������ ��� ������ ���� ������� (����� "Menu", ������ � ������� "Levels Menu")    ~~~~~~//
public class LevelsScript : MonoBehaviour
{
    public GameObject levelsMenu;

    void Start()
    {
        //~~~~~~    ���������� ������, ����������� �� ������ ������ ���������� ���������    ~~~~~~//
        GameObject[] objects = GameObject.FindGameObjectsWithTag("levelButton");  // ������ �������� � ����� "levelButton"
        int lastLevel = SavesData.LastOpenedLevel();

        foreach (GameObject btn in objects)
        {
            if (Convert.ToInt32(btn.name.Substring(5)) > lastLevel)  // ����� ��� ������ ������� �������, �� ������� ��� ���������
            {
                btn.transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
        }
    }

    //~~~~~~    ������� �� ������ �������� �� ������ �������    ~~~~~~//
    public void ButtonClick()
    {
        SceneLoader.LoadLevel(
            Convert.ToInt32(
                EventSystem.current.currentSelectedGameObject.name.Substring(5)));
    }
}
