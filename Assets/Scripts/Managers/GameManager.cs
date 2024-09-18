using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour , ISaveManager
{
    public static GameManager instance;

    private Transform playerTrans;

    [SerializeField] private CheckPoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;

    [Header("Lost Currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        checkpoints = FindObjectsOfType<CheckPoint>();
    }

    private void Start()
    {
        playerTrans = PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelay(_data));
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = playerTrans.position.x;
        _data.lostCurrencyY = playerTrans.position.y;

        if (FindClosestCheckpoint() != null)
            _data.closestCheckpointId = FindClosestCheckpoint().id;
        _data.checkpoints.Clear();

        foreach (CheckPoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.actiationStatus);
        }
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        LoadClosestCheckpoint(_data);
        LoadCheckpoint(_data);
        LoadLostCurrency( _data);
    }

    private void LoadCheckpoint(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (CheckPoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                {
                    checkpoint.ActivateCheckPoint();
                }
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private void LoadClosestCheckpoint(GameData _data)
    {
        if (_data.closestCheckpointId == null)
            return;

        closestCheckpointId = _data.closestCheckpointId;

        foreach (CheckPoint checkpoint in checkpoints)
        {
            if (closestCheckpointId == checkpoint.id)
            {
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
            }
        }
    }

    private CheckPoint FindClosestCheckpoint()
    {
        float closestDis = Mathf.Infinity;
        CheckPoint cloesetCheckpoint = null;

        foreach(CheckPoint checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(playerTrans.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDis && checkpoint.actiationStatus == true)
            {
                cloesetCheckpoint = checkpoint;
                closestDis = distanceToCheckpoint;
            }
        }

        return cloesetCheckpoint;
    }
}
