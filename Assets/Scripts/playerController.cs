using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    private GameController _gameController;

    private Rigidbody2D         playerRb;
    private Animator            playerAnimator;
    private SpriteRenderer playerSR;

    public  float               speed;
    public  float               jumpForce;
    public  bool                isLookLeft;

    public  Transform           groundCheck;
    private bool                isGrounded;
    private bool                isAtack;

    public  GameObject          hitBoxPrefab;
    public  Transform            mao;

    public Color hitColor;
    public Color noHitColor;

    // Start is called before the first frame update
    void Start() {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;
        _gameController.playerTransform = this.transform;
        playerSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        playerAnimator.SetBool("isGrounded", isGrounded);

        if(_gameController.currentState != gameState.GAMEPLAY)
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            playerAnimator.SetInteger("h", 0);
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");

        if (isAtack && isGrounded) {
            h = 0;
        }

        if (h > 0 && isLookLeft) {
            Flip();
        }else if (h < 0 && !isLookLeft) {
            Flip();
        }

        float speedY = playerRb.velocity.y;

        if(Input.GetButtonDown("Jump") && isGrounded) {
            _gameController.playSFX(_gameController.sfxJump, 0.3f);
            playerRb.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetKeyUp(KeyCode.J) && !isAtack){
            _gameController.playSFX(_gameController.sfxAtack, 0.5f);
            isAtack = true;
            playerAnimator.SetTrigger("atack");
        }

        playerRb.velocity = new Vector2(h * speed, speedY);

        playerAnimator.SetInteger("h", (int) h);

        playerAnimator.SetFloat("speedY", speedY);
        playerAnimator.SetBool("isAtack", isAtack);
    }

    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "coletavel") {
            _gameController.playSFX(_gameController.sfxCoin, 0.3f);
            Destroy(collision.gameObject);
            _gameController.getCoint();
        }else if (collision.gameObject.tag == "damage")
        {
            _gameController.getHit();
            if(_gameController.vida > 0)
            {
                StartCoroutine("damageController");
            }
        }else if(collision.gameObject.tag == "win") {
            _gameController.theEnd();
        }
    }



    void Flip() {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    void onEndeAtack()
    {
        isAtack = false;
    }

    void hitBoxAtack() {
        GameObject hitBoxTemp = Instantiate(hitBoxPrefab, mao.position, transform.localRotation);
        Destroy(hitBoxTemp, 0.2f);
    }

    void footStep() {
        _gameController.playSFX(_gameController.sfxStep[Random.Range(0, _gameController.sfxStep.Length)], 1f);
    }

    IEnumerator damageController()
    {
        _gameController.playSFX(_gameController.sfxDamage, 0.3f);
        this.gameObject.layer = LayerMask.NameToLayer("Invencivel");
        playerSR.color = hitColor;
        yield return new WaitForSeconds(0.5f);
        playerSR.color = noHitColor;

        for(int i = 0; i < 5; i++)
        {
            playerSR.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerSR.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        playerSR.color = Color.white;
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }


}
