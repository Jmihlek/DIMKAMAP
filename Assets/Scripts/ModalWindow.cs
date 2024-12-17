using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ModalWindow : MonoBehaviour
{
    public UnityEvent EventAfterClose;
    private PlayerInput _input;

    private void OnEnable()
    {
        _input = FindAnyObjectByType<PlayerInput>();
        _input.SwitchCurrentActionMap("Modal");
        _input.actions["ModelClose"].performed += ModalWindow_performed;
    }

    private void ModalWindow_performed(InputAction.CallbackContext obj)
    {
        _input.SwitchCurrentActionMap("Player");
        EventAfterClose?.Invoke();
        gameObject.SetActive(false);
    }
}
