using UnityEngine;

public class GameInit : MonoBehaviour
{
    public bool NeedFadeoutOnLoad = true;
    public CoverController BeginCover;

    void Start()
    {
        if (NeedFadeoutOnLoad)
        {
            if (BeginCover != null)
                BeginCover.FadeIn();
            else
            {
                var cameraUtils = FindAnyObjectByType<CameraUtils>();
                cameraUtils.ShowScreen(4);
            }
        }
    }

}
