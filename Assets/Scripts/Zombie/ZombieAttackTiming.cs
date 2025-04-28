using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackTiming : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    private AudioSource zombieAudio;
    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        zombieAudio = GetComponent<Transform>().root.GetComponent<AudioSource>();

    }
    public void ZAttack()
    {
        StartCoroutine(DisableTemporarily(capsuleCollider));
    }
    private IEnumerator DisableTemporarily(CapsuleCollider box)
    {
        SoundManager.Instance.GetSound(zombieAudio, 15, 0.3f);

        box.enabled = true;
        yield return new WaitForSeconds(0.3f);
        box.enabled = false;
    }
}