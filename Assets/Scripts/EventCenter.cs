using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenter : BaseManager<EventCenter>
{
    // Start is called before the first frame update
    //key 事件名
    //value 监听此事件所对应的委托
    private Dictionary<string, UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();
    
    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="action">对应委托</param>
    public void AddEventListener(string name,UnityAction<object> action)
    {
        if(eventDic.ContainsKey(name))
        {
            eventDic[name] += action;
        }
        else
        {
            eventDic.Add(name, action);
        }
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name,UnityAction<object> action)
    {
        if (eventDic.ContainsKey(name))
            eventDic[name] -= action;
    }

    /// <summary>
    /// 事件触发,执行委托
    /// </summary>
    /// <param name="name">事件名</param>
    public void EventTrigger(string name,object info)
    {
        if(eventDic.ContainsKey(name))
        {
            eventDic[name].Invoke(info);
        }
    }
}
