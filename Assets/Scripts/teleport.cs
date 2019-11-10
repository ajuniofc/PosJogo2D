using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    private GameController _gameController;
    public Transform pontoSaida;
    public Transform posCamera;

    public Transform limiteCamEsq, limiteCamDir, limiteCamSup, limiteCamBaixo;

    public musicaFase novaMusica;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _gameController.trocarMusica(musicaFase.CAVERNA);
            collision.transform.position = pontoSaida.position;
            Camera.main.transform.position = posCamera.position;

            _gameController.limiteCamEsq = limiteCamEsq;
            _gameController.limiteCamDir = limiteCamDir;
            _gameController.limiteCamSup = limiteCamSup;
            _gameController.limiteCamBaixo = limiteCamBaixo;

        }
    }
}
