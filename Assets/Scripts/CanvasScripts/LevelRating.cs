using UnityEngine;
using UnityEngine.UI;

public class LevelRating : MonoBehaviour
{
    public GameObject starPrefab;
    private int _maxCollectible;

    void Awake()
    {
        _maxCollectible = CollectibleGridGenerator.TotalCollectibles;
    }

    void OnEnable()
    {
        PlayerScript.OnLevelCompleted += CalculateRating;
    }

    void OnDisable()
    {
        PlayerScript.OnLevelCompleted -= CalculateRating;
    }

    private void CalculateRating(float time, int collected)
    {
        // ���� �� ������� ��� �������
        if (collected < _maxCollectible)
        {
            SetStars(1);
            return;
        }

        // ������� ������ �� �������
        int rating = 1;
        if (time < 60f) rating = 3;
        else if (time < 120f) rating = 2;

        SetStars(rating);
    }

    private void SetStars(int count)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        for (int i = 0; i < count; i++)
        {
            GameObject star = Instantiate(starPrefab, transform);
            star.transform.localScale = Vector3.one;

            if (star.TryGetComponent<RectTransform>(out RectTransform rt))
                rt.anchoredPosition = Vector2.zero;
        }
    }
}
