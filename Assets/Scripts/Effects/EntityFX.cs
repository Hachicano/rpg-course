using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Player player;
    private GameObject myHealthBar;

    [Header("After Image FX")]
    [SerializeField] private GameObject[] afterImagePrefabs;
    [SerializeField] private float colorLoseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;

    [Header("Pop Up Text FX")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("Screen Shake FX")]
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeCatchSword;
    public Vector3 shakeHighDamage;
    private CinemachineImpulseSource screenShake;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration = .2f;
    protected Material originalMat;

    [Header("Ailment Colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFX_00;
    [SerializeField] private GameObject hitFX_01;
    [Space]
    [SerializeField] private ParticleSystem dustFX;

    protected virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        screenShake = GetComponent<CinemachineImpulseSource>();
        myHealthBar = GetComponentInChildren<UI_HealthBar>().gameObject;
    }

    protected virtual void Start()
    {
        originalMat = sr.material;
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0 && afterImagePrefabs.Length > 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            int i = Random.Range(0, afterImagePrefabs.Length);
            GameObject newAfterImage = Instantiate(afterImagePrefabs[i], transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLoseRate, afterImagePrefabs[i].GetComponent<SpriteRenderer>().sprite);
        }
    }

    public void CreatePopUpText(string _text)
    {
        float xOffset = Random.Range(-1, 1);
        float yOffset = Random.Range(1, 2);
        Vector3 positionOffset = new Vector3(xOffset, yOffset, 0);

        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public virtual void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            sr.color = Color.clear;
            myHealthBar.SetActive(false);
        }
        else
        {
            sr.color = Color.white;
            myHealthBar.SetActive(true);
        }
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

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void IgniteFXFor(float _seconds)
    {
        igniteFX.Play();

        InvokeRepeating("IgniteColorFX", 0, .5f); // .5f is same as igniteDamgeCooldown
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        chillFX.Play();

        InvokeRepeating("ChillColorFX", 0, .5f); // .5f means nothing, two color is same;
        //sr.color = chillColor[0];
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFXFor(float _seconds)
    {
        shockFX.Play();

        InvokeRepeating("ShockColorFX", 0, .3f); // Frequency of the blink is no realistic meaning, just for visual effect. 
        Invoke("CancelColorChange", _seconds);
    }

    
    private void ChillColorFX()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }
    
    private void IgniteColorFX()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ShockColorFX()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    public void CreateHitFX(Transform _target, bool _isCritical)
    {
        float zRotation = Random.Range(-90, 90);
        float xOffset = Random.Range(-.5f, .5f);
        float yOffset = Random.Range(-.5f, .5f);

        Vector3 hitFXRotation = new Vector3(0, 0, zRotation);

        GameObject hitFXPrefab = hitFX_00;

        if (_isCritical)
        {
            hitFXPrefab = hitFX_01;

            float yRotation = 0;
            zRotation = Random.Range(-20, 45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;

            hitFXRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFX = Instantiate(hitFXPrefab, _target.position + new Vector3(xOffset, yOffset), Quaternion.identity);
        //GameObject newHitFX = Instantiate(hitFXPrefab, _target.position + new Vector3(xOffset, yOffset), Quaternion.identity, _target); // critical hit fx will follow the target
        newHitFX.transform.Rotate(hitFXRotation);

        Destroy(newHitFX, .5f);
    }
    public void PlayDustFX()
    {
        if (dustFX != null)
        {
            dustFX.Play();
        }
    }
}
