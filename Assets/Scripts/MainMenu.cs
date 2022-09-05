using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public Button[] lvlbuttons;
    public Slider volume;
    private void Start()
    {
        volume.value = PlayerPrefs.GetFloat("volume");
        int levelAt = PlayerPrefs.GetInt("LevelAt",1);
        for (int i = 0; i < lvlbuttons.Length; i++)
        {
            var i1 = i;
            lvlbuttons[i].onClick.AddListener(
                delegate{
                    StartGame(i1);
                });
            if (i + 1 > levelAt)
            {
                lvlbuttons[i].interactable = false;
                lvlbuttons[i].GetComponent<Image>().maskable = false;

            }
        }
    }
    private void Update()
    {
        if(PlayerPrefs.GetFloat("volume",1)!=volume.value)
        PlayerPrefs.SetFloat("volume", volume.value);
    }
    public void StartGame(int index)
    {
        PlayerPrefs.SetInt("MapList", index);
        SceneManager.LoadScene("Game");
    }
  
    public void Deneme(int index)
    {
        Debug.Log(index);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
