using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {

    public  bool StartFlag = false;

    public void StartClick()
    {
       StartFlag = true;
        gameObject.SetActive(false);
    }
}
