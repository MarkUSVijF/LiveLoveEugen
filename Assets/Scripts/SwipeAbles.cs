using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SwipeAble
{
    public void SwipeStart(Vector2 position);
    public void OnSwipe(Vector2 position);
    public void SwipeEnd(Vector2 position);
}
