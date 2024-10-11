using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour, IInteractable
{
    public void Click()
    {
        ReceiveInteraction();
    }

    public virtual void ReceiveInteraction()
    {
        if(CheckIfInteractable())
            ExcutiveAction();
    }

    public virtual bool CheckIfInteractable()
    {
        return true;
    }

    public virtual void ExcutiveAction()
    {

    }

}
