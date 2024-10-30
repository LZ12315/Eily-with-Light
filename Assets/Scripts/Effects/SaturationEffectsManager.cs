using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaturationEffectsManager : MonoBehaviour
{
    public SaturationSender sender;
    public SaturationStatues SaturationAction;
    public ShockWaveControl waveControl;

    public void StartEffect()
    {
        if(SaturationAction == SaturationStatues.TurnOff)
        {
            sender.ControlOff();
        }
        if (SaturationAction == SaturationStatues.TurnOn)
        {
            sender.ControlOn();
        }
    }
}
