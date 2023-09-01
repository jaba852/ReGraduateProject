using UnityEngine;
using System.Collections;

public class PlayAudioOnEnable : MonoBehaviour
{
    public AudioClip soundClip; 
    private AudioSource audioSource;
   
    private void OnEnable()
    {
       
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        
        audioSource.clip = soundClip;

        
    

        
        StartCoroutine(PlayAudioCoroutine());
        //일시정시 상태일때에도 실행되도록 코루틴 사용
    }

    private IEnumerator PlayAudioCoroutine()
    {   audioSource.volume = 1f;
        audioSource.Play();
        yield return new WaitForSecondsRealtime(0.4f);
        for(int i = 0;i<10;i++){
        audioSource.volume *= 0.5f;
        yield return new WaitForSecondsRealtime(0.3f);
        }
       
        audioSource.Stop();
        audioSource.volume = 1f;
    }
}