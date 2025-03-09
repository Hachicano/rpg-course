using System;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;
    [SerializeField] private bool bounceUnlocked;
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
    [SerializeField] private bool pierceUnlocked;
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;
    [SerializeField] private bool spinUnlocked;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinGravity = 1;
    [SerializeField] private float hitCooldown = .35f;

    [Header("Skill Info")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;
    public bool swordUnlocked { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Passive Skill")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    [SerializeField] private UI_SkillTreeSlot vulnerabilityUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    public bool vulnerabilityUnlocked { get; private set; }

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

        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerabilityUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerability);
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

    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockTimeStop();
        UnlockVulnerability();
        UnlockBounceSword();
        UnlockPierceSword();
        UnlockSpinSword();
    }

    #region Unlock region

    public void UnlockSword()
    {
        swordUnlocked = swordUnlockButton.unlocked;
    }

    public void UnlockBounceSword()
    {
        bounceUnlocked = bounceUnlockButton.unlocked;
        if (bounceUnlocked)
            swordType  = SwordType.Bounce;
    }

    public void UnlockPierceSword()
    {
        pierceUnlocked = pierceUnlockButton.unlocked;
        if (pierceUnlocked)
            swordType = SwordType.Pierce;
    }

    public void UnlockSpinSword()
    {
        spinUnlocked = spinUnlockButton.unlocked;
        if (spinUnlocked)
            swordType = SwordType.Spin;
    }

    public void UnlockTimeStop()
    {
        timeStopUnlocked = timeStopUnlockButton.unlocked;
    }

    public void UnlockVulnerability()
    {
        vulnerabilityUnlocked = vulnerabilityUnlockButton.unlocked;
    }


    #endregion

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
                newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
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
