using UnityEngine;
using System.Collections;

public class MuzzleRotation :MonoBehaviour
{
    Transform target;
	// Use this for initialization
	void Start () {
        target = GameObject.Find("unitychan").transform;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 vec = target.position - transform.position;
        vec = vec.normalized;
        transform.LookAt(target);//敵の銃口を自機に向ける
    }
}
