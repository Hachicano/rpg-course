using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_Blackhole_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    // [SerializeField] private List<KeyCode> keyCodeList;
    public int hotkeyNumber = 0;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer;

    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotkey = true;
    private bool cloneAttackRelease;
    private bool detectEnemy;
    private bool playerCanDispear = true;

    private int AttackTimes;
    private int FinalAttackAcount;
    private float cloneAttackCooldown;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>(); // ��ס����ĳ�private�Ļ�����Ҫ�Լ��������б�
    [SerializeField] private List<GameObject> createdHotkey = new List<GameObject>();

    public bool playerCanExitTheState {  get; private set; }

    public void SetupBlackhole(float _blackholeDuration, float _maxSize, float _growSpeed, float _shrinkSpeed, int _AttackTimes, float _cloneAttackCooldown)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        AttackTimes = _AttackTimes;
        cloneAttackCooldown = _cloneAttackCooldown;

        blackholeTimer = _blackholeDuration;
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && hotkeyNumber == 0 && detectEnemy)
        {
            ReleaseCloneAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !detectEnemy)
        {
            EndBlackhole();
        }

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;
            if (detectEnemy)
            { 
                for (int i = 0; i < createdHotkey.Count; i++)
                {
                    if (createdHotkey[i] != null && createdHotkey[i].GetComponent<Skill_Blackhole_HotKey_Controller>().isFirstAdd)
                        createdHotkey[i].GetComponent<Skill_Blackhole_HotKey_Controller>().CheckEnemy();
                }
                ReleaseCloneAttack(); 
            }
            else
                EndBlackhole();
        }

        CloneAttackLogic();
        ChangeBlackholeScale();
    }

    private void ChangeBlackholeScale()
    {
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        DestroyHotkeys();
        cloneAttackRelease = true;
        canCreateHotkey = false;
        FinalAttackAcount = targets.Count * AttackTimes;
        if (FinalAttackAcount < 10)
            FinalAttackAcount = 10;
        else if (FinalAttackAcount > 100)
            FinalAttackAcount = 100;
        if (playerCanDispear)
        {
            PlayerManager.instance.player.MakeTransparent(playerCanDispear);
            playerCanDispear = false;
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackRelease && targets.Count > 0 && FinalAttackAcount > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;
            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            FinalAttackAcount--;
            if (FinalAttackAcount <= 0)
            {
                Invoke(nameof(EndBlackhole), .5f); // ����һ���ӳ��Ӿ�Ч������
            }
        }
    }

    private void EndBlackhole()
    {
        playerCanExitTheState = true;
        canShrink = true;
        cloneAttackRelease = false;
    }

    private void DestroyHotkeys()
    {
        if (createdHotkey.Count <= 0)
            return;
        for (int i = 0; i < createdHotkey.Count; i++) 
        {
            Destroy(createdHotkey[i]); // �ݻ��б��Ӧλ�ô洢�����壬����û��������б����Ƴ�
        }
        // createdHotkey.RemoveRange(0, createdHotkey.Count); // ����б� ��֪��ΪɶĪ���ᱨ��
        // ����龳�¿��Բ������б���Ϊ�����б��gameobjectҲ�ᱻ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            detectEnemy = true;
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotkey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent <Enemy>().FreezeTime(false);
        }
    }

    private void CreateHotkey(Collider2D collision)
    {
        /*
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("Not enough hotkeys in KeyCodeList !!!");
            return;
        }*/
        if (!canCreateHotkey)
            return;

        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotkey.Add(newHotkey);

        // KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        // keyCodeList.Remove(choosenKey);

        Skill_Blackhole_HotKey_Controller newHotkeyScript = newHotkey.GetComponent<Skill_Blackhole_HotKey_Controller>();
        hotkeyNumber++;
        newHotkeyScript.SetupHotkey(collision.transform, this, hotkeyNumber);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
    public void MinusHotkeyNumber() => hotkeyNumber--;
    public void TargetAttack(int _index, Vector2 _offset) => SkillManager.instance.clone.CreateClone(targets[_index], _offset);
    public int TargetCount() => targets.Count;
}
