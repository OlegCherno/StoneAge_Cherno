using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManager;
    DataBaseManager dataBaseManager;

    [Header("Шлкала Голода")]
    public Image imageScale;

    [Header("Шлкала Жизней")]
    public GameObject imageLifes;

    void Start()
    {
        dataBaseManager = FindObjectOfType(typeof(DataBaseManager)) as DataBaseManager;
        dataBaseManager.NotifyLife   += ControlLife;
        scoreManager.Hunger = 0;
        ControlLife(scoreManager.Lifes);
    }

    private void FixedUpdate()
    {
        scoreManager.Hunger += Time.deltaTime * scoreManager.hungerSpeed;
        imageScale.fillAmount = scoreManager.Hunger;
        if (scoreManager.Hunger >= 1f)
            dataBaseManager.DecreaseLifes();
    }
  

    private void ControlLife(int count)
    {
       for (int i=count; i < 3; i++)
            imageLifes.transform.GetChild(i).GetComponent<Image>().color = Color.cyan;
    }
}
