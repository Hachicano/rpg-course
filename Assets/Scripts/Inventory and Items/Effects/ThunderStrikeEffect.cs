using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item_Effect/Thunder_Strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _executeTransform)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _executeTransform.position, Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }
}
