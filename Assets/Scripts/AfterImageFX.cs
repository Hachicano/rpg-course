using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;  // If it is null then just make it serializeField and drag it in by yourself
    private float colorLoseRate;

    private void Update()
    {
        float alpha = sr.color.a - colorLoseRate * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (sr.color.a <= 0)
            Destroy(gameObject);
    }

    public void SetupAfterImage(float _losingSpeed, Sprite _spriteImage)
    {
        sr.sprite = _spriteImage;
        colorLoseRate = _losingSpeed;
    }
}
