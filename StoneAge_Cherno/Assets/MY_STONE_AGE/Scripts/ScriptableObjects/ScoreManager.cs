using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreManager", menuName = "ScriptableObjects/NewScoreManager")]
public class ScoreManager : ScriptableObject
{
    [SerializeField] private float hunger;
    [SerializeField] private int countLifes;
    public float hungerSpeed;
    private int numberScene;
    public Vector3 startPlayerPosition;
    public bool isFinish;
        
    public int NumberScene
    {        
       get { return numberScene; }
       set { numberScene =  value; }
    }

    public int Lifes 
    { 
        get { return countLifes; }
        set { countLifes = value; }
    }
    public float Hunger
    {
        get
        {
            if (hunger >= 1f)
                return 1f;
            else
                return hunger;
        }
        set
        {
            if (value <= 0)
                hunger = 0;
            else
                hunger = value;
        }
    }

    private void OnEnable()
    {
        isFinish = false;
        countLifes  = 3;
    }
          
}
