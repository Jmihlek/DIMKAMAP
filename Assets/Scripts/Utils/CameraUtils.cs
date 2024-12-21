using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class CameraUtils : MonoBehaviour
{
    private UIDocument _hud;
    private VisualElement _fadeElement => _hud.rootVisualElement.Q<VisualElement>("FadeElement");
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
        _hud = GetComponent<UIDocument>();
    }

    public void ShowScreen(float duration)
    {
        StartCoroutine(FadeToVisible(duration));
    }

    public void HideScreen(float duration)
    {
        StartCoroutine(FadeToBlack(duration));
    }

    public void Shake(float magnitude)
    {
        StartCoroutine(ShakeCamera(magnitude));
    }

    private IEnumerator FadeToVisible(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / duration);
            _fadeElement.style.backgroundColor = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeToBlack(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            _fadeElement.style.backgroundColor = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    private IEnumerator ShakeCamera(float magnitude)
    {
        Vector3 originalPosition = _mainCamera.transform.position;
        float elapsedTime = 0f;
        float shakeDuration = 0.5f; // Фиксированное время тряски

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            _mainCamera.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            yield return null;
        }

        _mainCamera.transform.position = originalPosition;
    }
}
