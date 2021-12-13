using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectWeapon : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject buttonAttack;
    SpriteRenderer sr;
    AtlasLoader atlasLoader;

    void Start()
    {
        atlasLoader = new AtlasLoader("Atlas_H_M_W_0000");                                     // Class [System.Serializable]
        sr = player.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void SelectSlot(string slot)
    {
        sr.sprite = atlasLoader.getAtlas(slot);
        buttonAttack.transform.GetComponent<Image>().sprite = sr.sprite;
        buttonAttack.transform.GetComponent<Image>().SetNativeSize();
    }
}