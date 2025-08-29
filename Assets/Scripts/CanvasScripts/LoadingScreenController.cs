using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private Slider progressBar;

    void Start()
    {
        StartCoroutine(SceneLoader.LoadTargetAsync(UpdateProgress));
    }

    private void UpdateProgress(float progress)
    {
        if (progressBar != null)
            progressBar.value = progress;
    }
}
