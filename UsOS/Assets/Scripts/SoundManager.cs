#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

using UnityEngine;
using System.Collections;
using System.Linq;

//Add new clips here manually. Check indexes of other clips to be correct Number.Enum when add new assets.
public enum Clip
{
    Click = 0,
    Loading = 1,
    LevelUp = 2,
    Purchase = 3
}

/// <summary>
/// Play music and sound effects.
/// Check indexes of other clips enumeration to be correct Enum => number in Editor inspector when add new items.
/// </summary>
[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    private const string SuccesfullyLoadedMsg = " Success!";
    private const string LibraryIsEmptyMsg = " Library is empty.";

    public bool forceRefresh;
    public AudioSource musicSource;
    public AudioSource firstFxSrc;
    public AudioSource secondFxSrc;

    [Header("Don't forget enumeration in the script.")]
    public SoundData[] clipsLibrary;

    public static SoundManager instance;
    public static bool isLoaded;

    private void Awake()
    {
        if (isLoaded)
        {
            return;
        }

        isLoaded = true;
        instance = this;
    }

    public void MusicPlay(Clip clip)
    {
        this.musicSource.clip = this.clipsLibrary[(byte)clip].Clip;
        BasePlay(this.musicSource);
    }

    public void MusicFadeIn(float totalTime = 1f, float endVolume = 1f)
    {
        StartCoroutine(BaseFadeIn(this.musicSource, totalTime, endVolume));
    }

    public void MusicFadeOut(float totalTime = 0.5f, float startVolume = 1f)
    {
        StartCoroutine(BaseFadeOut(this.musicSource, totalTime, startVolume));
    }

    public void FxPlayOnce(Clip clip)
    {
        BasePlayOneShot(BindClipToSrc(FindClip(clip)));
    }

    public void FxPlayDelayed(AudioClip sound, float delay)
    {
        BindClipToSrc(sound);
        BasePlayWithDelay(this.musicSource, delay);
    }
    
    private void BasePlay(AudioSource sound)
    {
        if (!StaticData.haveSound)
        {
            return;
        }

        sound.Play();
    }

    private void BasePlayOneShot(AudioSource sound)
    {
        if (!StaticData.haveSound)
        {
            return;
        }

        sound.PlayOneShot(sound.clip);
    }

    private void BasePlayWithDelay(AudioSource sound, float delay)
    {
        if (!StaticData.haveSound)
        {
            return;
        }

        sound.PlayDelayed(delay);
    }

    private IEnumerator BaseFadeIn(AudioSource source, float totalTime, float endVolume)
    {
        float currentTime = 0f;

        source.volume = 0f;
        source.Play();

        while (currentTime < totalTime)
        {
            source.volume = Mathf.Lerp(0f, endVolume, currentTime / totalTime);
            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        source.volume = endVolume;
    }

    private IEnumerator BaseFadeOut(AudioSource source, float totalTime, float startVolume)
    {
        if (!source.isPlaying)
        {
            yield break;
        }

        float currentTime = 0f;

        source.volume = startVolume;

        while (currentTime < totalTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0f, currentTime / totalTime);
            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        source.Stop();
    }

    private AudioClip FindClip(Clip clip)
    {
        return this.clipsLibrary[(byte)clip].Clip;
    }

    private AudioSource BindClipToSrc(AudioClip clip)
    {
        if (!this.firstFxSrc.isPlaying)
        {
            this.firstFxSrc.clip = clip;
            return this.firstFxSrc;
        }

        if (!this.secondFxSrc.isPlaying)
        {
            this.secondFxSrc.clip = clip;
            return this.secondFxSrc;
        }

        if (this.firstFxSrc.time > this.secondFxSrc.time)
        {
            this.secondFxSrc.clip = clip;
            return this.secondFxSrc;
        }

        this.firstFxSrc.clip = clip;
        return this.firstFxSrc;
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (this.forceRefresh)
        {
            this.forceRefresh = false;
            FillSoundLibrary();
        }
    }

    private void FillSoundLibrary()
    {
        DirectoryInfo info = new DirectoryInfo("Assets/Sounds");
        FileInfo[] soundFiles = info.GetFiles().Where(file => file.Extension != ".meta").ToArray();
        this.clipsLibrary = new SoundData[soundFiles.Length];

        for (int i = 0; i < this.clipsLibrary.Length; i++)
        {
            this.clipsLibrary[i] = new SoundData
            {
                Name = soundFiles[i].Name.Replace(soundFiles[i].Extension, ""),
                Clip = AssetDatabase.LoadAssetAtPath("Assets/Sounds/" + soundFiles[i].Name, typeof(AudioClip)) as AudioClip
            };
        }

        if (this.clipsLibrary.Length == 0)
        {
            Debug.Log(string.Format("{0}: {1}", GetType().Name, LibraryIsEmptyMsg));
        }
        else
        {
            Debug.Log(string.Format("{0}: {1}", GetType().Name, SuccesfullyLoadedMsg));
        }
    }
#endif
}