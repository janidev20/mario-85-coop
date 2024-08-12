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
    [SerializeField] private LensDistortion lensDistortion;
    [SerializeField] private Vignette vignette;
    [SerializeField] private MotionBlur motionBlur;
    [SerializeField] private ColorGrading colorGrading;
    [SerializeField] private DepthOfField dOf;
    [SerializeField] private ChromaticAberration CA;
    [SerializeField] private GameObject grid;

    [SerializeField] private bool isRetroLook, isFS;
    [SerializeField] private Toggle RLToggle, FSToggle, VSyncToggle;

    [Header("Settings Menu/GAME")]
    [Space(1)]

    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
    private bool isVsyncOn;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    private void Update()
    {
        if (grid == null)
        {

            grid = GameObject.FindGameObjectWithTag("grid").gameObject;
        }

        if (postProcessVolume == null)
        {
            postProcessVolume = GameObject.FindGameObjectWithTag("PP").GetComponent<PostProcessVolume>();

            PlayerPrefs.SetInt("retroLook", isRetroLook ? 1 : 0);
            lensDistortion.active = isRetroLook;
            vignette.active = isRetroLook;
            motionBlur.active = isRetroLook;
            colorGrading.active = isRetroLook;
            dOf.active = isRetroLook;
            CA.active = isRetroLook;
            grid.SetActive(isRetroLook);
            RLToggle.isOn = isRetroLook;
        }
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("ao"))
        {
            isRetroLook = true;
            isFS = true;
        }



        // SETTINGS MENU/GAME
        postProcessVolume.profile.TryGetSettings<LensDistortion>(out lensDistortion);
        postProcessVolume.profile.TryGetSettings<Vignette>(out vignette);
        postProcessVolume.profile.TryGetSettings<MotionBlur>(out motionBlur);
        postProcessVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);
        postProcessVolume.profile.TryGetSettings<DepthOfField>(out dOf);
        postProcessVolume.profile.TryGetSettings<ChromaticAberration>(out CA);

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
        grid = GameObject.FindGameObjectWithTag("grid").gameObject;

        isRetroLook = PlayerPrefs.GetInt("retroLook") == 1 ? true : false;
        lensDistortion.active = isRetroLook;
        vignette.active = isRetroLook;
        motionBlur.active = isRetroLook;
        colorGrading.active = isRetroLook;
        dOf.active = isRetroLook;
        CA.active = isRetroLook;
        grid.SetActive(isRetroLook);
        RLToggle.isOn = isRetroLook;

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

    public void RetroLook(bool is_retrolook)
    {
        isRetroLook = is_retrolook;
        PlayerPrefs.SetInt("retroLook", isRetroLook ? 1 : 0);
        lensDistortion.active = isRetroLook;
        vignette.active = isRetroLook;
        motionBlur.active = isRetroLook;
        colorGrading.active = isRetroLook;
        dOf.active = isRetroLook;
        CA.active = isRetroLook;
        grid.SetActive(isRetroLook);
        RLToggle.isOn = isRetroLook;

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
