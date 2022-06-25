using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//using UnityEngine.Localization;
//using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UIOptionsManager : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown frameRateDropdown;
    [SerializeField] private Toggle vsyncToggle;

    [Header("Audio")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [Space]
    [SerializeField] private AudioMixer audioMixer;

    // [Header("Language")]
    // [SerializeField] private Dropdown languageDropdown;
    // [SerializeField] private string playerPreferenceKey = "selected_locale";

    private string code;

    private int[] frameRates = { 30, 60, 120, 1000 };
    private Resolution[] resolutions;

    private void Awake()
    {
        ResetQuality(); //Auto populate dropdowns
        ResetResolution(); //Auto populate dropdowns

        //AddListener onValueChanged
        fullscreenToggle.onValueChanged.AddListener(Fullscreen);
        qualityDropdown.onValueChanged.AddListener(Quality);
        resolutionDropdown.onValueChanged.AddListener(Resolution);
        frameRateDropdown.onValueChanged.AddListener(FrameRate);
        vsyncToggle.onValueChanged.AddListener(Vsync);

        masterSlider.onValueChanged.AddListener(MasterVolume);
        musicSlider.onValueChanged.AddListener(MusicVolume);
        effectsSlider.onValueChanged.AddListener(EffectsVolume);

        //Set settings to default values if no saves
        if (!PlayerPrefs.HasKey("quality"))
        {
            ResetSettings();
        }
    }

    //IEnumerator
    private void Start()
    {
        Load();

        // // Wait for the localization system to initialize
        // yield return LocalizationSettings.InitializationOperation;

        // // Generate list of available Locales
        // var options = new List<Dropdown.OptionData>();
        // int selected = 0;
        // for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        // {
        //     var locale = LocalizationSettings.AvailableLocales.Locales[i];
        //     if (LocalizationSettings.SelectedLocale == locale)
        //         selected = i;
        //     options.Add(new Dropdown.OptionData(locale.Identifier.Code.ToUpper()));
        // }
        // languageDropdown.options = options;

        // languageDropdown.value = selected;
        // languageDropdown.onValueChanged.AddListener(LocaleSelected);
    }

    // #region Language
    // public void PostInitialization(LocalizationSettings settings)
    // {
    //     if (Application.isPlaying)
    //     {
    //         // Record the new selected locale so it can persist between runs
    //         var selectedLocale = settings.GetSelectedLocale();
    //         if (selectedLocale != null)
    //             PlayerPrefs.SetString(playerPreferenceKey, selectedLocale.Identifier.Code);
    //     }
    // }

    // public Locale GetStartupLocale(ILocalesProvider availableLocales)
    // {
    //     if (PlayerPrefs.HasKey(playerPreferenceKey))
    //     {
    //         code = PlayerPrefs.GetString(playerPreferenceKey);
    //         if (!string.IsNullOrEmpty(code))
    //         {
    //             return availableLocales.GetLocale(code);
    //         }
    //     }

    //     // No locale could be found
    //     return null;
    // }

    // private static void LocaleSelected(int index)
    // {
    //     LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    // }
    // #endregion

    #region Settings UI
    private void ResetQuality()
    {
        var qualityLevels = QualitySettings.names;

        var options = new List<Dropdown.OptionData>();
        int selected = 0;
        foreach (var names in qualityLevels)
        {
            string text = names;

            options.Add(new Dropdown.OptionData(text));
        }
        qualityDropdown.options = options;

        qualityDropdown.value = selected;
    }

    private void ResetResolution()
    {
        resolutions = Screen.resolutions;

        var options = new List<Dropdown.OptionData>();
        int selected = 0;
        foreach (var res in resolutions)
        {
            string text = res.width + "x" + res.height/* + " @" + res.refreshRate*/;

            options.Add(new Dropdown.OptionData(text));
        }
        resolutionDropdown.options = options;

        resolutionDropdown.value = selected;
    }
    #endregion

    #region Settings
    public void Quality(int value)
    {
        QualitySettings.SetQualityLevel(value);

        Vsync(vsyncToggle.isOn);
    }

    public void Resolution(int value)
    {
        var resolution = resolutions[value];
        Screen.SetResolution((int)resolution.width, (int)resolution.height, Screen.fullScreen);
    }

    public void FrameRate(int value)
    {
        Application.targetFrameRate = frameRates[value];
    }

    public void Fullscreen(bool value)
    {
        Screen.fullScreen = value;
    }

    public void Vsync(bool value)
    {
        if (!value) { QualitySettings.vSyncCount = 0; }
        else { QualitySettings.vSyncCount = 1; }
    }

    public void MasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void MusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void EffectsVolume(float volume)
    {
        audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
    }
    #endregion

    #region Save and Load
    public void ResetSettings()
    {
        PlayerPrefs.SetInt("quality", 2);
        PlayerPrefs.SetInt("resolution", 7);
        PlayerPrefs.SetInt("framerate", 1);
        PlayerPrefs.SetInt("fullscreen", boolToInt(false));
        PlayerPrefs.SetFloat("sensitivity", 1);
        PlayerPrefs.SetInt("vsync", 1);
        PlayerPrefs.SetFloat("master", 1);
        PlayerPrefs.SetFloat("music", 1);
        PlayerPrefs.SetFloat("effects", 1);

        //Debug.Log("Reset settings");
    }

    public void Load()
    {
        int quality = PlayerPrefs.GetInt("quality");
        int resolution = PlayerPrefs.GetInt("resolution");
        int framerate = PlayerPrefs.GetInt("framerate");
        bool fullscreen = intToBool(PlayerPrefs.GetInt("fullscreen", 0));
        bool vsync = intToBool(PlayerPrefs.GetInt("vsync", 0));

        float master = PlayerPrefs.GetFloat("master");
        float music = PlayerPrefs.GetFloat("music");
        float effects = PlayerPrefs.GetFloat("effects");

        masterSlider.value = master;
        MasterVolume(master);

        musicSlider.value = music;
        MusicVolume(music);

        effectsSlider.value = effects;
        EffectsVolume(effects);

        qualityDropdown.value = quality;
        Quality(quality);

        resolutionDropdown.value = resolution;
        Resolution(resolution);

        frameRateDropdown.value = framerate;
        FrameRate(framerate);

        fullscreenToggle.isOn = fullscreen;
        Fullscreen(fullscreen);

        vsyncToggle.isOn = vsync;
        Vsync(vsync);

        //Debug.Log("Load settings");
    }

    public void Save()
    {
        PlayerPrefs.SetInt("quality", qualityDropdown.value);
        PlayerPrefs.SetInt("resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("framerate", frameRateDropdown.value);
        PlayerPrefs.SetInt("fullscreen", boolToInt(fullscreenToggle.isOn));
        PlayerPrefs.SetInt("vsync", boolToInt(vsyncToggle.isOn));

        PlayerPrefs.SetFloat("master", masterSlider.value);
        PlayerPrefs.SetFloat("music", musicSlider.value);
        PlayerPrefs.SetFloat("effects", effectsSlider.value);

        //Debug.Log("Save settings");
    }

    private int boolToInt(bool value)
    {
        if (value)
            return 1;
        else
            return 0;
    }

    private bool intToBool(int value)
    {
        if (value != 0)
            return true;
        else
            return false;
    }

    private void OnApplicationQuit()
    {
        Save();
    }
    #endregion
}
