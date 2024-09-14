using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TopicList", menuName = "Scriptable Objects/Topic Table")]
public class ChatTopicsScriptableObject : ScriptableObject
{
    public List<Topic> topics;
}

[Serializable]
public class Topic
{
    public String Name;
    public String Thoughts;
    public String State;
}