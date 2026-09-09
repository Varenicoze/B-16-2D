using UnityEngine;

public class EnemyWavesManager : MonoBehaviour
{
    public int[] waves = { 17, 15, 3, 5, 4, 8, 10 };

    private void Start()
    {
        CheckWaves();
    }

    private void CheckWaves()
    {
        int totalEnemyAmount = 0;
        for (int i = 0; i < waves.Length; i++)
        {
            totalEnemyAmount += waves[i];

        } 
        Debug.Log($"Всего врагов: {totalEnemyAmount}");
        
        if (totalEnemyAmount > 30)
        {
            Debug.Log("Сложный уровень");
        }
        else
        {
            Debug.Log("Легкий уровень");
        }

        if (waves.Length == 0) return;
        int enemyAmount = waves[0];
        for (int i = 1; i < waves.Length; i++)
        {
            if (waves[i] > enemyAmount)
            {
                enemyAmount = waves[i];
            }
        }
        if (enemyAmount < 5)
        {
            Debug.Log("Легкая волна");
        }
        else if (enemyAmount > 10)
        {
            Debug.Log("Сложная волна");
        }
        else
        {
            Debug.Log("Средняя волна");
        }
    }
    //Почему сначала легкий уровень, а потом сложный?? Потому что вынести if (totalEnemyAmount > 30) за Debug.Log надо
}