using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnInfo
{
    public string Message;
    public string[] Params;
    public Animator[] Animations;

    public UnityAction Action;

    public void ShowMessageWindow(MessageWindow messageWindow)
    {
        messageWindow.Params = Params;
        messageWindow.Effects = Animations;
        messageWindow.StartMessage(Message);
    }
}