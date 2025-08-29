using UnityEngine;
using System.Collections.Generic;

public class CollectibleGridGenerator : MonoBehaviour
{
    [Header("Настройки генератора")]
    public GameObject collectiblePrefab;
    public int collectibleCount = 20;
    public float initialSpacing = 3f;
    public float spacingStep = 0.1f;
    public string floorTag = "floor";
    public string obstacleTag = "Obstacle";
    public float checkRadius = 0.5f;
    public float raisingPoints = 1f;
    public GameObject player;
    public float minDistanceToPlayer = 2f;
    public static int TotalCollectibles { get; private set; }

    private Bounds floorBounds;
    private Collider[] obstacleColliders;

    void Awake()
    {
        TotalCollectibles = collectibleCount;
    }

    public void Start()
    {
        InitObstacles(GameObject.FindGameObjectsWithTag(obstacleTag));
        GetFloorBounds();
        Generate();
    }

    private void Generate()
    {
        float spacing = initialSpacing;
        List<Vector3> points = new List<Vector3>();

        while (points.Count < collectibleCount && spacing > 0.1f)
        {
            points.Clear();

            for (float d = floorBounds.min.x + floorBounds.min.z + initialSpacing;
                      d < floorBounds.max.x + floorBounds.max.z - initialSpacing;
                      d += spacing)
            {
                float lastX = floorBounds.min.x;

                for (float x = floorBounds.min.x; x <= floorBounds.max.x;)
                {
                    float z = d - x;
                    if (z < floorBounds.min.z || z > floorBounds.max.z)
                    {
                        x += spacing;
                        continue;
                    }

                    // Рандомное смещение вдоль линии от spacing до spacing*3
                    float step = Random.Range(spacing, spacing * 3f);

                    Vector3 candidate = new Vector3(x, floorBounds.max.y + raisingPoints, z);

                    // Проверка попадания внутрь пола
                    if (IsValidSpawnPoint(candidate, checkRadius))
                    {
                        points.Add(candidate);
                    }

                    lastX = x;
                    x += step;
                }
            }

            spacing -= spacingStep; // уменьшаем плотность, если точек мало
        }

        // Размещаем объекты с учётом поворота
        int placed = 0;
        foreach (var pos in points)
        {
            if (placed >= collectibleCount) break;
            Quaternion randomRotation = Quaternion.Euler(45f, Random.Range(0f, 360f), 45f);
            Instantiate(collectiblePrefab, pos, randomRotation);
            placed++;
        }

        Debug.Log($"Размещено {placed}/{collectibleCount} объектов");
    }


    private void GetFloorBounds()
    {
        var floorRoot = GameObject.FindGameObjectWithTag(floorTag);
        if (floorRoot == null)
        {
            Debug.LogError("Не найден объект с тегом Floor!");
            return;
        }

        // Собираем все коллайдеры дочерних объектов
        Collider[] colliders = floorRoot.GetComponentsInChildren<Collider>();
        Renderer[] renderers = floorRoot.GetComponentsInChildren<Renderer>();

        if (colliders.Length > 0)
        {
            Bounds b = colliders[0].bounds;
            for (int i = 1; i < colliders.Length; i++)
                b.Encapsulate(colliders[i].bounds);
            floorBounds = b;
        }
        else if (renderers.Length > 0)
        {
            Bounds b = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
                b.Encapsulate(renderers[i].bounds);
            floorBounds = b;
        }
        else
        {
            Debug.LogError("Объект Floor и его дети не имеют Collider или Renderer!");
            return;
        }
    }

    private bool IsFree(Vector3 candidate, float radius)
    {
        // Проверка на препятствия
        foreach (var col in obstacleColliders)
        {
            if (col.bounds.SqrDistance(candidate) <= radius * radius)
                return false;
        }

        // Проверка на игрока
        if (player != null)
        {
            float sqrDistToPlayer = (candidate - player.transform.position).sqrMagnitude;
            if (sqrDistToPlayer < minDistanceToPlayer * minDistanceToPlayer)
                return false;
        }

        return true;
    }

    private bool IsValidSpawnPoint(Vector3 candidate, float radius)
    {
        // Проверяем коллизии
        if (!IsFree(candidate, radius))
            return false;

        // Проверяем, что под точкой есть пол
        Ray ray = new Ray(candidate + Vector3.up * 5f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            Transform t = hit.collider.transform.parent;
            if (t != null)
            {
                if (t.CompareTag(floorTag))
                {
                    candidate.y = hit.point.y + raisingPoints;
                    return true;
                }
            }
        }

        return false;
    }


    private void InitObstacles(GameObject[] obstacles)
    {
        List<Collider> colliders = new List<Collider>();
        foreach (var obs in obstacles)
        {
            colliders.AddRange(obs.GetComponentsInChildren<Collider>());
        }
        obstacleColliders = colliders.ToArray();
    }
}
