using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseManager<MusicMgr>
{
    //背景音组件
    private AudioSource bkMusic = null;
    //背景音大小
    private float bkValue = 1;
    //音效依附对象
    private GameObject soundObj = null;
    //音效列表
    private List<AudioSource> soundList = new List<AudioSource>();
    private float soundValue = 1;


    //移除非播放状态音效
    private void Update()
    {
        for(int i = soundList.Count - 1; i >=0; --i)
        {
            if(!soundList[i].isPlaying)
            {
                
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
            
        }
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBkMusic(string name)
    {
        if(bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BkMusic";
            bkMusic = obj.AddComponent<AudioSource>();
        }
        //异步加载完成后播放
        ResMgr.Instance.LoadAsync<AudioClip>("Music/BK/"+name,(clip)=> 
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkValue;
            bkMusic.Play();
        });
    }
    /// <summary>
    /// 改变背景音效大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeBkValue(float v)
    {
        bkValue = v;
        if (bkMusic == null)
            return;
        bkMusic.volume = bkValue;
    }
    /// <summary>
    /// 暂停音效
    /// </summary>
    public void PauseBkMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    /// <summary>
    /// 停止播放
    /// </summary>
    /// <param name="name"></param>
    public void StopBkMusic(string name)
    {
        if (bkMusic == null)
            return;
        bkMusic.Stop();
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySound(string name,bool isLoop,UnityAction<AudioSource> callBack = null)
    {
        if(soundObj =null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }       
        //音效加载结束后，再添加音效
        ResMgr.Instance.LoadAsync<AudioClip>("Music" + name, (clip) =>
        {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = isLoop;
            source.volume = soundValue;
            source.Play();
            soundList.Add(source);

            if(callBack != null)
                callBack(source);
        });
    }

    /// <summary>
    /// 改变音效大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundValue(float value)
    {
        soundValue = value;
        for (int i = 0; i < soundList.Count; i++)
            soundList[i].volume = value;
    }
    /// <summary>
    /// 停止音效
    /// </summary>
    public void StopSound(AudioSource source)
    {
        if(soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }
    
}
