using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class DiedMenu : MonoBehaviour
{
    public GameObject diedMenuUI;
    public void Pause()
    {
        Time.timeScale = 0f;  
        diedMenuUI.SetActive(true);      
    }
    public void ContinuePlaying ()
    {
        Time.timeScale = 1f;
        diedMenuUI.SetActive(false);
    }

    public void GoBackMenu ()
    {
        SceneManager.LoadScene("Level Selector");
        ContinuePlaying();
    }
}