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
    //������inspector�����������¼��
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

        //�� volume.profile��Ҳ������������ļ�������Ի�ȡһ������Ϊ ColorAdjustments �ĺ��������
        //������ڣ��Ͱ�����ֵ������ colorAdjustments��
        //TryGet<T>() �� VolumeProfile �ṩ��һ�� ���ͷ���,������ VolumeProfile �в���ָ�����͵�Ч�������
        volume.profile.TryGet(out colorAdjustments);

        resolutions = Screen.resolutions;

        //����б�
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
        //ˢ�������˵�����ʾ�ı�
        resolutionDropdown.RefreshShownValue();
    }


    public void StartGame()
    {
        //�л�����
        SceneManager.LoadScene(_newGameLevel);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    
    public void SetVolume(float volume)
    {
        //��volume��ֵ��AudioListener.volume��ֵ��0��1֮��
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");

        
    }


    public void VolumeApply()
    {
        //��AudioListener.volume��ֵ��"masterVolume"Ϊkey�洢��playerprefs��
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


    //��������
    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessValue.text = brightness.ToString("0.0");

        // value һ�㷶Χ��-2 ~ +2��URP��׼��
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

        //_isFullscreen ? 1 : 0���_isFullscreen Ϊtrue���� 1,��� _isFullscreen Ϊfalse���� 0
        PlayerPrefs.SetInt("masterIsFullscreen", (_isFullscreen ? 1 : 0));
    }

    
    //��Ļ�ֱ���
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


    //�첽�����³���������ʹ�����ں�̨����
    IEnumerator  LoadNewSceneASync(int index)
    {
        //�첽���أ�����̨����
        //AsyncOperation ��һ���첽���������࣬���Լ�ؿ����첽����

        //��Ϸ�Ῠסֱ�������������
        //SceneManager.LoadScene(index);
        //Debug.Log("���д���Ҫ�ȳ���������ɺ��ִ��");

        // �������أ���Ϸ��������
        //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(index);
        //Debug.Log("���д��������ִ�У����õȴ��������");
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(index);

        //loadOperation.isDone����첽�����Ƿ����
        while (!loadOperation.isDone)
        {
            //������0��һ֮��
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

            //Screen.currentResolution����Ļ���ֱ��ʣ���ֵ��Ĭ�Ϸֱ���
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
        }
    }
}
