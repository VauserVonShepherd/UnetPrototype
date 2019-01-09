using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {
    public static MainMenuManager instance;

    //List for menus
    [Tooltip("All Menu in Main Menu, 0 Lobby, 1 Room")]
    public List<GameObject> MenuList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void SetMenu(int menuVal)
    {
        foreach (GameObject menu in MenuList)
        {
            menu.SetActive(false);
        }

        MenuList[menuVal].SetActive(true);
    }
}
