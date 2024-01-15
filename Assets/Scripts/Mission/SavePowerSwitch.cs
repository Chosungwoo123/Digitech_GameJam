using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SavePowerSwitch : MonoBehaviour
{
    public SavePowerMission mission;
    public SpriteRenderer lightSR;
    public SpriteRenderer switchSR;

    public Sprite switchOnImage;
    public Sprite switchOffImage;

    public Sprite switchOnLightImage;
    public Sprite switchOffLightImage;

    private bool switchOn = true;

    private void Start()
    {
        switchOn = true;

        switchSR.sprite = switchOnImage;
        lightSR.sprite = switchOnLightImage;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Debug.Log("Å¬¸¯");

        switchOn = !switchOn;

        if (switchOn)
        {
            switchSR.sprite = switchOnImage;
            lightSR.sprite = switchOnLightImage;
            mission.Minus();
        }
        else
        {
            switchSR.sprite = switchOffImage;
            lightSR.sprite = switchOffLightImage;
            mission.Success();
        }
    }
}