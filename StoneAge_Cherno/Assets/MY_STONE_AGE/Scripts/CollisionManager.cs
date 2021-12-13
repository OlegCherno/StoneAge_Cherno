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
    [SerializeField] InitFood initFood;                                                            // ������ �� ������� ��� � ScriptableObject

    DataBaseManager dataBaseManager;
    
    private Collider2D circleColliderPlayer,
                       capsuleColliderPlayer;
   private GameObject go;
    private List<string> listNameFood = new List<string>();                                         // ���� ���� ������� 

    private void Awake()
    {
        dataBaseManager = FindObjectOfType(typeof(DataBaseManager)) as DataBaseManager;
                
        goAttack.GetComponent<Attack>().NotifyCollision += Collisions;                            // �������� �� �������(���� ������ �� ���������)
        goPlayer.GetComponent<MyPlayerMovement>().NotifyPlayerCollision += Collisions;             // �������� �� ������������ � �������
       
        circleColliderPlayer  = goPlayer.transform.GetComponent<CircleCollider2D>();
        capsuleColliderPlayer = goPlayer.transform.GetComponent<CapsuleCollider2D>();
             
        InitListNameFood();
    }

    public void Collisions(GameObject go, Vector2 collisionPoint, GameObject primaryGO)   // ������� ����������� ����
    {
        this.go = go;
        string name = go.name.ToLower();
        string tagGO = go.tag;
        int idGo = go.GetInstanceID();

       
       // print("from CollisionManagerPrimaryGO   " + primaryGO.name + "  go " + go.name);

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ���� �������� � WEAPONS      >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (primaryGO.name == "Weapons")
        {
            //----------------------------------------------------------------------------------------------------
            if (dataBaseManager.listIDCollection.Contains(idGo))                           // ���� ������ � ������ ������������� �����
            {                                                                              // ������� �� ������ ���� ������ �� ���������
                if (!dataBaseManager.DeleteFromListIDCollection(idGo))
                {
                    LevelController.instance.isEndGame();                                  // ������ ������. ����� ������2. �������� �����4
                    print("<<<<<<<<<<<  END LEVEL  >>>>>>>>>>>>");
                }
            }
           
            //-------------------------------------------------------------------------------------------------------------
            if (name.Contains("stone"))                                              // ������  Stone     
            {
                GameObject particle = Instantiate(shardsParticlePrefab, collisionPoint, shardsParticlePrefab.transform.rotation);
                soundManager.PlayStone();
                Destroy(particle, 1f);
            }
            //----------------------------------------------------------------------------------------------------
            else if (name.Contains("leave"))                                             // ����� ������ Leaves
            {
                soundManager.PlayAxeTree();
                animTree.SetTrigger("Swing");
            }
            //----------------------------------------------------------------------------------------------------
            else if (name.Contains("tree"))                                               // ������ TreeStump
            {
                soundManager.PlayAxeTree();
            }
            //----------------------------------------------------------------------------------------------------
            else if ( listNameFood.Contains(tagGO))                                      // Banana Orange Mushroom Pineaaple Kokonut Lemon Grape Cherrie Finish
            {
                CollectFood( tagGO );
                if(tagGO == "Finish")                                                    // ���� ���. ������� ���������� ���������� �� ����� �������
                    scoreManager.isFinish = true;
            }
            //------------------------------------------------------------------------------------------------------
            else if (name.Contains("palm"))                                                   // ������ � ������� Palm2
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
               
                animPalm.SetTrigger("Swing");                                              // �������� ����� ���������� ��� ���� �����
            }
            //------------------------------------------------------------------------------------------------------
            else if (tagGO == "bone")                                  // ����� � ������
            {
                go.SetActive(false);
            }
            //------------------------------------------------------------------------------------------------------
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>   ���� �������� � ������� >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        else if (primaryGO.name == "Player")
        {
            //----------------------------------------------------------------------------------------------------------------
            if (name.Contains("precipice"))                                                // �����
            {
                ColledersPlaerManager(false);
                animator.SetFloat("Speed", 0f);
                StartCoroutine(WaitEnd());
            }
            //----------------------------------------------------------------------------------------------------------------
            else if (name.Contains("water"))                                                 // Water 
            {
                if (scoreManager.isFinish)                                                   // ���� ������ �������� �� ���� �������
                {
                    scoreManager.isFinish = false;
                    ColledersPlaerManager(false);                                            // ��� ���������� � water ����������� ������
                    LevelController.instance.isEndGame();                                    // � LevelControllere ��������� �� ��� ������� � ��������� ���
                    print("<<<<<<<<<<<  END LEVEL  >>>>>>>>>>>>");                           // ����� ������1. �������� �����2
                }
                else 
                {
                    ColledersPlaerManager(false);
                    animator.SetFloat("Speed", 0f);
                    StartCoroutine(WaitEnd());
                }
            }
            //-------------------------------------------------------------------------------------------------------------
            /*else if (go.CompareTag("Stun"))                                                // ����� �� ������
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

    private void ColledersPlaerManager(bool isEnabled)                                        // On/Off collides � ������
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
