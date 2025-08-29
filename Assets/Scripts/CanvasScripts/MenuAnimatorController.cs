using UnityEngine;

public class MenuAnimatorController : MonoBehaviour
{
    private Animator animator;
    private bool isVisible = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Show");
        isVisible = true;
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
        isVisible = false;
    }

    // ���� ����� ��������� �� Animation Event � ����� �������� Hide
    public void DisableObject()
    {
        if (!isVisible)
            gameObject.SetActive(false);
    }
}
