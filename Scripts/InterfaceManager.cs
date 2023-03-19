using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    // You need to be able to change the animations by clicking the corresponding button (index based)

    [Header("Animation Buttons")]
    public Button[] animationButtons;
    public string[] animationTitles;
    public TMP_Text[] animationTexts;

    [Header("Animation State")]
    public Animator animator;
    public int animationState;
    private float animationBlend;

    [Header("Animation List")]
    public GameObject animationList;
    public Toggle animationListToggle;

    [Header("Settings")]
    public GameObject settingsPanel;

    [Header("External Links")]
    public string[] externalLinks;
    public GameObject externalLinkWindow;
    public TMP_Text externalLinkText;
    private int selectedLinkIndex;

    [Header("Sounds")]
    public static AudioSource[] sounds;
    // [0] - Standard Click [1] - Toggle [2] - Open Window
    public Slider soundVolume;

    [Header("Music")]
    public static AudioSource[] musics;
    public Slider musicVolume;

    [Header("Intro")]
    public GameObject fadePanel;
    public GameObject inputPanel;

    private IEnumerator Start()
    {
        sounds = GetComponents<AudioSource>();

        musics = GameObject.Find("Music").GetComponents<AudioSource>();

        AdjustVolume(true);
        AdjustVolume(false);

        // Add each of the OnClick events to the animation buttons
        for(int i = 0; i < animationButtons.Length; i++)
        {
            int num = i;
            animationButtons[i].onClick.AddListener(delegate
            {
                SwitchAnimation(num);
            });
        }

        yield return new WaitForSeconds(1.1f);

        fadePanel.SetActive(false);

        inputPanel.SetActive(true);
    }

    private void Update()
    {
        animationBlend = Mathf.Lerp(animationBlend, animationState, 0.025f);
        animator.SetFloat("Blend", animationBlend);
    }

    public void ToggleAnimationList()
    {
        sounds[1].Play();
        animationList.SetActive(animationListToggle.isOn);
    }

    public void SwitchAnimation(int index)
    {
        sounds[0].Play();

        foreach(var music in musics)
        {
            music.Stop();
        }

        if (index > 0 && index < 4)
        {
            if (!musics[index - 1].isPlaying)
            {
                musics[index - 1].Play();
            }
        }

        for(int i = 0; i < animationTexts.Length; i++)
        {
            animationTexts[i].text = animationTitles[i];
        }

        animationState = index;
        animationTexts[index].text = animationTitles[index] + " (<color=green>Active</color>)";
    }

    public void ToggleSettings()
    {
        sounds[0].Play();
        settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
    }

    public void CloseLinkWindow()
    {
        sounds[0].Play();
        externalLinkWindow.SetActive(false);
    }

    public void SelectLink(int index)
    {
        sounds[2].Play();
        externalLinkText.text = $"Are you sure you want to visit this url: <color=blue>{externalLinks[index]}</color>?";
        externalLinkWindow.SetActive(true);
        selectedLinkIndex = index;
    }

    public void OpenLink()
    {
        Application.OpenURL(externalLinks[selectedLinkIndex]);
        CloseLinkWindow();
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void AdjustVolume(bool isMusic)
    {
        if (isMusic)
        {
            foreach(var music in musics)
            {
                music.volume = musicVolume.value;
            }
        }
        else
        {
            foreach (var sound in sounds)
            {
                sound.volume = soundVolume.value;
            }
        }
    }
}
