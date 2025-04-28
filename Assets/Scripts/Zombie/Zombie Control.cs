using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControl : MonoBehaviour
{
    private AudioSource zpmbieConaudio;

    ZombieAttackTiming zombieAttackTiming;

    // Start is called before the first frame update
    void Start()
    {
        zpmbieConaudio = GetComponent<AudioSource>();
        StartCoroutine(ZombieSound()); // 문자열 대신 메서드 참조로 코루틴 실행
        zombieAttackTiming = GetComponentInChildren<ZombieAttackTiming>();
    }

    // Update is called once per frame
    void Update()
    {
/*        if (zombieMove.followTime <= 0f)
        {
            audioSource.Stop();
        }
        else if(zombieMove.followTime > 0f)
        {
            audioSource.Play();
        }*/
    }

    public IEnumerator ZombieSound()
    {
        while (true)
        {
            SoundManager.Instance.GetSound(zpmbieConaudio,11,0.1f);
            yield return new WaitForSeconds(5);
        }
    }


    public void AttackFunction()  // Animation Event에서 호출되는 함수
    {
        if (zombieAttackTiming != null)
        {
            zombieAttackTiming.ZAttack();  // 다른 오브젝트의 함수 호출


        }
    }
}
