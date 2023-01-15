using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject mainmenu;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject graphics;
    [Header("Volume Setting")]
    [SerializeField] private TextMeshProUGUI volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;
    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    [Header("Resolution Dropdown")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private void Start()
    {

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        SetResolution(currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        _isFullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;
    }

    public void loadOptions()
    {
        mainmenu.SetActive(false);
        options.SetActive(true);
    }

    public void loadMenu()
    {
        mainmenu.SetActive(true);
        options.SetActive(false);
    }

    public void loadAudioMenu()
    {
        audioMenu.SetActive(true);
        options.SetActive(false);
    }

    public void loadGraphics()
    {
        graphics.SetActive(true);
        options.SetActive(false);
    }

    public void loadOptions2()
    {
        graphics.SetActive(false);
        audioMenu.SetActive(false);
        options.SetActive(true);
    }

    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

     public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Graphics")
        {

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        } 

        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
    }
}
