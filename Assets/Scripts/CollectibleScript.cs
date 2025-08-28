using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float minWindForce = 2f;   // ����������� ���� �����
    public float maxWindForce = 5f;   // ������������ ���� �����
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!collected && other.CompareTag("Player"))
        {
            collected = true;

            // ���������� ��������� ������ �����
            Vector3 windDir = new Vector3(
                Random.Range(-1f, 1f),
                0f, // ���� �� ����� ������������ ������
                Random.Range(-1f, 1f)
            ).normalized;

            float windForce = Random.Range(minWindForce, maxWindForce);

            // ��������� � Rigidbody ������
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(windDir * windForce, ForceMode.Impulse);

            // ����� ��� ��� ������� �����, ������� ����������� �������� ���� ��� ����
            Destroy(gameObject);
        }
    }
}
