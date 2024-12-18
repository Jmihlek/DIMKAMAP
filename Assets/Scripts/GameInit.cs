using UnityEngine;

public class GameInit : MonoBehaviour
{
    public bool NeedFadeoutOnLoad = true;
    public CoverController BeginCover;

    void Start()
    {
        if (NeedFadeoutOnLoad)
            BeginCover.FadeIn();
    }

}
