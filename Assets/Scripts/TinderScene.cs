using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using static SceneDescribtion;

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

    [ConditionalField(nameof(is_animated), inverse:true)] public Sprite Eugen;
    [ConditionalField(nameof(is_animated))] public AnimatorController AnimatedEugen;
    public bool is_animated=false;
    public string user = "Prince Eugen";
    public int age = 361;
    public string location = "Belvedere Palace, Vienna";
    public string bio = "A True Art Connoisseur.";
}