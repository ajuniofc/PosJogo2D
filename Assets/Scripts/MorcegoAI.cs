using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorcegoAI : MonoBehaviour
{
    private GameController _gameController;
    private Animator lamaAnimator;

    public float speed;

    public GameObject hitBox;

    private bool isFollow;

    private bool isLookLeft;

    private int h;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;
        lamaAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_gameController.currentState != gameState.GAMEPLAY) { return; }

        if (isFollow)
        {
            transform.position = Vector3.MoveTowards(transform.position, _gameController.playerTransform.position, speed * Time.deltaTime);
        }

        if(transform.position.x < _gameController.playerTransform.position.x && !isLookLeft)
        {
            Flip();
        }else if (transform.position.x > _gameController.playerTransform.position.x && isLookLeft)
        {
            Flip();
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "hitBox")
        {
            h = 0;
            Destroy(hitBox);
            _gameController.playSFX(_gameController.sfxEnemyDead, 0.2f);
            lamaAnimator.SetTrigger("dead");
        }
    }

    void OnBecameVisible()
    {
        isFollow = true;
    }

    void OnDead()
    {
        Destroy(this.gameObject);
    }

    void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
