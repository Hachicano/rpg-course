using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    // Can choose use dictionary to store sfx & bgm
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    private int bgmIndex;
    private bool canPlaySFX;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        Invoke("AllowSFX", 1f);
    }

    private void Update()
    {
        if (!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
                // Or play random bgm
                // PlayRandomBGM();
            }
        }
    }

    public void PlayerSFX(int _sfxIndex, Transform _source)
    {
        if (!canPlaySFX)
            return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)
            return;

        if (_sfxIndex < sfx.Length) //&& !sfx[_sfxIndex].isPlaying
        {
            sfx[_sfxIndex].pitch = Random.Range(0.95f, 1.05f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();

    public void StopSFXWithTiem(int _sfxIndex)
    {
        StartCoroutine(DecreaseVolume(sfx[_sfxIndex]));
    }

    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;  //线性渐入渐出，也可以使用当前的值来做乘法做到一个越接近目标速度越慢的渐入渐出
        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.25f);

            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }

    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;
        StopAllBGM();

        bgm[bgmIndex].Play();
    }

    public void PlayRandomBGM()
    {
        StopAllBGM();

        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySFX = true;
}
