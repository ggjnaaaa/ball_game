using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static int _targetSceneIndex = -1;
    private static AsyncOperation _loadingOperation;

    // Загружаем меню без экрана загрузки
    public static void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Перезапуск текущего уровня
    public static void ReloadLevel()
    {
        int current = SceneManager.GetActiveScene().buildIndex - 1;
        LoadLevel(current);
    }

    // Загрузка следующего уровня
    public static void LoadNextLevel()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(current);
    }

    // Главный метод для загрузки уровня (с экраном загрузки)
    public static void LoadLevel(int sceneIndex)
    {
        _targetSceneIndex = sceneIndex + 1;
        SceneManager.LoadScene(1); // всегда сначала грузим экран загрузки
    }

    public static IEnumerator LoadTargetAsync(System.Action<float> onProgress = null, float artificialDelay = 0.01f)
    {
        if (_targetSceneIndex < 0)
        {
            Debug.LogError("SceneLoader: Не задана цель загрузки!");
            yield break;
        }

        _loadingOperation = SceneManager.LoadSceneAsync(_targetSceneIndex);
        _loadingOperation.allowSceneActivation = false;

        while (!_loadingOperation.isDone)
        {
            // op.progress идёт до 0.9f, пока сцена не готова
            float progress = Mathf.Clamp01(_loadingOperation.progress / 0.9f);
            onProgress?.Invoke(progress);

            // Когда всё загружено, активируем
            if (progress >= 1f)
                _loadingOperation.allowSceneActivation = true;

            // Небольшая задержка, чтобы анимация успевала проигрываться
            yield return new WaitForSeconds(artificialDelay);
        }
    }
}
