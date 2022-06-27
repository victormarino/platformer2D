using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public HealthBase healthBase;

    [Header("Setup")]
    public SOPlayerSetup soPlayerSetup;

    //public Animator animator;

    private float _currentSpeed;

    private Animator _currentPlayer;

    private float playerDirectionX = 1;

    private void Awake()
    {
        if(healthBase != null)
        {
            healthBase.OnKill += OnPlayerKill;
        }

        _currentPlayer = Instantiate(soPlayerSetup.player, transform);

        PlayerDestroyHelper playerDestroyHelper = _currentPlayer.GetComponentInChildren<PlayerDestroyHelper>();
        if (playerDestroyHelper != null)
        {
            playerDestroyHelper.player = this;
        }

        GunBase gunBase = _currentPlayer.GetComponentInChildren<GunBase>();
        if (gunBase != null)
        {
            gunBase.playerSideReference = transform;
        }

    }

    private void OnPlayerKill()
    {
        healthBase.OnKill -= OnPlayerKill;

        _currentPlayer.SetTrigger(soPlayerSetup.triggerDeath);
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _currentSpeed = soPlayerSetup.speedRun;
            _currentPlayer.speed = 2;
        }
        else
        {
            _currentSpeed = soPlayerSetup.speed;
            _currentPlayer.speed = 1; 
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            myRigidbody.velocity = new Vector2(-_currentSpeed, myRigidbody.velocity.y);
            /*if (myRigidbody.transform.localScale.x != -1)
            {
                myRigidbody.transform.DOScaleX(-1, playerSwipeDuration);
            }*/

            var lS = myRigidbody.transform.localScale;
            lS.x = -1;
            myRigidbody.transform.localScale = lS;

            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
            playerDirectionX = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            myRigidbody.velocity = new Vector2(_currentSpeed, myRigidbody.velocity.y);
            /*if (myRigidbody.transform.localScale.x != 1)
            {
                myRigidbody.transform.DOScaleX(1, playerSwipeDuration);
            }*/

            var lS = myRigidbody.transform.localScale;
            lS.x = 1;
            myRigidbody.transform.localScale = lS;

            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
            playerDirectionX = 1;
        }
        else
        {
            _currentPlayer.SetBool(soPlayerSetup.boolRun, false);
        }

        if (myRigidbody.velocity.x > 0)
        {
            myRigidbody.velocity += soPlayerSetup.friction;
        }
        else if (myRigidbody.velocity.x < 0)
        {
            myRigidbody.velocity -= soPlayerSetup.friction;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidbody.velocity = Vector2.up * soPlayerSetup.forceJump;
            myRigidbody.transform.localScale = Vector2.one;

            DOTween.Kill(myRigidbody.transform);

            HandleScaleJump();
        }
    }

    private void HandleScaleJump()
    {
        myRigidbody.transform.DOScaleY(soPlayerSetup.jumpScaleY, soPlayerSetup.animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(soPlayerSetup.ease);
        //myRigidbody.transform.DOScaleX(jumpScaleX, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);

        DOTween.To(XScaleGetter, XScaleSetter, soPlayerSetup.jumpScaleX, soPlayerSetup.animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(soPlayerSetup.ease);
    }

    private float XScaleGetter()
    {
        return Mathf.Abs(myRigidbody.transform.localScale.x);
    }

    private void XScaleSetter(float value)
    {
        Vector3 localScale = myRigidbody.transform.localScale;

        localScale.x = value * playerDirectionX;

        myRigidbody.transform.localScale = localScale;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
