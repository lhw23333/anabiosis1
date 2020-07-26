using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Door : MonoBehaviour
{
    public GameObject nextDoor;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && TpManager.Instance.GetTpState())
        {
            TpManager.Instance.StartTpTimer();
            //Tp(collision.gameObject);
            //collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);//问题 角色仍然被控制               预设解决方案 销毁 重新创建
            //collision.gameObject.SetActive(false); //问题 冲刺时设置的重力不能正常恢复

            Destroy(collision.gameObject);
            GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = nextDoor.transform;
            StartCoroutine(TpReady(0.8f));
        }
    }
    void Tp()
    {
        // player.active = true;
        // player.GetComponent<SpriteRenderer>().color = new Color(0,0,0, 1);
        //player.SetActive(true);


        //player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //player.transform.position = nextDoor.transform.position;

        
        GameObject player = ResMgr.Instance.Load<GameObject>("Player");
        player.transform.position = nextDoor.transform.position;
        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
    }
    
    IEnumerator TpReady(float timer)
    {
        while(timer>0)
        {
            timer -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        Tp();
    }
}
