using UnityEngine;

public class CheckScore : MonoBehaviour
{
    public int[] scores = { 10, 15, 20, 5, };

    private void Start()
    {
        CheckPlayerPerformance();
    }
    private void CheckPlayerPerformance()
    {
        int sum = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            sum += scores[i];
        }
        
        float average = (float)sum / scores.Length;
        Debug.Log($"Средний балл: {average}");
        
            if (average >= 50)
            {
                Debug.Log("Победа");
            }
            else
            {
                Debug.Log("Пора попробовать снова");
            }
    }
//Нужно ли заменять int на var?(13 и 14 строка) Были проблемы с циклом и средним значением(пробел в ЗУНах)
//Можно ли сделать public int[] scores со случайными значениями?
//14 строчка: создаю i = 0, говорю: если i < ДЛИНЫ МАССИВА, то прибавляю на 1, пока i не будет = ДЛИНЕ МАССИВА
//sum += scores[i] - складывает все элементы массива по очереди и сохраняет результат в переменную sum
}