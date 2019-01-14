using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{

    public GameObject bullet;
    public Transform muzzle;
    public float bulletPower;
    public Texture2D cursor;
    public float recoverySeconds;
    public GameObject HitEffect;
    Animator animator;
    CharacterController controller;
    const int maxShotPower = 1;
    int shotPower = maxShotPower;
    public int GunEnemyKill=0;
    private AudioClip Sound01;


    // Use this for initialization
    void Start()
    {
        Cursor.SetCursor(cursor,new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);//カーソルをサイトに変更
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            shot();
        }


    }

    void shot()
    {
        if (shotPower <= 0) return;//射撃にインターバルを設定
        animator.SetTrigger("shot");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.0f;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, bulletPower, LayerMask.GetMask("enemy")))//クリックした先に敵がいるかどうかを判定
        {
            Destroy(hit.collider.gameObject);//敵を消す
            GunEnemyKill++;//射撃で敵を倒した数をカウント
            Instantiate(HitEffect, muzzle.position, muzzle.rotation);
        }

        consumeShotPower();

    }

    void consumeShotPower()
    {
        shotPower--;
        Instantiate(bullet, muzzle.position, muzzle.rotation);
        StartCoroutine(RecoverShotPower());//パワーが回復することで再び銃を撃てるようになる
    }

    IEnumerator RecoverShotPower()
    {
        yield return new WaitForSeconds(recoverySeconds);
        shotPower++;
    }
}
