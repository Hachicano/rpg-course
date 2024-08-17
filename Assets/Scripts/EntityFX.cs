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

    [Header("Ailment Colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        originalMat = sr.material;
    }
    public virtual void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    public IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
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

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void ChillFxFor(float _seconds)
    {
        InvokeRepeating("ChillColorFx", 0, .5f); // .5f means nothing, two color is same;
        //sr.color = chillColor[0];
        Invoke("CancelColorChange", _seconds);
    }

    public void IgniteFXFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, .5f); // .5f is same as igniteDamgeCooldown
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFXFor(float _seconds)
    {
        InvokeRepeating("ShockColorFx", 0, .3f); // Frequency of the blink is no realistic meaning, just for visual effect. 
        Invoke("CancelColorChange", _seconds);
    }

    
    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }
    
    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

}
