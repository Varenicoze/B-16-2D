using UnityEngine;

public class CheckOpEnemy : MonoBehaviour
{
    public int[] enemyHealth = { 10, 25, 40, 50, 70, 85, 100 };

    private void Start()
    {
        CheckEnemyHealth();
    }

    private void CheckEnemyHealth()
    {
        if (enemyHealth == null || enemyHealth.Length == 0)
        {
            Debug.LogWarning("Массив enemyHealth пуст");
            return;
        }
        
        int maxHealth = enemyHealth[0];
        
        for (int i = 1; i < enemyHealth.Length; i++)
        {
            if (enemyHealth[i] > maxHealth)
            {
                maxHealth = enemyHealth[i];
            }
        }
        
        string enemyType = "";
        
        if (maxHealth >= 100)
        {
            enemyType = "Босс";
            Debug.Log("Босс");
        }
        else if (maxHealth >= 50)
        {
            enemyType = "Элитный";
            Debug.Log("Элитный");
        }
        else
        {
            enemyType = "Обычный";
            Debug.Log("Обычный");
        }
        Debug.Log($"Найден враг с максимальным здоровьем: {maxHealth}. Тип: {enemyType}");
    }
    //Была проблема с максимумом, не знал, как записать конструкцию
    //Пытался через Linq первоначально, понял, что не стоит(GarbageCollector не одобряет)
    //Возникла проблема с if else(Не присвоил значение - получил ошибку)
    // if (enemyHealth == null || enemyHealth.Length == 0) - чтоб не было ошибки при изменении массива
    //string enemyType = ""; на всякий случай, вдруг эта локальная переменная не инициализируется
}