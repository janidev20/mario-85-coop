using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;


public class SettingsMenu : MonoBehaviour
{
    // SETTINGS MENU
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

    }

    //SETTINGS MENU
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    [Tooltip("Game - 0, Graphics - 1, Audio - 2")]
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

    //GRAPHICS
    public void AOSet(bool is_ao)
    {
        AO.active = is_ao;
    }
    public void MotionBlur(bool is_motionBlur)
    {
        motionBlur.active = is_motionBlur;
    }
    public void Bloom(bool is_bloom)
    {
        bloom.active = is_bloom;
    }
    public void Grain(bool is_grain)
    {
        grain.active = is_grain;
    }
    public void RetroLook(bool is_retrolook)
    {
        retroLook.active = is_retrolook;
    }

    //GAME
    public void FullScreen(bool is_fullscene)
    {
        Screen.fullScreen = is_fullscene;
    }
    public void VSync(bool is_Vsync) //0 (OFF) or 1 (ON)
    {
        isVsyncOn = is_Vsync;

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
