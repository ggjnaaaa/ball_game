using UnityEngine;
using System;

//~~~~~~    ������ ��� ����������� Dead Zone    ~~~~~~//
// ��������� ����������� ������ �������� � ��������� � ����� "�����"
public class DeadZoneScript : MonoBehaviour
{
    public static event Action OnPlayerLose;

    public GameObject lose;
    public GameObject panel;
    public GameObject player;

    private bool isLose = false;

    private void Update()
    {
        //~~~~~~    ���������� ������� ���� ������ (������� ���� ������ �� ��� y ������� �������)    ~~~~~~//
        float temp = transform.position.y;
        Vector3 vec = player.transform.position - transform.position;
        vec.y = temp;
        transform.position = vec;

        //~~~~~~    �� ������ ���� ����� �����, �� ���� � ����� �� �����������    ~~~~~~//
        if (player.transform.position.y < transform.position.y)
            end();
    }

    private void OnTriggerEnter(Collider other)
    {
        //~~~~~~    ���� ����������� ��������� � ��������� � ����� "�����"    ~~~~~~//
        if (other.tag == "Player")
            end();
    }

    //~~~~~~    ��������� ���� ���������    ~~~~~~//
    private void end()
    {
        if (isLose) return;
        isLose = true;
        OnPlayerLose?.Invoke();
    }
}
