using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] private GameObject shardsParticlePrefab;
    [SerializeField] private GameObject goAttack;
    [SerializeField] private GameObject goPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animTree, 
                                      animPalm;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] SoundManager soundManager;
    [SerializeField] InitFood initFood;                                                            // Ссылка на каталог еды в ScriptableObject

    DataBaseManager dataBaseManager;
    
    private Collider2D circleColliderPlayer,
                       capsuleColliderPlayer;
   private GameObject go;
    private List<string> listNameFood = new List<string>();                                         // Лист имен фруктов 

    private void Awake()
    {
        dataBaseManager = FindObjectOfType(typeof(DataBaseManager)) as DataBaseManager;
                
        goAttack.GetComponent<Attack>().NotifyCollision += Collisions;                            // Подписка на событие(удар палкой по предметам)
        goPlayer.GetComponent<MyPlayerMovement>().NotifyPlayerCollision += Collisions;             // Подписка на столкновения с Игроком
       
        circleColliderPlayer  = goPlayer.transform.GetComponent<CircleCollider2D>();
        capsuleColliderPlayer = goPlayer.transform.GetComponent<CapsuleCollider2D>();
             
        InitListNameFood();
    }

    public void Collisions(GameObject go, Vector2 collisionPoint, GameObject primaryGO)   // Требует оптимизации кода
    {
        this.go = go;
        string name = go.name.ToLower();
        string tagGO = go.tag;
        int idGo = go.GetInstanceID();

       
       // print("from CollisionManagerPrimaryGO   " + primaryGO.name + "  go " + go.name);

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Блок КОЛЛИЗИЙ с WEAPONS      >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (primaryGO.name == "Weapons")
        {
            //----------------------------------------------------------------------------------------------------
            if (dataBaseManager.listIDCollection.Contains(idGo))                           // если объект в списке обязательного сбора
            {                                                                              // удаляем из списка пока список не обнулится
                if (!dataBaseManager.DeleteFromListIDCollection(idGo))
                {
                    LevelController.instance.isEndGame();                                  // Список пустой. Конец уровня2. Загрузка Сцены4
                    print("<<<<<<<<<<<  END LEVEL  >>>>>>>>>>>>");
                }
            }
           
            //-------------------------------------------------------------------------------------------------------------
            if (name.Contains("stone"))                                              // Камень  Stone     
            {
                GameObject particle = Instantiate(shardsParticlePrefab, collisionPoint, shardsParticlePrefab.transform.rotation);
                soundManager.PlayStone();
                Destroy(particle, 1f);
            }
            //----------------------------------------------------------------------------------------------------
            else if (name.Contains("leave"))                                             // Крона дерева Leaves
            {
                soundManager.PlayAxeTree();
                animTree.SetTrigger("Swing");
            }
            //----------------------------------------------------------------------------------------------------
            else if (name.Contains("tree"))                                               // Бревно TreeStump
            {
                soundManager.PlayAxeTree();
            }
            //----------------------------------------------------------------------------------------------------
            else if ( listNameFood.Contains(tagGO))                                      // Banana Orange Mushroom Pineaaple Kokonut Lemon Grape Cherrie Finish
            {
                CollectFood( tagGO );
                if(tagGO == "Finish")                                                    // пока так. требует пересмотра реализации на более изящное
                    scoreManager.isFinish = true;
            }
            //------------------------------------------------------------------------------------------------------
            else if (name.Contains("palm"))                                                   // пальма с кокосом Palm2
            {
                soundManager.PlayAxeTree();
                foreach (Transform child in go.GetComponentsInChildren<Transform>())
                {
                    if (child.name.ToLower().Contains("kokonut"))
                    {
                        child.GetComponent<CircleCollider2D>().enabled = true;
                        child.GetComponent<Rigidbody2D>().gravityScale = 1;
                        go.GetComponent<Collider2D>().enabled = false;
                    }
                }
               
                animPalm.SetTrigger("Swing");                                              // Тестовый вызов Доработать для всех пальм
            }
            //------------------------------------------------------------------------------------------------------
            else if (tagGO == "bone")                                  // Кости и кокосы
            {
                go.SetActive(false);
            }
            //------------------------------------------------------------------------------------------------------
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>   Блок КОЛИИЗИЙ с ИГРОКОМ >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        else if (primaryGO.name == "Player")
        {
            //----------------------------------------------------------------------------------------------------------------
            if (name.Contains("precipice"))                                                // Обрыв
            {
                ColledersPlaerManager(false);
                animator.SetFloat("Speed", 0f);
                StartCoroutine(WaitEnd());
            }
            //----------------------------------------------------------------------------------------------------------------
            else if (name.Contains("water"))                                                 // Water 
            {
                if (scoreManager.isFinish)                                                   // Если съеден спецгриб то след уровень
                {
                    scoreManager.isFinish = false;
                    ColledersPlaerManager(false);                                            // два коллайдера и water срабатывает дважды
                    LevelController.instance.isEndGame();                                    // в LevelControllere переходим на сле уровень и открываем тек
                    print("<<<<<<<<<<<  END LEVEL  >>>>>>>>>>>>");                           // конец уровня1. Загрузка Сцены2
                }
                else 
                {
                    ColledersPlaerManager(false);
                    animator.SetFloat("Speed", 0f);
                    StartCoroutine(WaitEnd());
                }
            }
            //-------------------------------------------------------------------------------------------------------------
            /*else if (go.CompareTag("Stun"))                                                // Искры из головы
            {
                animator.SetTrigger("Stun");
            }*/

            IEnumerator WaitEnd()
            {
                yield return new WaitForSeconds(0.8f);
                primaryGO.transform.position = scoreManager.startPlayerPosition;
                dataBaseManager.DecreaseLifes();
                ColledersPlaerManager(true);
            }
                       
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    }

    private void ColledersPlaerManager(bool isEnabled)                                        // On/Off collides у Игрока
    {
        circleColliderPlayer.enabled = capsuleColliderPlayer.enabled = isEnabled;
    }
  
    void CollectFood(string food)
    {
        dataBaseManager.AddFoodCount(food);
        soundManager.PlayCollect();
        go.SetActive(false);
    }

    private void InitListNameFood()
    {
        foreach (Food f in initFood.catalogFoods)
            listNameFood.Add(f.name);
    }
}
