using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SettingsMenu : MonoBehaviour
{
    [Header("Settings Menu")]
    [Space(1)]
    [SerializeField] private RectTransform SettingsHolder;

    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private TextMeshProUGUI GameButton, GraphicsButton, AudioButton;


    [Header("Settings Menu/GRAPHICS")]
    [Space(1)]
    [SerializeField] private AmbientOcclusion AO;
    [SerializeField] private MotionBlur motionBlur;
    [SerializeField] private Bloom bloom;
    [SerializeField] private Grain grain;
    [SerializeField] private LensDistortion retroLook;

    [SerializeField] private bool isAO, isMB, isB, isG, isRL, isFS;
    [SerializeField] private Toggle AOToggle, MBToggle, BToggle, GToggle, RLToggle, FSToggle, VSyncToggle;

    [Header("Settings Menu/GAME")]
    [Space(1)]

    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
    private bool isVsyncOn;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("ao"))
        {
            isAO = true;
            isMB = true;
            isB = true;
            isG = true;
            isRL = true;
            isFS = true;
        }
     

        // SETTINGS MENU/GAME
        postProcessVolume.profile.TryGetSettings<AmbientOcclusion>(out AO);
        postProcessVolume.profile.TryGetSettings<MotionBlur>(out motionBlur);
        postProcessVolume.profile.TryGetSettings<Bloom>(out bloom);
        postProcessVolume.profile.TryGetSettings<Grain>(out grain);
        postProcessVolume.profile.TryGetSettings<LensDistortion>(out retroLook);

        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            Debug.Log("Resolution: " + currentRefreshRate);
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + " Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        isAO = PlayerPrefs.GetInt("ao") == 1 ? true : false;
        AO.active = isAO;
        AOToggle.isOn = isAO;

        isMB = PlayerPrefs.GetInt("motionBlur") == 1 ? true : false;
        motionBlur.active = isMB;
        MBToggle.isOn = isMB;

        isB = PlayerPrefs.GetInt("bloom") == 1 ? true : false;
        bloom.active = isB;
        BToggle.isOn = isB;

        isG = PlayerPrefs.GetInt("grain") == 1 ? true : false;
        grain.active = isG;
        GToggle.isOn = isG;

        isRL = PlayerPrefs.GetInt("retroLook") == 1 ? true : false;
        retroLook.active = isRL;
        RLToggle.isOn = isRL;

        isFS = PlayerPrefs.GetInt("fullscene") == 1 ? true : false;
        FSToggle.isOn = isFS;

        isVsyncOn = PlayerPrefs.GetInt("vsync") == 1 ? true : false;
        VSyncToggle.isOn = isVsyncOn;
    }

    //SETTINGS MENU
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    [Tooltip("Game - 0, Graphics - 1, Audio - 2")]
    // If we click on a different Settings Menu point, change that button we clicked on, to underlined style.
    public void ChangeStroke(int strokeObject)
    {
        // Object values :
        // Game - 0
        // Graphics - 1
        // Audio - 2

        if (strokeObject == 0)
        {
            GameButton.fontStyle = TMPro.FontStyles.Underline;
            GraphicsButton.fontStyle &= ~FontStyles.Underline;
            AudioButton.fontStyle &= ~FontStyles.Underline;
        }
        else if (strokeObject == 1)
        {
            GameButton.fontStyle &= ~FontStyles.Underline;
            GraphicsButton.fontStyle = TMPro.FontStyles.Underline;
            AudioButton.fontStyle &= ~FontStyles.Underline;
        }
        else if (strokeObject == 2)
        {
            GameButton.fontStyle &= ~FontStyles.Underline;
            GraphicsButton.fontStyle &= ~FontStyles.Underline;
            AudioButton.fontStyle = TMPro.FontStyles.Underline;
        }
    } 

    //GRAPHICS//
    public void AOSet(bool is_ao)
    {
        isAO = is_ao;
        PlayerPrefs.SetInt("ao", isAO ? 1 : 0);
        AO.active = isAO;
    }
    public void MotionBlur(bool is_motionBlur)
    {
        isMB = is_motionBlur;
        PlayerPrefs.SetInt("motionBlur", isMB ? 1 : 0);
        motionBlur.active = isMB;
    }
    public void Bloom(bool is_bloom)
    {

        isB = is_bloom;
        PlayerPrefs.SetInt("bloom", isB ? 1 : 0);
        bloom.active = isB;

    }
    public void Grain(bool is_grain)
    {
        isG = is_grain;
        PlayerPrefs.SetInt("grain", isG ? 1 : 0);
        grain.active = isG;

    }
    public void RetroLook(bool is_retrolook)
    {
        isRL = is_retrolook;
        PlayerPrefs.SetInt("retroLook", isRL ? 1 : 0);
        retroLook.active = isRL;
    }

    //GAME//
    public void FullScreen(bool is_fullscene)
    {
        isFS = is_fullscene;
        PlayerPrefs.SetInt("fullscene", isFS ? 1 : 0);
        Screen.fullScreen = isFS;

    }
    public void VSync(bool is_Vsync) //0 (OFF) or 1 (ON)
    {
        isVsyncOn = is_Vsync;
        PlayerPrefs.SetInt("vsync", isVsyncOn ? 1 : 0);

            if (isVsyncOn)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }
    }

}
