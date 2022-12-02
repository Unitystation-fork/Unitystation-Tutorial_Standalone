using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GUI_Tutorial : MonoBehaviour
{
    public GameObject languageChoice;
    public void OnTutorialButton(string choice)
    {
        ///Start tutorial
        GameManager.Instance.onTuto = true;
        GameManager.Instance.language = choice;
        LoadingScreenManager.LoadFromLobby(CustomNetworkManager.Instance.StartHost);
    }
    
    public void ChoiceLanguage()
    {
        languageChoice.SetActive(true);
    }
    public void ExitChoice()
    {
        languageChoice.SetActive(false);
    }
}
