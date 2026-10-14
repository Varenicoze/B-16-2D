/*using Inventory.Container;
using Inventory.Database;
using Inventory.Items;
using NUnit.Framework;
using Tests.Inventory;
using UnityEngine;
using Inventory;
using InventoryContainerData = Inventory.Container.Inventory;

namespace Tests.Inventory
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
            _database = TestHelpers.CreateTestDatabase(10, 1);//
            _inventory = TestHelpers.CreateTestInventory(_database, 3);//
            
            _database = BuildDatabase(
                (id: 0, name: "Wood", maxStack: 10),
                (id: 1, name: "Stone", maxStack: 5)
            );

            _inventory = ScriptableObject.CreateInstance<InventoryObject>();
            _inventory.database = _database;
            _inventory.container = new InventoryContainerData
            {
                Items = BuildEmptySlots(SlotCount)
            };
        }

        [TearDown]
        public void TearDown()
        {
            TestHelpers.Destroy(_inventory);//
            TestHelpers.Destroy(_database);//
            
            Object.DestroyImmediate(_inventory);

            foreach (var item in _database.Items)
            {
                if (item)
                    Object.DestroyImmediate(item);
            }

            Object.DestroyImmediate(_database);
        }
        
        //Helpers

        private static ItemDatabaseObject BuildDatabase(params (int id, string name, int maxStack)[]
        {
            var db = ScriptableObject.CreateInstance<ItemDatabaseObject>();
            db.Items = new ItemsObject[defs.Length];

            for (int i = 0; i < defs.Length; i++)
            {
                var item = ScriptableObject.CreateInstance<ItemDatabaseObject>();
                item.Id = defs[i].id;
                item.Name = defs[i].name;
                item.MaxStack = defs[i].maxStack;
                db.Items[i] = item;
            }

            db.OnAfterDeserialize();
            return db;
        }

        private static InventorySlot[] BuildEmptySlots(int count)
        {
            var slots = new InventorySlot[count];
            for (int i = 0; i < count; i++)
                slots[i] = new InventorySlot();
            return slots;
        }

        private Items.Item Wood => _database.GetItem[0].CreateItem();
        private Items.Item Stone => _database.GetItem[1].CreateItem();

        // AddItem

        [Test]
        public void AddItem_ToEmptyInventory_PlacesInFirstEmptySlot()
        {
            _inventory.AddItem(Wood, 3);

            Assert.AreEqual(0, _inventory.container.Items[0].ID);
            Assert.AreEqual(3, _inventory.container.Items[0].amount);
        }

        [Test]
        public void AddItem_WithExistingPartialStack_MergesBeforeUsingEmptySlot()
        {
            _inventory.container.Items[0].UpdateSlot(0, 4, _database); //Wood, 4/10

            _inventory.AddItem(Wood, 3);

            Assert.AreEqual(7, _inventory.container.Items[0].amount);
            Assert.AreEqual(-1, _inventory.container.Items[1].ID); //untouched
        }

        [Test]
        public void AddItem_OverflowingSingleStack_SpillsIntoNewSlot()
        {
            _inventory.container.Items[0].UpdateSlot(0, 8, _database); //Wood, 8/10

            _inventory.AddItem(Wood, 5); // 2 fits in slot 0, 3 spills into a new slot

            Assert.AreEqual(10, _inventory.container.Items[0].amount);
            Assert.AreEqual(10, _inventory.container.Items[1].amount);
            Assert.AreEqual(-1, _inventory.container.Items[2].ID);
        }

        [Test]
        public void AddItem_FillsAllMatchingPartialStackBeforeNewSlots()
        {
            _inventory.container.Items[0].UpdateSlot(0, 8, _database);
            _inventory.container.Items[1].UpdateSlot(0, 7, _database);

            _inventory.AddItem(Wood, 5); // 2 -> slot0, 3 -> slot1, nothing left over

            Assert.AreEqual(10, _inventory.container.Items[0].amount);
            Assert.AreEqual(10, _inventory.container.Items[1].amount);
            Assert.AreEqual(-1, _inventory.container.Items[2].ID); // no new slot added
        }

        [Test]
        public void AddItem_WhenInventoryFull_SilentyDropsExcess()
        {
            for (int i = 0; i < SlotCount; i++)
                _inventory.container.Items[i].UpdateSlot(1, 5, _database); // Stone, maxed out

            Assert.DoesNotThrow(() => _inventory.AddItem(Stone, 1));

            for (int i = 0; i < SlotCount; i++)
                Assert.AreEqual(5, _inventory.container.Items[i].amount);
        }

        [Test]
        public void AddItem_ExeedingRemainingCapacity_FillsWhatFitsAndDropsRest()
        {
            // Only slot 3 is free; requesting 25 with MaxStack 10 means
            // 10 gets placed and 15 is silently lost (current behavior)
            _inventory.container.Items[0].UpdateSlot(0, 10, _database);
            _inventory.container.Items[1].UpdateSlot(0, 10, _database);
            _inventory.container.Items[2].UpdateSlot(0, 10, _database);

            _inventory.AddItem(Wood, 25);

            Assert.AreEqual(0, _inventory.container.Items[3].amount);
            Assert.AreEqual(10, _inventory.container.Items[3].amount);
        }

        //Remove Item

        [Test]
        public void RemoveItem_ClearsAmountAndCachedItem()
        {
            var slot = _inventory.container.Items[0];
            slot.UpdateSlot(0, 5, _database);

            _inventory.RemoveItem(slot);

            Assert.AreEqual(-1, slot.ID);
            Assert.AreEqual(0, slot.amount);
            Assert.IsNull(slot.item);
        }

        //Move Item

        [Test]
        public void MoveItem_SameSlotReference_DoesNothing()
        {
            var slot = _inventory.container.Items[0];
            slot.UpdateSlot(0, 5, _database);

            _inventory.MoveItem(slot, slot);

            Assert.AreEqual(0, slot.ID);
            Assert.AreEqual(5, slot.amount);
        }

        [Test]
        public void MoveItem_DifferentItems_SwapContents()
        {
            var slotA = _inventory.container.Items[0];
            var slotB = _inventory.container.Items[1];
            slotA.UpdateSlot(0, 3, _database); //Wood x3
            slotB.UpdateSlot(1, 2, _database); //Stone x2

            _inventory.MoveItem(slotA, slotB);

            Assert.AreEqual(1, slotA.ID);
            Assert.AreEqual(2, slotA.amount);
            Assert.AreEqual(0, slotB.ID);
            Assert.AreEqual(3, slotB.amount);
        }

        [Test]
        public void MoveItem_SameItem_MergesFullyWhenUnderMaxStack()
        {
            var slotA = _inventory.container.Items[0];
            var slotB = _inventory.container.Items[1];
            slotA.UpdateSlot(0, 4, _database); // Wood x4, MaxStack 10
            slotB.UpdateSlot(0, 3, _database); // Wood x3

            _inventory.MoveItem(slotA, slotB); // toSlot = A, fromSlot = B

            Assert.AreEqual(7, slotA.amount);
            Assert.AreEqual(-1, slotB.ID);
            Assert.AreEqual(0, slotB.amount);
        }

        [Test]
        public void MoveItem_SameItem_PartialMergeWhenExceedingMaxStack()
        {
            var slotA = _inventory.container.Items[0];
            var slotB = _inventory.container.Items[1];
            slotA.UpdateSlot(0, 8, _database); // Wood x8, MaxStack 10, space = 2
            slotB.UpdateSlot(0, 5, _database); // Wood x5

            _inventory.MoveItem(slotA, slotB);

            Assert.AreEqual(10, slotA.amount); //Filled to Max
            Assert.AreEqual(0, slotB.ID); // still Wood, not cleared
            Assert.AreEqual(3, slotB.amount); // 5 - 2 moved over
        }

        // ClearSlots

        [Test]
        public void ClearSlots_ResetsEveryContainerSlot()
        {
            _inventory.container.Items[0].UpdateSlot(0, 5, _database);
            _inventory.container.Items[2].UpdateSlot(1, 2, _database);

            _inventory.ClearSlots();

            foreach (var slot in _inventory.container.Items)
            {
                Assert.AreEqual(-1, slot.ID);
                Assert.AreEqual(0, slot.amount);
            }
        }
    }
    
    public class InventorySlotTests
    {
        private ItemDatabaseObject _database;

        [SetUp]
        public void Setup()
        {
            var item = ScriptableObject.CreateInstance<TestItemsObject>();
            item.Id = 0;
            item.Name = "Wood";
            item.MaxStack = 10;
            
            _database = ScriptableObject.CreateInstance<ItemDatabaseObject>();
            _database.Items = new ItemsObject[] { item };
            _database.OnAfterDeserialize();
        }

        [TearDown]
        public void TearDown()
        { 
            foreach (var item in _database.Items)
                Object.DestroyImmediate(item);
            
            Object.DestroyImmediate(_database);
        }

        [Test]
        public void CanPlaceInSlot_EmptySlot_ReturnsTrue()
        {
            var slot = new InventorySlot();
            Assert.IsTrue(slot.CanPlaceInSlot(null));
        }

        [Test]
        public void CanPlaceInSlot_EmptySlot_ReturnsFalse()
        {
            var slot = new InventorySlot(0, 5);
            Assert.IsFalse(slot.CanPlaceInSlot(null));
        }

        [Test]
        public void UpdatesSlot_SetsIdAmountAndCachedItem()
        {
            var slot = new InventorySlot();
            slot.UpdateSlot(0, 4, _database);
            
            Assert.AreEqual(0, slot.ID);
            Assert.AreEqual(4, slot.amount);
            Assert.IsNotNull(slot.item);
            Assert.AreEqual("Wood", slot.item.Name);
        }

        [Test]
        public void UpdateSlot_WithNegativeId_ClearsCachedItem()
        {
            var slot = new InventorySlot();
            slot.UpdateSlot(0, 4, _database);
            
            slot.UpdateSlot(-1, 0, _database);
            
            Assert.IsNull(slot.item);
        }

        [Test]
        public void UpdateSlot_RaisesOnChangedEvent()
        {
            var slot = new InventorySlot();
            bool raised = false;
            slot.OnChanged += () => raised = true;
            
            slot.UpdateSlot(0, 1, _database);
            
            Assert.IsTrue(raised);
        }

        [Test]
        public void AddAmount_IncreasesAmountBySpecificValue()
        {
            var slot = new InventorySlot();
            
            slot.AddAmount(3);
            
            Assert.AreEqual(8, slot.amount);
        }
    }
}*/