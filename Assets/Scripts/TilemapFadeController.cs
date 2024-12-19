using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class TilemapFadeController : MonoBehaviour
{
    public float FadeOutDuration = 1f;
    public float FadeInDuration = 1f;
    public AnimationCurve FadeOutCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve FadeInCurve = AnimationCurve.Linear(0, 1, 1, 0);
    private Tilemap _tilemap;

    public UnityEvent OnFadeOutStart;
    public UnityEvent OnFadeOutComplete;
    public UnityEvent OnFadeInStart;
    public UnityEvent OnFadeInComplete;

    private void Start()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    public void StartFadeSequence()
    {
        StartCoroutine(FadeSequence());
    }

    public void FadeOut(float delay = 0f)
    {
        StartCoroutine(FadeOutCoroutine(delay));
    }

    public void FadeIn(float delay = 0f)
    {
        StartCoroutine(FadeInCoroutine(delay));
    }

    private IEnumerator FadeSequence()
    {
        yield return FadeOutCoroutine();
        yield return FadeInCoroutine();
    }

    private IEnumerator FadeOutCoroutine(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        OnFadeOutStart?.Invoke();
        yield return StartCoroutine(Fade(0, 1f, FadeOutDuration, FadeOutCurve));
        OnFadeOutComplete?.Invoke();
    }

    private IEnumerator FadeInCoroutine(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        OnFadeInStart?.Invoke();
        yield return StartCoroutine(Fade(1, 0f, FadeInDuration, FadeInCurve));
        OnFadeInComplete?.Invoke();
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration, AnimationCurve curve)
    {
        float elapsed = 0f;
        Color color = _tilemap.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float curveValue = curve.Evaluate(t);
            _tilemap.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha, endAlpha, curveValue));
            yield return null;
        }

        _tilemap.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
