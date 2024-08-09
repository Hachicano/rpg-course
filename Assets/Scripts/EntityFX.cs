using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration = .2f;
    private Material originalMat;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        originalMat = sr.material;
    }

    public IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMat;
    }

    private void RedColorBlink() { 
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else { 
            sr.color = Color.red; 
        }
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

}
