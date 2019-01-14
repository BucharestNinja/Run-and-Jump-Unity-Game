using UnityEngine;
using System.Collections;

public class KamikazeEnemy : MonoBehaviour
{
    bool EnemyRange = false;
    UnityEngine.AI.NavMeshAgent agent;
    Transform target;
    // Use this for initialization
    void Start () {
        target = GameObject.Find("unitychan").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (EnemyRange == true)//自機が射程内に入った場合
        {
            agent.destination = target.transform.position;//カミカゼ（敵）が自機を追いかける
        }
    }

    void OnTriggerEnter(Collider collider)//自機が敵の射程内に入ったかどうかの処理
    {
        if (collider.gameObject.tag == "Player")
        {
            EnemyRange = true;
        }
    }
}
