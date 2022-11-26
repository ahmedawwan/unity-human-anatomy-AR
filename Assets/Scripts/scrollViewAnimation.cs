using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollViewAnimation : MonoBehaviour
{
    [Header("ScrollView")]
    public GameObject btnOpen;

    public void hidePlay()
    {
        btnOpen.SetActive(false);
    }
    public void showPlay()
    {
        btnOpen.SetActive(true);
    }
    
}
