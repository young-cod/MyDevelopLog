using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public enum EMenuInfo
    {
        Main,
        Tech,
        Game,

    }
    public EMenuInfo menuInfo = EMenuInfo.Main;

    public GameObject mainMenu;
    public GameObject techMenu;
    public GameObject gameMenu;

    public GameObject backBtn;
    public GameManager exitSceneBtn;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (menuInfo)
            {
                case EMenuInfo.Main:
                    Application.Quit();
                    break;
                case EMenuInfo.Tech:
                case EMenuInfo.Game:
                    SceneManager.LoadScene("UIScene");
                    mainMenu.SetActive(true);
                    backBtn.SetActive(false);
                    menuInfo = EMenuInfo.Main;
                    break;
                default:
                    break;
            }
        }
    }

    public void MainBtnClick(string str)
    {
        mainMenu.SetActive(false);

        backBtn.SetActive(true);
        switch (str)
        {
            case "Tech":
                techMenu.SetActive(true);
                menuInfo = EMenuInfo.Tech;
                break;
            case "Game":
                gameMenu.SetActive(true);
                menuInfo = EMenuInfo.Game;
                break;
            default:
                break;
        }
    }

    public void TechBtnClick(string tectStr)
    {
        SceneManager.LoadScene(tectStr);
        techMenu.SetActive(false);
        backBtn.SetActive(false);
    }

    public void BackBtn()
    {
        mainMenu.SetActive(true);


        switch (menuInfo)
        {
            case EMenuInfo.Tech:
                techMenu.SetActive(false);
                break;
            case EMenuInfo.Game:
                gameMenu.SetActive(false);
                break;
            default:
                break;
        }

        backBtn.SetActive(false);
        menuInfo = EMenuInfo.Main;
    }
}
