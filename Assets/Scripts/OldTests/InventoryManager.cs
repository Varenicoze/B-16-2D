using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int[] items = { 10, 25, 30, 90, 70, 85, 100 };

    private void Start()
    {
        CheckInventory();
    }

    private void CheckInventory()
    {
        int totalValue = 0;
        int rarityChecker = 0;
        for (int i = 0; i < items.Length; i++)
        {
            totalValue += items[i];
            if (items[i] > 70)
            {
                rarityChecker ++;
            }
        } 
        Debug.Log($"Общая ценность инвентаря: {totalValue}");
        Debug.Log($"Количество редких предметов: {rarityChecker}");

        if (rarityChecker > 2)
        {
            Debug.Log("Богатый игрок");
        }
        else
        {
            Debug.Log("Обычный игрок");
        }
    }
    //Не понимал, почему неправильно подсчитывалась сумма и if (rarityChecker > 2). Нужно было ресетнуть скрипт в юньке
}