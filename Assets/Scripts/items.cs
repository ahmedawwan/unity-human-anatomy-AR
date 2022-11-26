using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Item1", menuName = "AddItem/Item")]
public class items : ScriptableObject
{
   public GameObject itemPrefab;
   public Sprite itemImage;
}
