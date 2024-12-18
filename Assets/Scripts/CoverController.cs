using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CoverController : MonoBehaviour
{
    public float FadeOutDuration = 1f;
    public float FadeInDuration = 1f;
    private SpriteRenderer _spriteRenderer;

    public UnityEvent OnFadeOutStart;
    public UnityEvent OnFadeOutComplete;
    public UnityEvent OnFadeInComplete;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartFade()
    {
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // ������� ����� ������� ����������
        OnFadeOutStart?.Invoke();

        // ������� ����������
        yield return StartCoroutine(Fade(0, 1f, FadeOutDuration));

        // ������� ����� ���������� ����������
        OnFadeOutComplete?.Invoke();

        // ������� ���������
        yield return StartCoroutine(Fade(1, 0f, FadeInDuration));

        // ������� ����� ���������� ���������
        OnFadeInComplete?.Invoke();
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = _spriteRenderer.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // ������� ��������� ������������
            _spriteRenderer.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha, endAlpha, t));

            yield return null;
        }

        _spriteRenderer.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
