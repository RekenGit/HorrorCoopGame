using Assets.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;

public class Binds : MonoBehaviour
{
    //Keyboard + mouse
    public TextMeshProUGUI bindForward;
    public TextMeshProUGUI bindBackward;
    public TextMeshProUGUI bindLeft;
    public TextMeshProUGUI bindRight;
    public TextMeshProUGUI bindJump;
    public TextMeshProUGUI bindCrouch;
    public TextMeshProUGUI bindInteraction;
    public TextMeshProUGUI bindFlashlight;

    //Sounds
    public Slider masterVolume;

    public bool isChangingBind = false;
    private string lastBindName;

    #region Binds
    public void ChangeKeyBind(string name)
    {
        isChangingBind = true;
        BindList.FindBind(name).BindLabel.text = "Press button";
        lastBindName = name;
    }

    public void ResetKeyBind(string name)
    {
        BindData _bind = BindList.FindBind(name);
        _bind.ChangeBind(_bind.BindkeyCodeDefault);
    }
    #endregion
    #region Sliders
    public void ResetSensivity() { UIManager.instance.sensivitySlider.value = 40; }
    public void ResetMasterVolume() { masterVolume.value = AudioListener.volume = 1; }
    public void OnSensivityChanged() { PlayerPrefs.SetFloat("Sensivity", UIManager.instance.sensivitySlider.value); }
    public void OnMasterVolumeChanged() { PlayerPrefs.SetFloat("MasterVolume", masterVolume.value); AudioListener.volume = masterVolume.value; }
    #endregion
    public void OnNickChanged()
    {
        PlayerPrefs.SetString("NickName", UIManager.instance.connectUsername.text);
    }

    private void Start()
    {
        UIManager.instance.connectUsername.text = PlayerPrefs.GetString("NickName");
        UIManager.instance.sensivitySlider.value = PlayerPrefs.GetFloat("Sensivity") == 0 ? 40 : PlayerPrefs.GetFloat("Sensivity");
        AudioListener.volume = masterVolume.value = PlayerPrefs.GetFloat("MasterVolume") == 0 ? 40 : PlayerPrefs.GetFloat("MasterVolume");
        BindList.Binds = new()
        {
            new BindData("Forward", bindForward, string.IsNullOrEmpty(PlayerPrefs.GetString("Forward")) ? KeyCode.W : (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Forward")), KeyCode.W),
            new BindData("Backward", bindBackward, string.IsNullOrEmpty(PlayerPrefs.GetString("Backward")) ? KeyCode.S : (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Backward")), KeyCode.S),
            new BindData("Left", bindLeft, string.IsNullOrEmpty(PlayerPrefs.GetString("Left")) ? KeyCode.A : (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left")), KeyCode.A),
            new BindData("Right", bindRight, string.IsNullOrEmpty(PlayerPrefs.GetString("Right")) ? KeyCode.D : (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right")), KeyCode.D),
            new BindData("Jump", bindJump, string.IsNullOrEmpty(PlayerPrefs.GetString("Jump")) ? KeyCode.Space : (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump")), KeyCode.Space),
            new BindData("Crouch", bindCrouch, string.IsNullOrEmpty(PlayerPrefs.GetString("Crouch")) ? KeyCode.LeftControl : (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch")), KeyCode.LeftControl),
            new BindData("Interaction", bindInteraction, string.IsNullOrEmpty(PlayerPrefs.GetString("Interaction")) ? KeyCode.E : (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interaction")), KeyCode.E),
            new BindData("Flashlight", bindFlashlight, string.IsNullOrEmpty(PlayerPrefs.GetString("Flashlight")) ? KeyCode.F : (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Flashlight")), KeyCode.F)
        };
    }

    private void Update()
    {
        if (isChangingBind)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    isChangingBind = false;
                    BindData _bind = BindList.FindBind(lastBindName);
                    _bind.BindLabel.text = key.ToString();
                    _bind.ChangeBind(key);
                    break;
                }
            }
        }
    }
}
