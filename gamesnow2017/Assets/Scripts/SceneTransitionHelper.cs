using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneTransitionHelper : MonoBehaviour {



    public static SceneTransitionHelper Instance;

    [Header("References")]
    public FadingUI maskUI;
    [Header("Settings")]
    public float maskFadeInTime;
    public float maskFadeOutTime;


    [Header("Settings if using a transition scene")]
    public bool useTransitionScene = false;
    public string transitionSceneName;
    public float sceneFadeInTime;
    public float sceneFadeOutTime;
    public float minTransitionSceneStayTime;

    public bool IsInTransition { get; private set; }

    private float minTransitionStayLeft = 0;

    private bool allowActiveNewScene = false;


    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("SceneTransitionHelper doesn't exist yet. Created new one.");
            DontDestroyOnLoad(gameObject);
            Instance = this;
            
        }
        else if (Instance != this)
        {
            Debug.Log("There is more than one SceneTransitionHelper. Destroyed the new one.");
            Destroy(gameObject);
        }


    }
    // Use this for initialization
    void Start () {
        maskUI.gameObject.SetActive(false);
        IsInTransition = false;
    }
	
	// Update is called once per frame
	void Update () { 

        if(minTransitionStayLeft >= 0)
        {
            minTransitionStayLeft -= Time.deltaTime;
            if(minTransitionStayLeft < 0)
            {
                minTransitionStayLeft = 0;
            }
        }
    }


    public void StartLoadingScene(string sceneName)
    {
        IsInTransition = true;
        minTransitionStayLeft = minTransitionSceneStayTime;

        maskUI.gameObject.SetActive(true);
        
        if (useTransitionScene)
        {
            //use transition scene
            allowActiveNewScene = false;
            maskUI.FadeIn(maskFadeInTime, () => {
                allowActiveNewScene = true;
                StartCoroutine(StartLevelLoading(transitionSceneName, null,
                    () => {
                        maskUI.FadeOut(sceneFadeInTime, () => maskUI.gameObject.SetActive(false));
                        allowActiveNewScene = false;
                        StartLevelLoading(sceneName,
                            () =>
                            {
                                maskUI.gameObject.SetActive(true);
                                maskUI.FadeIn(sceneFadeOutTime, () =>
                                {
                                    allowActiveNewScene = true;
                                });
                            },
                            () =>
                            {
                                maskUI.FadeOut(maskFadeOutTime, () => maskUI.gameObject.SetActive(false));
                                IsInTransition = false;
                            },
                            true
                            );
                    }
                    ));
            });

        }else
        {
            allowActiveNewScene = false;
            maskUI.FadeIn(maskFadeInTime, () => {
                allowActiveNewScene = true;
                StartCoroutine(StartLevelLoading(sceneName, null, () =>
                {
                    maskUI.FadeOut(maskFadeOutTime, null);
                    IsInTransition = false;
                }));
            });
        }
    }



    IEnumerator StartLevelLoading(string sceneName, Action onLevelReady = null, Action afterLevelLoaded = null, bool checkMinStayTime = false)
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.allowSceneActivation = false;
        bool readyHasCalled = false;
        while (!ao.isDone)
        {
            // Loading completed
            if(ao.progress >= 0.9f && !readyHasCalled && onLevelReady != null)
            {
                onLevelReady.Invoke();
            }

            if (ao.progress >= 0.9f && allowActiveNewScene && !(checkMinStayTime && minTransitionStayLeft >0))
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }

        if (afterLevelLoaded != null)
        {
            afterLevelLoaded.Invoke();
        }
    }



}
