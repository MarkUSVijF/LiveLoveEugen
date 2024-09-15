using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadTour : MonoBehaviour
{
    [SerializeField]
    public Image Background;
    [SerializeField]
    public Image Eugen;
    [SerializeField]
    public TextMeshProUGUI[] Text = { };
    /*
     * Tour
     * -) Background
     * -) Eugen
     * -) is choice
     * -) Text (Left/Right)
    */
    public bool Load(TourScene scenedata, int n)
    {
        Background.overrideSprite = scenedata.Background;
        if (n >= scenedata.dialog.Length)
        {
            if (!scenedata.has_end_choice)
            {
                return false;
            }
            var curr = scenedata.end_choice;
            Eugen.overrideSprite = curr.Emotion;
            Eugen.transform.OffsetXY(curr.options.offset);
            Text[0].text = curr.choice_left;
            Text[1].text = curr.choice_right;
        }
        else
        {
            var curr = scenedata.dialog[n];
            Eugen.overrideSprite = curr.Emotion;
            Eugen.transform.OffsetXY(curr.options.offset);
            Text[0].text = curr.dialoge;
        }
        return true;
    }
}
