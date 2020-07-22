using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载模块
/// 异步加载
/// 委托和lambda表达式
/// 协程
/// 泛型
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    //同步加载 
    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        //实例化GameObject
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else//音频文字资源无需实例化
            return res;       
    }
    //异步加载
    //异步加载
    public void LoadAsync<T>(string name,UnityAction<T> callback) where T:Object
    {
        ResMgr.Instance.StartCoroutine(ReallyLoadAsync(name,callback));
    }

    private IEnumerator ReallyLoadAsync<T>(string name,UnityAction<T> callback) where T:Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);//T类型被限定为Object，所以此处默认类型为Object
        yield return r;

        if (r.asset is GameObject)//此处需要转化为子类型
            callback(GameObject.Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
    }
    

}
