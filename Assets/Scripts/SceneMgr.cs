using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : BaseManager<SceneMgr>
{
    /// <summary>
    /// 切换场景 同步
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadScene(string name, UnityAction fun)
    {
        SceneManager.LoadScene(name);
        fun();
    }

    /// <summary> 
    /// 协程异步加载场景  查看加载进度
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsyn(string name,UnityAction fun)
    {
        SceneMgr.Instance.StartCoroutine(ReallyLoadSceneAsyn(name ,fun));
    }

    private IEnumerator ReallyLoadSceneAsyn(string name,UnityAction fun)
    {
        //协程加载 此处相当于协程嵌套
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //场景加载进度
        while(!ao.isDone)
        {
            //更新进度条 向外分发
            //UIManager结合实现可视化加载
            EventCenter.Instance.EventTrigger("Loading",ao.progress);
            yield return ao.progress;
        }
        //加载完成过后 执行加载操作
        fun();
    }
}
