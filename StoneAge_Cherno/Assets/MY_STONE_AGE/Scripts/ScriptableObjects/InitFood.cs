using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InitFood", menuName = "ScriptableObjects/NewInitFood")]

public class InitFood : ScriptableObject
{
    public List<Food> catalogFoods = new List<Food>();                               // База данных еды
}
