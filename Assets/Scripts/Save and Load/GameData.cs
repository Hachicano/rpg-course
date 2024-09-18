using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public int lostCurrencyAmount;
    public float lostCurrencyX;
    public float lostCurrencyY;

    public SerializableDictionary<string, float> volumeSettings;

    public GameData()
    {
        this.currency = 0;
        this.skillTree = new SerializableDictionary<string, bool>();
        this.inventory = new SerializableDictionary<string, int>();
        this.equipmentId = new List<string>();

        this.checkpoints = new SerializableDictionary<string, bool>();
        this.closestCheckpointId = string.Empty;

        this.lostCurrencyAmount = 0;
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;

        this.volumeSettings = new SerializableDictionary<string, float>();
    }
}
