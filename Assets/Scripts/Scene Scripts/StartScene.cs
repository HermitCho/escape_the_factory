using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    AudioSource bgmAudio;

    void Awake()
    {
        bgmAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        bgmAudio.loop = true;
        SoundManager.Instance.GetSound(bgmAudio, 0, 0.2f);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("House");
        /*        SaveLoadManager.Instance.LoadInventory();
                SaveLoadManager.Instance.LoadMoney();
                SaveLoadManager.Instance.LoadEquipment();
                SaveLoadManager.Instance.LoadChest();*/
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("���� ����");
    }

}
