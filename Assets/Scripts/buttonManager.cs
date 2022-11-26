using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class buttonManager : MonoBehaviour
{
    private Button btn;
    [SerializeField] private RawImage buttonImage;
    public GameObject Anatomy;
    private int _itemID;
    private Sprite _ButtonTexture;
    public int ItemID{
        set=> _itemID = value;
    }

    public Sprite ButtonTexture{
        set{
            _ButtonTexture = value;
            buttonImage.texture = _ButtonTexture.texture;
        }
    }
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(selectAnatomy);
    }

    void Update()
    {
        if(zoomManager.Instance.onEntered(gameObject))
        {
            transform.DOScale(Vector3.one*2, 0.3f);
        }
        else
        {
         transform.DOScale(Vector3.one, 0.3f);
        }
    }

    void selectAnatomy()
    {
        dataHandler.Instance.SetAnatomy(_itemID);

    }
}
