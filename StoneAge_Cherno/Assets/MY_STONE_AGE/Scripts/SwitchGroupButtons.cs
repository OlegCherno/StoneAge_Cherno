using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGroupButtons : MonoBehaviour
{
    [SerializeField] GameObject goupButtons;
    private bool switchOnOff;

    void Start()
    {
        switchOnOff = false;
        goupButtons.SetActive(false);
    }

    public void SwitchGroup()
    {
        if (switchOnOff)
        {
            switchOnOff = false;
            goupButtons.SetActive(false);
        }
        else
        {
            switchOnOff = true;
            goupButtons.SetActive(true);
        }
    }
}
