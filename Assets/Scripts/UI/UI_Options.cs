using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Options : MonoBehaviour
{
    public void SaveAndExit()
    {
        SaveManager.instance.SaveGame();
        //Debug.Log("Exit game");
        Application.Quit();
    }
}
