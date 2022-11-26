using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class settingsScript : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pnlMenu;
    public GameObject btnMenu;
    public GameObject pnlAbout;
    public GameObject pnlSettings;

    
    
    public void homeButton()
    {
        allFalse();
        btnMenu.SetActive(true);
    }

    public void menuButton()
    {
        allFalse();
        pnlMenu.SetActive(true);
    }

    public void logoutButton()
    {
        allFalse();
        authenticationManager auth = new authenticationManager();
        auth.signOut();
    }

    public void aboutButton()
    {
        allFalse();
        pnlAbout.SetActive(true);
    }

    public void settingsButton()
    {
        allFalse();
        pnlSettings.SetActive(true);
    }


    private void allFalse()
    {
        pnlMenu.SetActive(false);
        btnMenu.SetActive(false);
        pnlAbout.SetActive(false);
        pnlSettings.SetActive(false);
    }
}
