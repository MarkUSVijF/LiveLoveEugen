using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TinderScene", menuName = "Scriptable Objects/Scene/Tinder")]
public class TinderScene : SceneBase
{
    /*
     * Tinder [Matching]
     * -) Eugen (User Image)
     * -) User Name
     * -) User Age
     * -) Location?
     * -) (Bio/Beruf)
    */

    public Sprite Eugen;
    public string name = "Eugen";
    public int age = 361;
    public string location = "Belvedere Palace, Vienna";
    public string bio = "A True Art Connoisseur.";
}