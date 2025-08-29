using System;
using UnityEngine;
using UnityEngine.UI;

//~~~~~~    Скрипт управления игроком    ~~~~~~//
public class PlayerScript : MonoBehaviour
{
    public static event Action OnPlayerWin;
    public static event Action<float, int> OnLevelCompleted;

    public string CollectibleTag = "Collectible";
    public Camera cam;
    public GameObject player;
    public GameObject deadZone;

    public Slider speedSlider;

    public GameObject panel;
    public GameObject win;
    public GameObject lose;

    LineRenderer line;  // Линия для определения направления
    Rigidbody rb;       // Rigidbody игрока
    float speed;        // Скорость, задающаяся пользователем
    bool isPlay;        // Проверка активна ли сейчас игра
    bool isNotFly;      // Проверка летит ли объект

    private int score = 0;
    public int Score
    {
        get => score;
        private set
        {
            ScoreText.text = $"Collect:\n{value.ToString()}";
            score = value;
        }
    }
    public Text ScoreText;
    private float timeElapsed = 0f;
    public float TimeElapsed
    {
        get => timeElapsed;
        private set
        {
            timeElapsed = value;
            UpdateTimerUI();
        }
    }
    public Text TimerText;

    void Start()
    {
        //~~~~~~    Задаются дефолтные значения    ~~~~~~//
        rb = player.GetComponent<Rigidbody>();
        line = player.GetComponent<LineRenderer>();
        isPlay = true;
        isNotFly = true;
        TimeElapsed = 0f;
    }

    void Update()
    {
        if (isPlay)
        {
            TimeElapsed += Time.deltaTime;

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position);

            //~~~~~~    Если мышка указывает на любой объект и игрок не летит    ~~~~~~//
            if (Physics.Raycast(ray, out hit) && isNotFly)
            {
                //~~~~~~    Обновление указателя    ~~~~~~//
                line.SetPosition(1, CalculatingVector(hit));

                //~~~~~~    Наращевание скорости пока нажата ЛКМ и толчок шара когда ЛКМ отжалась после нажатия    ~~~~~~//
                if (Input.GetAxis("Fire1") == 1 && speed <= speedSlider.maxValue)
                {
                    speed += 0.1f;
                    speedSlider.value = speed;
                }
                if (Input.GetMouseButtonUp(0) && speed != 0)
                {
                    Vector3 vect = (hit.point - player.transform.position) * speed * 5;
                    rb.AddForce(vect);
                    speed = 0;
                    speedSlider.value = 0;
                }
            }
        }

        //~~~~~~    Если игра закончена, игрок останавливается    ~~~~~~//
        if (!isPlay)
            rb.velocity = Vector3.zero;

        //~~~~~~    Если игрок находится ниже зоны смерти, то игра останавливается    ~~~~~~//
        if (transform.position.y < deadZone.transform.position.y)
            isPlay = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //~~~~~~    Если игрок коснулся объекта с тэгом "Hole"    ~~~~~~//
        if (collision.transform.CompareTag("Hole") && isPlay)
        {
            //~~~~~~    Открытие меню победы    ~~~~~~//
            OnPlayerWin?.Invoke();
            OnLevelCompleted?.Invoke(TimeElapsed, score);

            //~~~~~~    Сохранение данных если уровень не был пройден ранее    ~~~~~~//
            int level = SavesData.CurrentLevel();

            if (level == SavesData.LastOpenedLevel())
                SavesData.Save(level + 1);

            isPlay = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //~~~~~~    Если игрок коснулся объекта с тэгом "Collectible"    ~~~~~~//
        if (other.CompareTag(CollectibleTag) && isPlay)
        {
            Score++;
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        if (TimerText != null)
            TimerText.text = $"Timer:\n{minutes:00}:{seconds:00}";
    }

    //~~~~~~    Если игрок перестал касаться чего-либо, то он летит    ~~~~~~//
    public void OnCollisionExit(Collision collision) => isNotFly = false;

    //~~~~~~    Если игрок стоит на чем-либо, то он не летит    ~~~~~~//
    public void OnCollisionStay(Collision collision) => isNotFly = true;

    //~~~~~~    Расчет вектора для линии    ~~~~~~//
    private Vector3 CalculatingVector(RaycastHit hit)
    {
        Vector3 nodet = hit.point;
        Vector3 nodef = transform.position;
        float step = 1.5f;

        float dx = nodet.x - nodef.x;
        float dy = nodet.y - nodef.y;
        float dz = nodet.z - nodef.z;

        float r = (float)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2) + Math.Pow(dz, 2));
        float xx = dx * ((float)step / r);
        float zz = dz * ((float)step / r);

        return new Vector3(nodef.x + xx, nodef.y, nodef.z + zz);
    }
}
