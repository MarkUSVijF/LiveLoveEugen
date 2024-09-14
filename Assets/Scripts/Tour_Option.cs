using SoftMasking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Tour_Option
{
    public SoftMask outline_mask;
    public SoftMask text_mask;
    public void updateState(float state)
    {
        var pos_state = Mathf.Clamp01(state);
        var neg_state = Mathf.Clamp01(-state);

        outline_mask.channelWeights = new Color(0, 0, 0, pos_state);
        text_mask.channelWeights = new Color(0,0,0,1-neg_state);
    }
}

[System.Serializable]
public struct Tour_Options
{
    [SerializeField]
    public Tour_Option left;
    [SerializeField]
    public Tour_Option right;

    public void updateState(float state)
    {
        right.updateState(state);
        left.updateState(-state);
    }

    public bool isValid()
    {
        return !(left.outline_mask == null || left.text_mask == null || right.outline_mask == null || right.text_mask == null);
    }
}
