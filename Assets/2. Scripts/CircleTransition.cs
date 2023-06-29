using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransition : MonoBehaviour
{
    Canvas canvas;
    public Image blackCircleScreen;
    public Image loadingScreen;
    public Image fadeScreen;

    Coroutine blackCircleCoroutine;
    Coroutine circleCoroutine;

    static readonly int RADIUS = Shader.PropertyToID("_Radius");
    static readonly int COLOR = Shader.PropertyToID("_Color");

    bool isComplete = false;
    public bool Complete { get { return isComplete; } }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        DrawBlackCircle();
    }

    public void CircleEnter()
    {
        if (circleCoroutine != null) return;

        loadingScreen.material.SetColor(COLOR, new Color(1, 1, 1, 0f));
        circleCoroutine = StartCoroutine(TransitionCircle(2f, 0f, 1f));
    }

    public void CircleExit()
    {
        if (circleCoroutine != null) return;

        loadingScreen.material.SetColor(COLOR, new Color(1, 1, 1, 1f));
        circleCoroutine = StartCoroutine(TransitionCircle(2f, 1f, 0f));
    }
    public void BlackCircleEnter()
    {
        if (blackCircleCoroutine != null) return;
        blackCircleScreen.material.SetColor(COLOR, new Color(0, 0, 0, 0f));
        blackCircleCoroutine = StartCoroutine(TransitionBlackCircle(2f, 0f, 1f));
    }
    public void BlackCircleExit()
    {
        if (blackCircleCoroutine != null) return;
        blackCircleScreen.material.SetColor(COLOR, new Color(0, 0, 0, 1f));
        blackCircleCoroutine = StartCoroutine(TransitionBlackCircle(2f, 1f, 0f));
    }

    void DrawBlackCircle()
    {
        Rect canvasRect = canvas.GetComponent<RectTransform>().rect;
        float canvasWidth = canvasRect.width;
        float canvasHeight = canvasRect.height;

        float squareValue = 0f;
        if (canvasWidth > canvasHeight) squareValue = canvasWidth;
        else squareValue = canvasHeight;

        loadingScreen.rectTransform.sizeDelta = new Vector2(squareValue, canvasHeight);
        blackCircleScreen.rectTransform.sizeDelta = new Vector2(squareValue, canvasHeight);
    }

    IEnumerator TransitionCircle(float duration, float beginRadius, float endRadius)
    {
        isComplete = false;
        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.fixedDeltaTime;
            var t = timer / duration;
            var radius = Mathf.Lerp(beginRadius, endRadius, t);

            loadingScreen.material.SetFloat(RADIUS, radius);

            yield return null;
        }
        isComplete = true;
        ResetCoroutine();
    }

    IEnumerator TransitionBlackCircle(float duration, float beginRadius, float endRadius)
    {
        blackCircleScreen.gameObject.SetActive(true);
        isComplete = false;
        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.fixedDeltaTime;
            var t = timer / duration;
            var radius = Mathf.Lerp(beginRadius, endRadius, t);

            blackCircleScreen.material.SetFloat(RADIUS, radius);

            yield return null;
        }
        isComplete = true;
        blackCircleScreen.gameObject.SetActive(false);
        ResetCoroutine();
    }

    void ResetCoroutine()
    {
        if (circleCoroutine != null) circleCoroutine = null;
        if (blackCircleCoroutine != null) blackCircleCoroutine = null;
    }

    public void SetColor(string str, Color color)
    {
        if (str == "Circle")
        {
            loadingScreen.material.SetColor(COLOR, color);
        }
        else blackCircleScreen.material.SetColor(COLOR, color);

    }
}
