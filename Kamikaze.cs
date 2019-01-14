using UnityEngine;
using System.Collections;

public class Kamikaze : MonoBehaviour {
    public GameObject Detonator;
    public float KamikazeSpeed;
    public GameObject target;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")//プレイヤーに触れた際に自爆する
        {
            Instantiate(Detonator, transform.position, transform.rotation);//爆風を生成
            Destroy(target);//自爆した敵を消す
        }
    }
}
