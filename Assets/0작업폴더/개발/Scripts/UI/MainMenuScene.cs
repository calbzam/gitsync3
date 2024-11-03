using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{
    [SerializeField] private FadeInOut _fadeInOut;
    private AsyncOperation _asyncLoad;

    private int _selfInstanceID;

    public static event Action Level1Loaded;

    public void StartGame()
    {
        _fadeInOut.StartFadeOutIn(_selfInstanceID);
    }

    private void Awake()
    {
        _selfInstanceID = GetInstanceID();
    }

    private void Start()
    {
        StartCoroutine(LoadLevel());
    }

    private void OnEnable()
    {
        _fadeInOut.FadeOutFinished += FadeOutFinishedAction;
    }

    private void OnDisable()
    {
        _fadeInOut.FadeOutFinished -= FadeOutFinishedAction;
    }

    private void FadeOutFinishedAction(int fromInstanceID)
    {
        if (fromInstanceID == _selfInstanceID)
        {
            StartCoroutine(WaitAfterFadeOut());
        }
    }

    private IEnumerator WaitAfterFadeOut()
    {
        yield return new WaitForSeconds(0.3f);
        _asyncLoad.allowSceneActivation = true;
        SceneLoadManager.Level1LoadedExternally = true;
    }

    private IEnumerator LoadLevel()
    {
        _asyncLoad = SceneManager.LoadSceneAsync("Level_1");
        _asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!_asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
