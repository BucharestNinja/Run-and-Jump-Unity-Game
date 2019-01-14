using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour {


    const int DefaultLife = 3;
    const float StunDuration = 0.5f;
    const int MaxAttackPower = 1;

    CharacterController controller;
    Animator animator;
    Vector3 moveDirection = Vector3.zero;
    Collision collision;
    int life =  DefaultLife;
    float count = 0;
    float attackTime = 0;
    float recoverTime = 0.0f;
    float PlusSpeedZ;
    bool isHit = false;
    
    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJumpY;
    public float acclerationZ;
    public GameObject target;
    private AudioSource sound02;
    private AudioSource sound03;
    private AudioSource sound04;
    private AudioSource sound05;
    public float attackRecoveryTime;
    public float SwordKillSpeed;
    public float GunKillSpeed;
    public Text LifeText;
    int AttackPower = MaxAttackPower;
    PlayerHit playerHit;
    GameObject go;
    Target targetgo;
    Clear c;
    public GameObject refObj;

    public int Life()
    {
        return life;
    }

    public bool IsStan()
    {
        return recoverTime > 0.0f || life <= 0;
    }

	void Start ()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        GameObject go = GameObject.Find("Sword");
        playerHit = go.GetComponent<PlayerHit>();
        targetgo = GetComponent<Target>();
        sound02 = audioSources[0];
        sound03 = audioSources[1];
        sound04 = audioSources[2];
        sound05 = audioSources[3];
        c = refObj.GetComponent<Clear>();
        sound05.Play();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")  && controller.isGrounded) Jump();
        if (Input.GetButtonDown("Fire1")) Attack();
        LifeText.text = "Life : " + life.ToString();//残り体力を表示
        if (IsStan())
        {
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;//速度を一時的に0に
            recoverTime -= Time.deltaTime;
            if(life <= 0)
            {
                Clear.time = 0;//タイムをリセット
                GameOver();

            }
        }
        else
        {
            if (controller.isGrounded)
            {
                PlusSpeedZ = speedZ + (SwordKillSpeed * playerHit.SwordEnemyKill)+(GunKillSpeed*targetgo.GunEnemyKill);//敵を斬った回数と撃った回数に応じて加速

                float acceleratedZ = moveDirection.z + (acclerationZ * Time.deltaTime);
                moveDirection.z = Mathf.Clamp(acceleratedZ, 0, PlusSpeedZ) ;//加速処理
                moveDirection.x = Input.GetAxis("Horizontal")*speedX;//水平移動
            }
           
        }
        

        moveDirection.y -= gravity * Time.deltaTime;//重力処理

        Vector3 globalDirection = transform.TransformDirection(moveDirection);//移動処理
        controller.Move(globalDirection * Time.deltaTime);

        if (controller.isGrounded) moveDirection.y = 0;//接地している場合に垂直方向の速度をリセット

        if (moveDirection.z > 0) animator.SetBool("run", true);

        if (c.clear == true)
        {
            sound05.Stop();//プレイ中BGMを停止
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Clear.time = 0;
            GameOver();//リスタート処理
        }

    }

        public void Attack()
    {
        if (IsStan()) return;
        if (AttackPower <= 0) return;//斬撃にインターバルを設定
        else
        {
            animator.SetTrigger("attack");
            sound02.PlayOneShot(sound02.clip);
            consumeAttackPower();
        }
    }

    void consumeAttackPower()
    {
        AttackPower--;
        StartCoroutine(recoverAttackPower());//パワーが回復することで再び剣を振れるようになる
    }

    IEnumerator recoverAttackPower()
    {
        yield return new WaitForSeconds(attackRecoveryTime);
        AttackPower++;
    }

        public void Jump()
    {
        if (IsStan()) return;
        moveDirection.y = speedJumpY;;
    }

    IEnumerator invincibleTime(float count)//被弾時の無敵時間処理
    {
        isHit = true;
        yield return new WaitForSeconds(count);
        isHit = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)//自機の食らい判定
    {
        if (IsStan()) return;
        if (hit.gameObject.tag == "enemy" && isHit == false)
        {
            life--;//ライフが減少
            recoverTime = StunDuration;
            animator.SetTrigger("damage");
            sound03.PlayOneShot(sound03.clip);
            Destroy(hit.gameObject);//ぶつかった敵を破壊
            StartCoroutine("invincibleTime", 1.5f);
        }
    }

    void OnTriggerEnter(Collider other)//自機の爆風に対する食らい判定
    {
        if (IsStan()) return;
        if (other.gameObject.tag == "detonator"&& isHit == false)
        {
            life--;
            recoverTime = StunDuration;
            animator.SetTrigger("damage");
            sound03.PlayOneShot(sound03.clip);
            StartCoroutine("invincibleTime", 1.5f);
        }
    }
    void GameOver()
    {
        SceneManager.LoadScene("Main");
    }
}