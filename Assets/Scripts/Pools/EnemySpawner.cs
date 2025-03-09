using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : ObjectSpawner
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public override void initialize()
    {
        // ��ԭ������ֵ���� EnemyStats ���ƣ�
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        // ��ԭ������ֵ
        enemyStats.resetStats();
        enemy.resetEnemy();
        // ���ö�������״̬
        enemy.stateMachine.changeState(enemy.defaultState);
        enemy.anim.speed = 1.0f;
        popCount++;
        Debug.Log("Enemy initialization has down");
    }
}
