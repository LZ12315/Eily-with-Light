using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : UIButton
{
    [Header("Ãæ°åµ÷³ö")]
    public GameObject menuCanvas;

    public override void ExcutiveAction()
    {
        base.ExcutiveAction();
        menuCanvas.SetActive(true);
    }
}
