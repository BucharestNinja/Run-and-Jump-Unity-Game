using UnityEngine;
using System.Collections;

public class MineAttack : MonoBehaviour {
    public GameObject Detonator;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)//自機が地雷を踏んだら爆風を生成
    {
        if(collider.gameObject.tag== "Player") Instantiate(Detonator, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
