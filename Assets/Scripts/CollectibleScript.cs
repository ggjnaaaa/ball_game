using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float minWindForce = 2f;   // минимальная сила ветра
    public float maxWindForce = 5f;   // максимальная сила ветра
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!collected && other.CompareTag("Player"))
        {
            collected = true;

            // Генерируем случайный вектор ветра
            Vector3 windDir = new Vector3(
                Random.Range(-1f, 1f),
                0f, // если не хотим вертикальный подъем
                Random.Range(-1f, 1f)
            ).normalized;

            float windForce = Random.Range(minWindForce, maxWindForce);

            // Применяем к Rigidbody игрока
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(windDir * windForce, ForceMode.Impulse);

            // Можно тут ещё вызвать метод, который увеличивает скорость шара или счет
            Destroy(gameObject);
        }
    }
}
