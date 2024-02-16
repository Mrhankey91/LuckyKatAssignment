using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    private bool show = false;
    private Toggle invertToggle;
    private Slider sensitivitySlider;

    void Awake()
    {
        invertToggle = transform.Find("InvertToggle").GetComponent<Toggle>();
        sensitivitySlider = transform.Find("SensitivitySlider").GetComponent <Slider>();
        gameObject.SetActive(show);

        invertToggle.isOn = PlayerPrefs.GetInt("InvertMovement", 0) == 1;
        invertToggle.onValueChanged.AddListener(OnInvertToggleChanged);

        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 1f);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderChanged);
    }

    public void ToggleMenu()
    {
        show = !show;
        gameObject.SetActive(show);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnInvertToggleChanged(bool value)
    {
        PlayerPrefs.SetInt("InvertMovement", value ? 1 : 0);
    }

    private void OnSensitivitySliderChanged(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
    }

}
