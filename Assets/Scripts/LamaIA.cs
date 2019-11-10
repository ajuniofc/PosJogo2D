using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamaIA : MonoBehaviour
{
    private GameController _gameController;
    private Rigidbody2D lamaRb;
    private Animator lamaAnimator;

    public float speed;
    public float timeToWalk;

    public GameObject hitBox;

    public bool isLookLeft;

    private int h;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;

        lamaRb = GetComponent<Rigidbody2D>();
        lamaAnimator = GetComponent<Animator>();

        StartCoroutine("lamaWalk");
    }

    // Update is called once per frame
    void Update()
    {
        if(_gameController.currentState != gameState.GAMEPLAY) { return; }

        if (h > 0 && !isLookLeft)
        {
            Flip();
        }
        else if (h < 0 && isLookLeft)
        {
            Flip();
        }

        lamaRb.velocity = new Vector2(h * speed, lamaRb.velocity.y);

        if(h != 0)
        {
            lamaAnimator.SetBool("isWalk", true);
        }
        else {
            lamaAnimator.SetBool("isWalk", false);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "hitBox") {
            h = 0;
            StopCoroutine("lamaWalk");
            Destroy(hitBox);
            _gameController.playSFX(_gameController.sfxEnemyDead, 0.2f);
            lamaAnimator.SetTrigger("dead");
        }
    }

    IEnumerator lamaWalk()
    {
        int rand = Random.Range(0, 100);

        if(rand < 55)
        {
            h = -1;
        }else if(rand < 65)
        {
            h = 0;
        }
        else
        {
            h = 1;
        }

        yield return new WaitForSeconds(timeToWalk);
        StartCoroutine("lamaWalk");
    }

    void OnDead() {
        Destroy(this.gameObject);
    }

    void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
