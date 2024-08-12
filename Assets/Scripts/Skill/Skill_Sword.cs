using System;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Skill_Sword : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce Info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinGravity = 1;
    [SerializeField] private float hitCooldown = .35f;

    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }

    protected override void Update()
    {
        base.Update();
        SetupGravity();
        if (Input.GetKeyUp(KeyCode.Mouse1) && !player.sword)
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPostion(i * spaceBetweenDots);
            }
        }
    }

    private void SetupGravity()
    {
        switch (swordType)
        {
            case SwordType.Regular:
                swordGravity = 4.5f;
                break;
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Skill_Sword_Controller newSwordScript = newSword.GetComponent<Skill_Sword_Controller>();

        switch (swordType)
        {
            case SwordType.Bounce:
                newSwordScript.SerupBounce(true, bounceAmount, bounceSpeed);
                break;
            case SwordType.Pierce:
                newSwordScript.SetupPierce(pierceAmount);
                break;
            case SwordType.Spin:
                newSwordScript.SetupSpine(true, maxTravelDistance, spinDuration, hitCooldown);
                break;
        }

        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }


    #region Aim
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPostion(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
    #endregion
}
