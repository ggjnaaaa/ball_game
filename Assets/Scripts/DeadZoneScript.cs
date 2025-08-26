using UnityEngine;

//~~~~~~    ������ ��� ����������� Dead Zone    ~~~~~~//
// ��������� ����������� ������ �������� � ��������� � ����� "�����"
public class DeadZoneScript : MonoBehaviour
{
    public GameObject lose;
    public GameObject panel;
    public GameObject player;

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
        lose.SetActive(true);
        panel.SetActive(true);
    }
}
