using Inventory.Container;
using Inventory.Database;
using Inventory.Item;
using NUnit.Framework;
using UnityEngine;
using InventoryContainerData = Inventory.Container.Inventory;

namespace Inventory.Tests
{
    public class TestItemsObject : ItemsObject
    {
    }
    
    public class InventoryObjectTest
    {
        private const int SlotCount = 4;
        
        private ItemDatabaseObject _database;
        private InventoryObject _inventory;

        [SetUp]
        public void Setup()
        {
            /*_database = BuildDatabase(
                (id: 0, name: "Wood", maxStack: 10),
                (id: 1, name: "Stone", maxStack: 10)
                );*/
            
            _inventory = ScriptableObject.CreateInstance<InventoryObject>();
            _inventory.database = _database;
            /*_inventory.container = new InventoryContainerData
            {
                Items = BuildEmptySlots(SlotCount)
            };*/
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_inventory);

            foreach (var item in _database.Items)
            {
                if (item)
                    Object.DestroyImmediate(item);
            }
            
            Object.DestroyImmediate(_database);
        }

        private Item.Item GetItem(int id)
        {
            return _database.GetItem[id].CreateItem();
        }

        [Test]
        public void AddItem_PutsItemIntoEmptySlot()
        {
            var item = GetItem(0);
            
            _inventory.AddItem(item, 3);
            
            Assert.AreEqual(0, _inventory.container.Items[0].ID);
            Assert.AreEqual(3, _inventory.container.Items[0].amount);
        }

        [Test]
        public void AddItem_AddsToExistingStack()
        {
            _inventory.container.Items[0].UpdateSlot(0, 4, _database);
            var item = GetItem(0);
            
            _inventory.AddItem(item, 3);
            
            Assert.AreEqual(7, _inventory.container.Items[0].amount);
        }

        [Test]
        public void AddItem_CreateNewStack_WhenCurrentStackIsFull()
        {
            _inventory.container.Items[0].UpdateSlot(0, 9, _database);
            var item = GetItem(0);
            
            _inventory.AddItem(item, 3);
            
            Assert.AreEqual(0, _inventory.container.Items[1].ID);
            Assert.AreEqual(4, _inventory.container.Items[0].amount);
        }

        [Test]
        public void AddItem_SplitsItemsBetweenSlots()
        {
            var item = GetItem(0);
            
            _inventory.AddItem(item, 25);
            
            Assert.AreEqual(10, _inventory.container.Items[0].amount);
            Assert.AreEqual(10, _inventory.container.Items[1].amount);
            Assert.AreEqual(5, _inventory.container.Items[2].amount);
        }

        [Test]
        public void AddItem_DoesNothing_WhenInventoryIsFull()
        {
            var item = GetItem(0);
            
            for (int i = 0; i < 3; i++)
            {
                _inventory.container.Items[i].UpdateSlot(1, 1, _database);
            }
            Assert.DoesNotThrow(() => _inventory.AddItem(item, 5));
        }

        [Test]
        public void MoveItem_SwapDifferentItems()
        {
            _inventory.container.Items[0].UpdateSlot(0, 4, _database);
            _inventory.container.Items[1].UpdateSlot(1, 1, _database);
            
            /*_inventory.container.MoveItem(_inventory.container.Items[1], _inventory.container.Items[0]);*/
            
            Assert.AreEqual(1, _inventory.container.Items[0].ID);
            Assert.AreEqual(1, _inventory.container.Items[0].amount);
            
            Assert.AreEqual(1, _inventory.container.Items[1].ID);
            Assert.AreEqual(1, _inventory.container.Items[0].amount);
        }
    }
}