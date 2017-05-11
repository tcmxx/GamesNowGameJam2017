using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingUI : MonoBehaviour {

    public Graphic[] graphics;

    public int GraphicCount {
        get { return graphics.Length; }
    }

    private Color[] endColor;
    private Color[] startColor;
    private Color[] defaultColor;
    private float timer = 0;
    private float transitionTime = 1.0f;
    private Action onLerpFinished;

	// Use this for initialization
	void Awake() {
        defaultColor = new Color[graphics.Length];
        for (int i = 0; i < graphics.Length; ++i)
        {
            defaultColor[i] = graphics[i].color;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(timer > 0)
        {

            SetColorAllLerp(1 - timer / transitionTime);
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = 0;
                SetColorAllLerp(1);
                if (onLerpFinished != null)
                {
                    onLerpFinished.Invoke();
                }
            }
        }
	}
    
    /// <summary>
    /// fade from transparent to default color
    /// </summary>
    /// <param name="intime"></param>
    /// <param name="onFishied"></param>
    public void FadeIn(float intime, Action onFishied)
    {
        startColor = new Color[graphics.Length];
        for (int i = 0; i < graphics.Length; ++i) {
            Color col = graphics[i].color;
            col.a = 0;
            startColor[i] = col;
        }

        endColor = new Color[graphics.Length];
        for (int i = 0; i < graphics.Length; ++i) { endColor[i] = defaultColor[i]; }

        StartLerp(intime, onFishied);
    }

    /// <summary>
    /// fade from default color to transparent
    /// </summary>
    /// <param name="intime"></param>
    /// <param name="onFishied"></param>
    public void FadeOut(float outtime, Action onFishied)
    {
        endColor = new Color[graphics.Length];
        for (int i = 0; i < graphics.Length; ++i)
        {
            Color col = graphics[i].color;
            col.a = 0;
            endColor[i] = col;
        }

        startColor = new Color[graphics.Length];
        for (int i = 0; i < graphics.Length; ++i) { startColor[i] = graphics[i].color; }

        StartLerp(outtime, onFishied);
    }


    public void FadeInOut(float inTime, float stayTime, float outTime, Action onFinished)
    {
        FadeIn(inTime, () =>
        {
            for (int i = 0; i < graphics.Length; ++i) { startColor[i] = graphics[i].color; }
            StartLerp(stayTime, () =>
            {
                FadeOut(outTime, onFinished);
            });
        });
    }


    public void StartLerp(float time, Action onFinished)
    {
        transitionTime = time;
        timer = time;
        onLerpFinished = onFinished;
        SetColorAllLerp(0);
    }


    /// <summary>
    /// Start to lerp the color of the graphic from fromColor to toColor
    /// </summary>
    /// <param name="fromColor"></param>
    /// <param name="toColor"></param>
    /// <param name="time"></param>
    public void StartLerpFromTo(Color fromColor, Color toColor, float time, Action onFinished)
    {
        transitionTime = time;
        timer = time; 
        startColor = new Color[graphics.Length];
        for(int i = 0;i < graphics.Length; ++i) { startColor[i] = fromColor; }
        endColor = new Color[graphics.Length];
        for (int i = 0; i < graphics.Length; ++i) { endColor[i] = toColor; }
        onLerpFinished = onFinished;
    }

    /// <summary>
    /// Start to lerp the color of the graphic from fromColor to toColor
    /// </summary>
    /// <param name="fromColor"></param>
    /// <param name="toColor"></param>
    /// <param name="time"></param>
    public void StartLerpFromTo(Color[] fromColor, Color[] toColor, float time, Action onFinished)
    {
        transitionTime = time;
        timer = time;
        startColor = new Color[graphics.Length];
        for (int i = 0; i < graphics.Length; ++i) { startColor[i] = fromColor[i]; }
        endColor = new Color[graphics.Length];
        for (int i = 0; i < graphics.Length; ++i) { endColor[i] = toColor[i]; }
        onLerpFinished = onFinished;
    }

    private void SetColorAllLerp(float t)
    {
        for(int i = 0; i < graphics.Length; ++i)
        {
            graphics[i].color = Color.Lerp(startColor[i],endColor[i], t);
        }
    }


    private void SetColorAll(Color color)
    {
        foreach(var g in graphics)
        {
            g.color = color;
        }
    }
}
