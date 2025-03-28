using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private  SoundManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

        }
        else
        {
            Destroy(gameObject);  // Удаляем дубликаты при смене сцен
        }
    }

    public void OnButtonClickAudio()
    {
        StartCoroutine(PlaySound());
    }
    // Метод для воспроизведения звука
    IEnumerator PlaySound()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);

    }
}
