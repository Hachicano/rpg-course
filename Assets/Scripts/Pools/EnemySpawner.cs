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
        // 还原各项数值（由 EnemyStats 控制）
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        // 还原各项数值
        enemyStats.resetStats();
        enemy.resetEnemy();
        // 重置动画播放状态
        enemy.stateMachine.changeState(enemy.defaultState);
        enemy.anim.speed = 1.0f;
        popCount++;
        Debug.Log("Enemy initialization has down");
    }
}
