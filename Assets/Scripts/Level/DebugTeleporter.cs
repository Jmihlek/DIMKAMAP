using System.Linq;
using UnityEngine;

public class DebugTeleporter : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            FindObjectsByType<Level>(FindObjectsSortMode.None).Where(l => l.Name == "1").FirstOrDefault().LoadLevel("lv1-2");
        if (Input.GetKeyDown(KeyCode.Alpha2))
            FindObjectsByType<Level>(FindObjectsSortMode.None).Where(l => l.Name == "2").FirstOrDefault().LoadLevel("lv2-1");
        if (Input.GetKeyDown(KeyCode.Alpha3))
            FindObjectsByType<Level>(FindObjectsSortMode.None).Where(l => l.Name == "3").FirstOrDefault().LoadLevel("lv3-2");
        if (Input.GetKeyDown(KeyCode.Alpha4))
            FindObjectsByType<Level>(FindObjectsSortMode.None).Where(l => l.Name == "4").FirstOrDefault().LoadLevel("lv4-3");
        if (Input.GetKeyDown(KeyCode.Alpha5))
            FindObjectsByType<Level>(FindObjectsSortMode.None).Where(l => l.Name == "5").FirstOrDefault().LoadLevel("lv5-4");
    }
}
