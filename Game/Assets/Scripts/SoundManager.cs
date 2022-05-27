using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource soundSource;
    public AudioClip[] slashSources;
    public AudioClip[] enemyHitSources;
    public AudioClip levelUpSound;
    [SerializeField] GameObject slashSound;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayerAttackSound()
    {
        soundSource.clip = slashSources[Random.Range(0, slashSources.Length-1)];
        soundSource.PlayOneShot(soundSource.clip);
    }
    public void EnemyHitSound()
    {
        soundSource.clip = enemyHitSources[Random.Range(0, enemyHitSources.Length - 1)];

        soundSource.PlayOneShot(soundSource.clip);
    }
    public void PlayLevelUpSound()
    {
        soundSource.PlayOneShot(levelUpSound);
    }
}
