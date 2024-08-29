using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private float xLimit = 920;
    [SerializeField] private float yLimit = 540;

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        float newXOffset = 0;
        float newYOffset = 0;
        if (mousePosition.x > xLimit)
            newXOffset = -250;
        else
            newXOffset = 300;

        if (mousePosition.y > yLimit)
            newYOffset = -100;
        else
            newYOffset = 200;

        transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
    }
}
