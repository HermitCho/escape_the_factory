using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : Singleton<PauseManager>
{
    private AudioSource mAudioSource;
    private bool activeAuthority = false;
    [HideInInspector] public bool isPaused = false;
    [Header("Set Child Object")]
    [SerializeField] private GameObject pauseUI;
    // Start is called before the first frame update
    /*    void Start()
        {

            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                Debug.Log("current Scene: MainScene");
                activeAuthority = false;
            }
            else { activeAuthority = true; Debug.Log("current Scene: " + SceneManager.GetActiveScene().name); }
        }
    */

    private void OnEnable()
    {
        // �� �ε� �̺�Ʈ�� �޼��� ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // �� �ε� �̺�Ʈ���� �޼��� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �� �ε� �� ȣ��Ǵ� �޼���
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mAudioSource = GetComponent<AudioSource>();
        if (scene.name == "MainScene")
        {
            Debug.Log("current Scene: MainScene");
            activeAuthority = false;
        }
        else
        {
           
            Debug.Log("current Scene: " + scene.name);
            activeAuthority = true;
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (activeAuthority)
        {
            if (InventoryMain.IsInventoryActive == false && Input.GetKeyDown(KeyCode.Escape))
            {
                //Ŀ�� Ȱ��ȭ
                UnityEngine.Cursor.lockState = CursorLockMode.Confined;
                UnityEngine.Cursor.visible = true;

                SoundManager.Instance.GetSound(mAudioSource, 7, 0.3f, 0.5f, 1f);
                pauseUI.SetActive(true);
                isPaused = true;
            }
        }

        else if (isPaused && Input.GetKeyDown(KeyCode.Escape)) 
        {
            //Ŀ�� ��Ȱ��ȭ
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;

            SoundManager.Instance.GetSound(mAudioSource, 7, 0.3f, 0.5f, 1f);
            pauseUI.SetActive(false);
            isPaused = false;
        }

    }


    public void ReturnToLobby()
    {
        if(SceneManager.GetActiveScene().name == "SampleScene")
        {
            isPaused = false;
            pauseUI.SetActive(false);
            SoundManager.Instance.GetSound(mAudioSource, 2, 3f);
            SceneManager.LoadScene("MainScene");

        }
        else 
        {
            isPaused = false;
            pauseUI.SetActive(false);
            SoundManager.Instance.GetSound(mAudioSource, 2, 3f);
            SaveLoadManager.Instance.SaveInventory();
            SaveLoadManager.Instance.SaveMoney();
            SaveLoadManager.Instance.SaveEquipment();
            SceneManager.LoadScene("MainScene");
        }
    }

    public void ContinueGame()
    {
        //Ŀ�� ��Ȱ��ȭ
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        SoundManager.Instance.GetSound(mAudioSource, 2, 3f);
        pauseUI.SetActive(false);
       isPaused = false;

    }

}
