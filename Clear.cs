using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Clear : MonoBehaviour {
    public bool clear = false;
    public float score;
    public static float time = 0;
    public Text TimeText;
    public Text RankText;
    public Text HighScoreText;
    private AudioSource sound01;
    public GameObject Barrier;
    public Transform BarrierPos;
    public Text PressRText;
    public Text PressTText;
    public Text TitleText;
    public Text PressSText;
    public Text TitleHighScoreText;
    
    StartButton startbutton;
	// Use this for initialization

	void Start () {
        GameObject a = GameObject.Find("StartButton");
        startbutton = a.GetComponent<StartButton>();
        sound01 = GetComponent < AudioSource >();
	}
	
	// Update is called once per frame
	void Update () {
        if(clear == false)
        {
            time += Time.deltaTime;//タイム計測
        }
        if (clear == true)
        {
            enabled = false;
            sound01.Play();//クリアBGM再生
            PressRText.text = "Play Again : Press R";//Rでリスタート
     
            if (time < 65)//クリアタイムごとにランクを表示
            {
                RankText.text = "Rank:GODLIKE";
            }
            else if (time < 70&&time>=65)
            {
                RankText.text = "Rank:SSS";
            }
            else if (time < 75&&time>=70)
            {
                RankText.text = "Rank:SS";
            }
            else if (time < 80&&time>=75)
            {
                RankText.text = "Rank:S";
            }
            else if (time < 90 && time >= 80)
            {
                RankText.text = "Rank:A";
            }
            else if (time < 100 && time >= 90)
            {
                RankText.text = "Rank:B";
            }
            else if (time < 110 && time >= 100)
            {
                RankText.text = "Rank:C";
            }
            else RankText.text = "Rank:D";

            if (PlayerPrefs.GetFloat("HighScore",1000000) > time)
            {
                PlayerPrefs.SetFloat(("HighScore"), time);
                HighScoreText.text = "HighScore";//クリアタイムがハイスコアだった場合クリア画面に表示
            }
            Invoke("Result", 180);//クリア後プレイヤーからの操作がない場合自動でスタート画面へ
            Instantiate(Barrier, BarrierPos.position, transform.rotation);//クリア後に爆風で死亡しないように壁を生成
        }

        

        if (startbutton.StartFlag == false)
        {
            Time.timeScale = 0;//Sを押すまで時間を止める
            TitleText.text = "DON'T STOP,JUST KILL";
            
            TitleHighScoreText.text = "High Score : " + PlayerPrefs.GetFloat("HighScore");

        }
        else if(startbutton.StartFlag == true)
        {
            Time.timeScale = 1;//Sが押されたら時間を元に戻しゲーム開始
            TimeText.text = "Time:" + time.ToString();//タイムを表示
            TitleText.text = "";//タイトル等を非表示
            TitleHighScoreText.text = "";
        }


    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player") clear = true;//ゴールゲートに接触するとクリアフラグが立つ
    }

    void Result()
    {
        time = 0;
        SceneManager.LoadScene("Main");
    }

    public static float GetTime()
    {
        return time;
    }
}
