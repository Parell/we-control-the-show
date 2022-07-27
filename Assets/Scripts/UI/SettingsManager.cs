using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using SaveSystem;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Toggle vsyncToggle;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;
    [Space]
    [SerializeField] AudioMixer audioMixer;

    public List<Resolution> resolutions;

    SettingsData data = new SettingsData();

    void Start()
    {
        ResetResolution();

        Load();

        resolutionDropdown.onValueChanged.AddListener(Resolution);
        fullscreenToggle.onValueChanged.AddListener(Fullscreen);
        vsyncToggle.onValueChanged.AddListener(Vsync);

        masterSlider.onValueChanged.AddListener(MasterVolume);
        musicSlider.onValueChanged.AddListener(MusicVolume);
        effectsSlider.onValueChanged.AddListener(EffectsVolume);
    }

    #region Settings UI
    void ResetResolution()
    {
        int selected = 0;
        bool foundResolution = false;

        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
            {
                foundResolution = true;

                selected = i;
            }
        }

        if (!foundResolution)
        {
            Resolution newResolution = new Resolution();
            newResolution.width = Screen.width;
            newResolution.height = Screen.height;

            resolutions.Add(newResolution);
            selected = resolutions.Count - 1;
        }

        var options = new List<Dropdown.OptionData>();
        foreach (var res in resolutions)
        {
            string text = res.width + "x" + res.height;

            options.Add(new Dropdown.OptionData(text));
        }
        resolutionDropdown.options = options;

        resolutionDropdown.value = selected;
    }
    #endregion

    #region Settings
    public void Resolution(int value)
    {
        var resolution = resolutions[value];
        Screen.SetResolution((int)resolution.width, (int)resolution.height, fullscreenToggle.isOn);
    }

    public void Fullscreen(bool value)
    {
        Screen.fullScreen = value;
        Resolution(resolutionDropdown.value);
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

    public void RestoreDefaultSettings()
    {
        data.resolution = 3;
        data.fullscreen = false;
        data.vsync = true;
        data.master = 1;
        data.music = 1;
        data.effects = 1;
        Debug.Log("reset");
    }

    public void Load()
    {
        if (SaveGame.Exists("settings")) data = SaveGame.Load<SettingsData>("settings");
        else RestoreDefaultSettings();

        masterSlider.value = data.master;
        MasterVolume(data.master);

        musicSlider.value = data.music;
        MusicVolume(data.music);

        effectsSlider.value = data.effects;
        EffectsVolume(data.effects);

        fullscreenToggle.isOn = data.fullscreen;
        Fullscreen(data.fullscreen);

        vsyncToggle.isOn = data.vsync;
        Vsync(data.vsync);

        resolutionDropdown.value = data.resolution;
        Resolution(data.resolution);
        Debug.Log("load");
    }

    public void Save()
    {
        data.resolution = resolutionDropdown.value;
        data.fullscreen = fullscreenToggle.isOn;
        data.vsync = vsyncToggle.isOn;
        data.master = masterSlider.value;
        data.music = musicSlider.value;
        data.effects = effectsSlider.value;

        SaveGame.Save<SettingsData>("settings", data);
        Debug.Log("save");
    }

    void OnApplicationQuit()
    {
        Save();
    }
    #endregion
}

[System.Serializable]
public class Resolution
{
    public int width, height;
}

public class SettingsData
{
    public int resolution;
    public bool fullscreen, vsync;
    public float master, music, effects;
}