using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static int _targetSceneIndex = -1;
    private static AsyncOperation _loadingOperation;

    // ��������� ���� ��� ������ ��������
    public static void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    // ���������� �������� ������
    public static void ReloadLevel()
    {
        int current = SceneManager.GetActiveScene().buildIndex - 1;
        LoadLevel(current);
    }

    // �������� ���������� ������
    public static void LoadNextLevel()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(current);
    }

    // ������� ����� ��� �������� ������ (� ������� ��������)
    public static void LoadLevel(int sceneIndex)
    {
        _targetSceneIndex = sceneIndex + 1;
        SceneManager.LoadScene(1); // ������ ������� ������ ����� ��������
    }

    public static IEnumerator LoadTargetAsync(System.Action<float> onProgress = null, float artificialDelay = 0.01f)
    {
        if (_targetSceneIndex < 0)
        {
            Debug.LogError("SceneLoader: �� ������ ���� ��������!");
            yield break;
        }

        _loadingOperation = SceneManager.LoadSceneAsync(_targetSceneIndex);
        _loadingOperation.allowSceneActivation = false;

        while (!_loadingOperation.isDone)
        {
            // op.progress ��� �� 0.9f, ���� ����� �� ������
            float progress = Mathf.Clamp01(_loadingOperation.progress / 0.9f);
            onProgress?.Invoke(progress);

            // ����� �� ���������, ����������
            if (progress >= 1f)
                _loadingOperation.allowSceneActivation = true;

            // ��������� ��������, ����� �������� �������� �������������
            yield return new WaitForSeconds(artificialDelay);
        }
    }
}
