using UnityEngine;

public class QuestStorage : MonoBehaviour
{
    public bool IsStartKeyFind;
    public void SetIsStartKeyFind(bool value) => IsStartKeyFind = value;

    public bool IsKeyReceived;
    public void SetIsKeyReceived(bool value) => IsKeyReceived = value;
}
