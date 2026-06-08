using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTimerLoader : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "";
    [SerializeField] private float delayInSeconds = 3f;

    private void Start()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(targetSceneName);
    }
}