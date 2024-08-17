using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Skill_Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    // private KeyCode myHotkey;
    private TextMeshProUGUI myText; // ÐÞ¸ÄTextÓÃ Text.text
    private Transform targetEnemy;
    private Skill_Blackhole_Controller myBlackholeController;
    private int myIndex;

    public bool isFirstAdd = true;

    public void SetupHotkey(Transform _myEnemy, Skill_Blackhole_Controller _myBlackholeController, int _index)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        targetEnemy = _myEnemy;
        myBlackholeController = _myBlackholeController;
        myIndex = _index;
    }
    /*
    public void SetupHotkey(KeyCode _myNewHotkey, Transform _myEnemy, Skill_Blackhole_Controller _myBlackholeController)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        targetEnemy = _myEnemy;
        myBlackholeController = _myBlackholeController;

        myHotkey = _myNewHotkey;
        myText.text = myHotkey.ToString();
    }*/

    private void Update()
    {
        if (isFirstAdd && myBlackholeController.hotkeyNumber == myIndex && Input.GetKeyDown(KeyCode.Mouse0))
        {
            CheckEnemy();
            // Debug.Log("Hotkey " + myText.text + " was added");
        }
        /*
        if (Input.GetKeyDown(myHotkey) && isFirstAdd)
        {
            myBlackholeController.AddEnemyToList(targetEnemy);
            myText.color = Color.clear;
            sr.color = Color.clear;
            isFirstAdd = false;
            // Debug.Log("Hotkey " + myText.text + " was added");
        }*/

    }

    public void CheckEnemy()
    {
        myBlackholeController.AddEnemyToList(targetEnemy);
        myText.color = Color.clear;
        sr.color = Color.clear;
        isFirstAdd = false;

        float xOffset;
        if (Random.Range(0, 100) > 50)
            xOffset = 2;
        else
            xOffset = -2;

        myBlackholeController.TargetAttack(myBlackholeController.TargetCount() - 1, new Vector3(xOffset, 0));
        myBlackholeController.MinusHotkeyNumber();
    }
}
