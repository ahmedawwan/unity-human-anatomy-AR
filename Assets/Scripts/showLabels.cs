using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class showLabels : MonoBehaviour
{
    [SerializeField] private TMP_Text details;
    private bool labels;
    void Start()
    {
        labels = false;
        details.gameObject.SetActive(labels);
    }
    public void Toggle()
    {

       details.gameObject.SetActive(details.gameObject.activeSelf);
    if(labels == false)
    {
        labels = true;
    }
    else if(labels == true)
    {
        labels = false;
    }
    details.gameObject.SetActive(labels);
    }


}
