using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] SoundManager soundManager;
    [SerializeField] InitFood initFood;                                           // —сылка на каталог еды в ScriptableObject

    /*==============================================================
    public class Food
    {
        public string Name  { get; set; }
        public float  Value { get; set; }
        public int    Count { get; set; }
    }

    public List<Food> catalogFoods = new List<Food>();                               // Ѕаза данных еды
    private Food singleFood = new Food();                                            // Ёкземпл€р еды
    ==================================================================*/

    private List<GameObject> listByTag = new List<GameObject>();                    // Ћист объектов которые необходимо все собрать в уровне если таковые есть
    public List<int> listIDCollection = new List<int>();                            // Ћист ID вышеполученных объектов(уникальные записи)

    public event UnityAction<int> NotifyLife;                                        // ќб€вл€ем событие  (изменилось кол-во ∆»«Ќ≈…)
    public event UnityAction NotifyDeath;                                            // ќбъ€вл€ем событие (”ћ≈–)
       
    void Start()
    {
        if (scoreManager.NumberScene == 2)
        {
            InitIDCollection("kokonut");                                                 // заполн€ем массив ID объектов дл€ сбора
            InitIDCollection("bone");                                                    // кокoсы и кости
        }
        if (scoreManager.NumberScene == 0)                                      
            ResetFoodCount();
                
       // InitDateBaseFoods();
    }

    //----------------------------------------------------------------------------------------------------------------------------------

    /*=======================================================================
    public void InitDateBaseFoods()
    {
       initFood.catalogFoods.Clear();                                                              // ќчищаем справочник Foods
      
       singleFood = new Food() { Name = "banana",     Value = -0.2f };
       catalogFoods.Add(singleFood);

       singleFood = new Food() { Name = "mushroom",   Value = 0.8f };
       catalogFoods.Add(singleFood);

       singleFood = new Food() { Name = "orange",     Value = -0.2f };
       catalogFoods.Add(singleFood);

       singleFood = new Food() { Name = "lemon",      Value = -0.1f };
       catalogFoods.Add(singleFood);

       singleFood = new Food() { Name = "grape",      Value = -0.3f };
       catalogFoods.Add(singleFood);

       singleFood = new Food() { Name = "kokonut",    Value = -0.4f };
       catalogFoods.Add(singleFood);

       singleFood = new Food() { Name = "pineapple",  Value = -0.3f };
       catalogFoods.Add(singleFood);

       singleFood = new Food() { Name = "meat",       Value = -0.6f };
       catalogFoods.Add(singleFood);

       singleFood = new Food() { Name = "watermelon", Value = -0.2f };
       catalogFoods.Add(singleFood);
     
    }
     ====================================================================================*/
    //-----------------------------------------------------------------------------------------------------------------------------------------------
    
    
    private void InitIDCollection( string item)
    {
        listByTag.Clear();
        listByTag.AddRange(GameObject.FindGameObjectsWithTag( item ));
        foreach (GameObject p in listByTag)
            listIDCollection.Add(p.GetInstanceID());
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------
    public void AddFoodCount(string food)                                               // —делана исключительно в обучающих цел€х. јпдейт полей елемента List
    {                                                                                   // Ёлементы листа - Ёкземпл€ры класса
        foreach (Food f in initFood.catalogFoods)
        {
            if (f.name == food)
            {
                f.count++;
                CountHunger(f.value);                                                       // –ассчитать голод и занести ScoreManager
            }
        }
    }

    public void CountHunger(float value)
    {
        scoreManager.Hunger += value;
              
        if (scoreManager.Hunger >= 1)
        {
            DecreaseLifes();                                                                 // мминус жизнь
            ResetFoodCount();
        }
    }
  
    public void DecreaseLifes()
    {
        soundManager.PlayLoseLife();
        scoreManager.Lifes--;                                                                  // сохран€ем количество жизней в ScoreManagere
        scoreManager.Hunger = 0;                                                               // обнулить голод
        NotifyLife?.Invoke(scoreManager.Lifes);                                                // ¬ызываем событие 
        if (scoreManager.Lifes <= 0)
            NotifyDeath?.Invoke();
    }

    public void ResetFoodCount()
    {
       foreach (Food f in initFood.catalogFoods)
         f.count = 0;
    }
    
    public bool DeleteFromListIDCollection(int idObj)
    {
        listIDCollection.Remove(idObj);
        if (listIDCollection.Count != 0)
            return true;
        else
            return false;
    }

}
