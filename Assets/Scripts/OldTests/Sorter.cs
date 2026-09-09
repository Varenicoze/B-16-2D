using UnityEngine;

public class Sorter : MonoBehaviour
{
    public int[] unsort = { 17, 15, 3, 5, 4, 8, 10 };

    private void Start()
    {
        Sorting();
    }

    private void Sorting()
    {
        if (unsort == null || unsort.Length == 0)
        {
            Debug.LogWarning("Массив unsort пуст");
            return;
        }
        Debug.Log($"Unsorted array is {string.Join(",", unsort)}");
        
        for (int i = 0; i < unsort.Length - 1; i++)
        {
            for (int j = i + 1; j < unsort.Length; j++)
            {
                if (unsort[i] > unsort[j])
                { 
                    int temp = unsort[i];
                    unsort[i] = unsort[j]; 
                    unsort[j] = temp;
                }
            }
        }
        
        Debug.Log($"Sorted array is {string.Join(",", unsort)}");
    }
    //Некорректно расположил Debug.Log($"Unsorted array). Использовал int temp = unsort[i]; с метанита
}