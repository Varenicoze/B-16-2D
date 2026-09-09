using System.Collections.Generic;
using Inventory.Item;
using UnityEngine;

namespace Inventory.Database
{
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Database", order = 0)]
    public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public ItemsObject[]  items;
        public Dictionary<int, ItemsObject> GetItem = new Dictionary<int, ItemsObject>();

        public void OnAfterDeserialize()
        {
            GetItem = new Dictionary<int, ItemsObject>();

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Id = i;
                GetItem.Add(i, items[i]);
            }
        }
        
        public void OnBeforeSerialize()
        {
            GetItem = new Dictionary<int, ItemsObject>();
        }
    }
}