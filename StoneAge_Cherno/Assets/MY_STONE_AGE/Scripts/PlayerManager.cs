using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    DataBaseManager dataBaseManager;
    [SerializeField] private Animator animator;
   
    void Start()
    {
        dataBaseManager = FindObjectOfType(typeof(DataBaseManager)) as DataBaseManager;
        dataBaseManager.NotifyDeath += ControlDeath;                                             // вызываем событие
        scoreManager.startPlayerPosition = transform.position;                                   // запомнить начальную позицию игрока и занести в ScoreManager 
    }

    void ControlDeath()
    {
        animator.SetTrigger("Death");
        print("<<<  GAME OVER  >>>");
    }
}
