using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "ScoreManager", menuName = "ScriptableObjects/NewScoreManager")]
public class ScoreManagerOld : ScriptableObject
{

    struct food
    {
        public string name;
        public float value;
        public int count;
    }

    public int NumberScene
    {
        get { return numberScene; }
        set { numberScene = value; }
    }

    // public int Lifes { get { return countLifes; } }
    public float CurrentHunger { get { return countHunger; } }

    private List<food> catalogFoods = new List<food>();
    private food singleFood = new food();

    [SerializeField]
    private int countLifes,
                                 numberScene;
    [SerializeField] private float countHunger;
    private float calories;

    public float startHunger;
    public Vector3 startPlayerPosition;
    public bool isFinish;

    public event UnityAction<float> NotifyHunger;                                         // Обявляем событие  (изменился ГОЛОД)
    public event UnityAction<int> NotifyLife;                                           // Обявляем событие  (изменилось кол-во ЖИЗНЕЙ) 
    public event UnityAction NotifyDeath;                                          // Объявляем событие (УМЕР)

    public void InitDateBaseFoods()
    {
        catalogFoods.Clear();                                                              // Очищаем справочник Foods

        singleFood.name = "banana";
        singleFood.value = -0.2f;
        catalogFoods.Add(singleFood);

        singleFood.name = "mushroom";
        singleFood.value = 0.8f;
        catalogFoods.Add(singleFood);

        singleFood.name = "orange";
        singleFood.value = -0.2f;
        catalogFoods.Add(singleFood);

        singleFood.name = "lemon";
        singleFood.value = -0.1f;
        catalogFoods.Add(singleFood);

        singleFood.name = "grape";
        singleFood.value = -0.3f;
        catalogFoods.Add(singleFood);

        /*for (int i = 0; i < catalogFoods.Count; i++)
        {
            Debug.Log(catalogFoods[i].name);
            Debug.Log(catalogFoods[i].value);
            Debug.Log(catalogFoods[i].count);
        }*/
    }

    private void OnEnable()
    {
        isFinish = false;
        countLifes = 3;
        startHunger = 0f;
        countHunger = 0f;
        calories = 0f;
        InitDateBaseFoods();
    }

    public void AddFoodCount(string food)
    {
        for (int i = 0; i < catalogFoods.Count; i++)
        {
            if (catalogFoods[i].name.Contains(food))
            {
                var tmp = catalogFoods[i];
                tmp.count++;
                catalogFoods[i] = tmp;

                CountHunger();                                                           // Расчет голода

                //  Debug.Log("ADDfoodCount   Name  " + catalogFoods[i].name);
                //  Debug.Log("ADDfoodCount  Count  " + catalogFoods[i].count);
                //  Debug.Log("ADDfoodCount  CountHunger" + countHunger);
            }
        }
    }


    private void CountHunger()
    {
        for (int i = 0; i < catalogFoods.Count; i++)
            calories += catalogFoods[i].value * catalogFoods[i].count;                // расчет калорий всех собранных продуктов при изменении их количества
        countHunger = startHunger + calories;


        if (countHunger >= 1)
        {
            CountLifes();
            ResetCountFood();                                                        // Обнуляем счеечики
            NotifyHunger?.Invoke(startHunger);                                       // Вызываем событие
        }
        else
        {
            NotifyHunger?.Invoke(countHunger);                                       // Вызываем событие 
        }
        calories = 0f;
    }

    private void ResetCountFood()
    {
        for (int i = 0; i < catalogFoods.Count; i++)
        {
            var tmp = catalogFoods[i];
            tmp.count = 0;
            catalogFoods[i] = tmp;
        }
    }

    public void CountLifes()
    {
        countLifes--;
        NotifyLife?.Invoke(countLifes);                                              // Вызываем событие 
        if (countLifes <= 0)
            NotifyDeath?.Invoke();                                                   // Вызывеем событие "Изя Все"
    }
}
