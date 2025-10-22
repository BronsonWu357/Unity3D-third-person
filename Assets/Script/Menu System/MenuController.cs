using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider sliderValue = null;
    [SerializeField] private float defaultVolume = 0.5f;


    [Header("Gameplay Setting")]
    [SerializeField] private Slider sliderControllerSen = null;
    [SerializeField] private TMP_Text controllerSenValue = null;
    [SerializeField] private bool isInvertY = false;
    [SerializeField] private int defaultControllerSen = 4;
    private float mainControllerSen = 4f;
    [SerializeField] private Toggle invertYToggle = null;


    [Header("Graphics Setting")]
    [SerializeField] private Slider sliderBrightness = null;
    [SerializeField] private TMP_Text brightnessValue = null;
    [SerializeField] private float defaultBrightness = 1.0f;
    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;

    private int _qualityLevel;
    private bool _isFullscreen;
    private float _brightnessLevel;

    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    //用来在inspector界面增加上下间距
    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;


    [Header("Scene")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private int _newGameLevel = 1;


    public void Start()
    {
        Screen.fullScreen = true;

        //从 volume.profile（也就是体积配置文件）里，尝试获取一个类型为 ColorAdjustments 的后处理组件，
        //如果存在，就把它赋值给变量 colorAdjustments。
        //TryGet<T>() 是 VolumeProfile 提供的一个 泛型方法,用来在 VolumeProfile 中查找指定类型的效果组件。
        volume.profile.TryGet(out colorAdjustments);

        resolutions = Screen.resolutions;

        //清空列表
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        Screen.fullScreen = true;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        //刷新下拉菜单的显示文本
        resolutionDropdown.RefreshShownValue();
    }


    public void StartGame()
    {
        //切换场景
        SceneManager.LoadScene(_newGameLevel);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    
    public void SetVolume(float volume)
    {
        //将volume赋值给AudioListener.volume，值在0到1之间
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");

        
    }


    public void VolumeApply()
    {
        //将AudioListener.volume的值以"masterVolume"为key存储到playerprefs中
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }


    public void setControllerSen(float controllerSen)
    {
        mainControllerSen = controllerSen;
        controllerSenValue.text = controllerSen.ToString("0");
    }


    public void SetInvertY(bool invertY)
    {
        isInvertY = invertY;
    }


    public void GameplayApply()
    {
        PlayerPrefs.SetFloat("controllerSen", mainControllerSen);
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetFloat("invertY", 1);
        }
        else
        {
            PlayerPrefs.SetFloat("invertY", 0);
        }
    }


    //设置亮度
    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessValue.text = brightness.ToString("0.0");

        // value 一般范围：-2 ~ +2（URP标准）
        colorAdjustments.postExposure.value = brightness;
    }


    public void SetFullscreen(bool isFullscreen)
    {
        _isFullscreen = isFullscreen;

        Screen.fullScreen = _isFullscreen;
    }


    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }


    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);

        PlayerPrefs.SetInt("masterQuality",_qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        //_isFullscreen ? 1 : 0如果_isFullscreen 为true返回 1,如果 _isFullscreen 为false返回 0
        PlayerPrefs.SetInt("masterIsFullscreen", (_isFullscreen ? 1 : 0));
    }

    
    //屏幕分辨率
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    public void LoadNewScene()
    {
        mainMenu.SetActive(false);
        LoadingScreen.SetActive(true);

        StartCoroutine(LoadNewSceneASync(_newGameLevel));
    }


    //异步加载新场景，可以使场景在后台加载
    IEnumerator  LoadNewSceneASync(int index)
    {
        //异步加载，即后台加载
        //AsyncOperation 是一个异步操作控制类，可以监控控制异步操作

        //游戏会卡住直到场景加载完成
        //SceneManager.LoadScene(index);
        //Debug.Log("这行代码要等场景加载完成后才执行");

        // 立即返回，游戏继续运行
        //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(index);
        //Debug.Log("这行代码会立即执行，不用等待加载完成");
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(index);

        //loadOperation.isDone检查异步操作是否完成
        while (!loadOperation.isDone)
        {
            //限制在0到一之间
            float progressValue = Mathf.Clamp01(loadOperation.progress);

            loadingSlider.value = progressValue;
            yield return null;
        }
    }


    public void Reset(string menuType)
    {
        if (menuType.Equals("Audio"))
        {
            sliderValue.value = defaultVolume;
            SetVolume(defaultVolume);
        }

        if (menuType.Equals("Gameplay"))
        {
            sliderControllerSen.value = defaultControllerSen;
            setControllerSen(defaultControllerSen);
            invertYToggle.isOn = false;
        }

        if (menuType.Equals("Graphics"))
        {
            sliderBrightness.value = defaultBrightness;
            brightnessValue.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullscreenToggle.isOn = true;
            Screen.fullScreen = true;

            //Screen.currentResolution是屏幕最大分辨率，赋值给默认分辨率
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
        }
    }
}
