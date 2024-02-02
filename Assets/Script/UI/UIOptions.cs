// Przypisz do obiektu: Menu - Options
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptions : MonoBehaviour
{
    public GameObject keyboardAndMouse;
    public GameObject graphic;
    public GameObject sounds;
    public GameObject home;

    #region OpenOptionsTab
    public void OpenOptionKeyboardAndMouse()
    {
        home.SetActive(false);
        keyboardAndMouse.SetActive(true);
    }
    public void OpenOptionGraphic()
    {
        home.SetActive(false);
        graphic.SetActive(true);
    }
    public void OpenOptionSounds()
    {
        home.SetActive(false);
        sounds.SetActive(true);
    }
    public void OpenOptionHome()
    {
        keyboardAndMouse.SetActive(false);
        graphic.SetActive(false);
        sounds.SetActive(false);
        home.SetActive(true);
    }
    #endregion
}
