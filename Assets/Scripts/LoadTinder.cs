using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadTinder : MonoBehaviour
{
    [SerializeField]
    public Image image;
    public TextMeshProUGUI UserName;
    public TextMeshProUGUI UserAge;
    public TextMeshProUGUI Location;
    public TextMeshProUGUI Bio;
    /*
     * Tinder [Matching]
     * -) Eugen (User Image)
     * -) User Name
     * -) User Age
     * -) Location?
     * -) (Bio/Beruf)
    */
    public void Load(TinderScene scenedata)
    {
        UserName.text = scenedata.user;
        UserAge.text = scenedata.age.ToString();
        Location.text = scenedata.location;
        Bio.text = scenedata.bio;
        if (scenedata.is_animated)
        {
            image.GetComponent<Animator>().runtimeAnimatorController = scenedata.AnimatedEugen;
        }
        else
        {
            //image.sprite = scenedata.Eugen;
            image.overrideSprite = scenedata.Eugen;
        }
    }
}
