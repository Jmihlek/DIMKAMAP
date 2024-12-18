using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class UniversalTrigger : MonoBehaviour
{
    public TriggerAction[] TriggerActions;
    private QuestStorage _questStorage;

    private void Start()
    {
        _questStorage = FindAnyObjectByType<QuestStorage>();
        if (_questStorage == null)
            Debug.Log("QuestStorage found in scene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController2D>(out var player))
        {
            foreach (var triggerAction in TriggerActions)
            {
                if (CheckConditions(triggerAction.Conditions))
                {
                    triggerAction.Action?.Invoke();
                    Debug.Log("Trigger action executed");
                }
                else
                {
                    Debug.Log("Conditions not met for action");
                }
            }
        }
    }

    private bool CheckConditions(TriggerCondition[] conditions)
    {
        foreach (var condition in conditions)
        {
            var field = _questStorage.GetType().GetField(condition.VariableName, BindingFlags.Public | BindingFlags.Instance);
            if (field == null || field.FieldType != typeof(bool) || (bool)field.GetValue(_questStorage) != condition.ExpectedValue)
            {
                Debug.LogWarning($"Condition not met: {condition.VariableName}");
                return false;
            }
        }
        return true;
    }
}

[System.Serializable]
public class TriggerCondition
{
    public string VariableName; // Имя переменной
    public bool ExpectedValue; // Ожидаемое значение
}

[System.Serializable]
public class TriggerAction
{
    public TriggerCondition[] Conditions; // Массив условий
    public UnityEvent Action; // Действие при выполнении условий
}
