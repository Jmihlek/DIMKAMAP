using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ModalWindow : MonoBehaviour
{
    public UnityEvent EventAfterClose;
    public DelayedUnityEvent DelayedEventAfterClose;
    public static bool IsShowModalNow = false;
    private PlayerInput _input;

    private void OnEnable()
    {
        IsShowModalNow = true;
        _input = FindAnyObjectByType<PlayerInput>();
        _input.SwitchCurrentActionMap("Modal");
        _input.actions["ModelClose"].performed += ModalWindow_performed;
    }

    private void OnDisable()
    {
        _input.actions["ModelClose"].performed -= ModalWindow_performed;
    }

    private void ModalWindow_performed(InputAction.CallbackContext obj)
    {
        IsShowModalNow = false;
        _input.SwitchCurrentActionMap("Player");
        EventAfterClose?.Invoke();
        DelayedEventAfterClose?.Invoke();
        gameObject.SetActive(false);
    }
}
