using System;
using System.Collections;
using System.Collections.Generic;
//using System.Threading.Tasks;
using UnityEngine;
//using UnityEngine.AddressableAssets;
public class dataHandler : MonoBehaviour
{
   private GameObject anatomy;
   [SerializeField] private buttonManager _buttonPrefab;
   [SerializeField] private GameObject buttonContainer;
   [SerializeField] private List<items> _items;
   [SerializeField] private String label;
   private int current_id = 0;

      private static dataHandler instance;

      public void Start()         // async if using addressables
      {
          LoadItems();
          //await Get(label);
          CreateButton();
      }
   

   public static dataHandler Instance 
   { 
       get {
           if (instance == null)
           {
               instance = FindObjectOfType<dataHandler>();
           }
           return instance;
       } 
   }

   void CreateButton()
   {
       foreach(items i in _items){
           buttonManager b = Instantiate(_buttonPrefab, buttonContainer.transform);
           b.ItemID = current_id;
           b.ButtonTexture = i.itemImage;
           current_id++;
       }

   }

   public void SetAnatomy(int id){
       anatomy = _items[id].itemPrefab;

   }

   public GameObject GetAnatomy(){
       return anatomy;
   }
   

   void LoadItems(){
       var items_obj = Resources.LoadAll("Items", typeof(items));
       foreach(var item in items_obj)
       {
           _items.Add(item as items);
       }
   }

  // public async Task Get(String label)
   //{
//       var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
  //     foreach (var location in locations)
   //    {
//           var obj = await Addressables.LoadAssetAsync<items>(location).Task;
//           _items.Add(obj);

 //      }

//   }
}
