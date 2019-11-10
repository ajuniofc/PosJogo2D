using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum musicaFase
{
    FLORESTA, CAVERNA, THEEND, GAMEOVER
}

public enum gameState
{
    TITULO, GAMEPLAY, GAMEOVER, THEEND
}

public class GameController : MonoBehaviour
{
    public gameState currentState;
    public GameObject painetlTitulo, painelGameOver, painelEnd;

    public  Transform       playerTransform;
    private Camera          camera;

    public  Transform       limiteCamEsq, limiteCamDir, limiteCamSup, limiteCamBaixo;

    public  float           speedCamera;

    [Header("Audio")]
    public AudioSource sfxSource, musicSource;

    public AudioClip sfxJump, sfxAtack, sfxCoin, sfxEnemyDead, sfxDamage;
    public AudioClip[] sfxStep;

    public GameObject[] fase;

    public musicaFase musicaAtual;

    public AudioClip musicFloresta, musicCaverna, musicEnd, musicGameOver;

    public int moedasColetadas = 0;
    public Text moedasTxt;
    public Image[] coracoes;
    public int vida;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;

        heartController();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == gameState.TITULO && Input.GetKeyDown(KeyCode.Space))
        {
            painetlTitulo.SetActive(false);
            currentState = gameState.GAMEPLAY;
        }
        else if(currentState == gameState.GAMEOVER && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if(currentState == gameState.THEEND && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void LateUpdate()
    {
        float posCamX = playerTransform.position.x;
        float posCamy = playerTransform.position.y;

        if(camera.transform.position.x < limiteCamEsq.position.x &&
            playerTransform.position.x < limiteCamEsq.position.x) {

            posCamX = limiteCamEsq.position.x;
        }else if (camera.transform.position.x > limiteCamDir.position.x &&
            playerTransform.position.x > limiteCamDir.position.x)
        {

            posCamX = limiteCamDir.position.x;
        }

        if(camera.transform.position.y < limiteCamBaixo.position.y &&
            playerTransform.position.y < limiteCamBaixo.position.y) {
            posCamy = limiteCamBaixo.position.y;
        }else if (camera.transform.position.y > limiteCamSup.position.y &&
            playerTransform.position.y > limiteCamSup.position.y)
        {
            posCamy = limiteCamSup.position.y;
        }

        Vector3 posCam = new Vector3(posCamX, posCamy, camera.transform.position.z);

        camera.transform.position = Vector3.Lerp(camera.transform.position, posCam, speedCamera * Time.deltaTime);
    }

    public void playSFX(AudioClip sfxClip, float volume) {
        sfxSource.PlayOneShot(sfxClip, volume);
    }

    public void trocarMusica(musicaFase novaMusica)
    {
        AudioClip clip = null;
        switch (novaMusica)
        {
            case musicaFase.FLORESTA:
                clip = musicFloresta;
                break;
            case musicaFase.CAVERNA:
                clip = musicCaverna;
                break;
        }

        StartCoroutine("controleMusica", clip);
    }

    IEnumerator controleMusica(AudioClip musica)
    {
        float volumeMaximo = musicSource.volume;
        for (float volume = volumeMaximo; volume > 0; volume -= 0.01f)
        {
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }

        musicSource.clip = musica;
        musicSource.Play();

        for (float volume = 0; volume < volumeMaximo; volume += 0.01f)
        {
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }
    }

    public void heartController()
    {
        foreach(Image image in coracoes)
        {
            image.enabled = false;
        }

        for(int i = 0; i < vida; i++)
        {
            coracoes[i].enabled = true;
        }
    }

    public void theEnd()
    {
        currentState = gameState.THEEND;
        painelEnd.SetActive(true);
        trocarMusica(musicaFase.THEEND);
    }

    public void getCoint()
    {
        moedasColetadas += 1;
        moedasTxt.text = moedasColetadas.ToString();
    }

    public void getHit()
    {
        vida -= 1;
        heartController();
        if(vida <= 0)
        {
            playerTransform.gameObject.SetActive(false);
            painelGameOver.SetActive(true);
            currentState = gameState.GAMEOVER;
            trocarMusica(musicaFase.GAMEOVER);
        }
    }
}
