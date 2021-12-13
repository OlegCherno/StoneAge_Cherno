using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] SoundManager soundManager;
    [SerializeField] InitFood initFood;                                           // ������ �� ������� ��� � ScriptableObject

    /*==============================================================
    public class Food
    {
        public string Name  { get; set; }
        public float  Value { get; set; }
        public int    Count { get; set; }
    }

    public List<Food> catalogFoods = new List<Food>();                               // ���� ������ ���
    private Food singleFood = new Food();                                            // ��������� ���
    ==================================================================*/

    private List<GameObject> listByTag = new List<GameObject>();                    // ���� �������� ������� ���������� ��� ������� � ������ ���� ������� ����
    public List<int> listIDCollection = new List<int>();                            // ���� ID �������������� ��������(���������� ������)

    public event UnityAction<int> NotifyLife;                                        // �������� �������  (���������� ���-�� ������)
    public event UnityAction NotifyDeath;                                            // ��������� ������� (����)
       
    void Start()
    {
        if (scoreManager.NumberScene == 2)
        {
            InitIDCollection("kokonut");                                                 // ��������� ������ ID �������� ��� �����
            InitIDCollection("bone");                                                    // ���o�� � �����
        }
        if (scoreManager.NumberScene == 0)                                      
            ResetFoodCount();
                
       // InitDateBaseFoods();
    }

    //----------------------------------------------------------------------------------------------------------------------------------

    /*=======================================================================
    public void InitDateBaseFoods()
    {
       initFood.catalogFoods.Clear();                                                              // ������� ���������� Foods
      
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
    public void AddFoodCount(string food)                                               // ������� ������������� � ��������� �����. ������ ����� �������� List
    {                                                                                   // �������� ����� - ���������� ������
        foreach (Food f in initFood.catalogFoods)
        {
            if (f.name == food)
            {
                f.count++;
                CountHunger(f.value);                                                       // ���������� ����� � ������� ScoreManager
            }
        }
    }

    public void CountHunger(float value)
    {
        scoreManager.Hunger += value;
              
        if (scoreManager.Hunger >= 1)
        {
            DecreaseLifes();                                                                 // ������ �����
            ResetFoodCount();
        }
    }
  
    public void DecreaseLifes()
    {
        soundManager.PlayLoseLife();
        scoreManager.Lifes--;                                                                  // ��������� ���������� ������ � ScoreManagere
        scoreManager.Hunger = 0;                                                               // �������� �����
        NotifyLife?.Invoke(scoreManager.Lifes);                                                // �������� ������� 
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
