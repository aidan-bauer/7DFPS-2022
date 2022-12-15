using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    [SerializeField] RectTransform player1XHair, player2XHair;
    [Range(0f, 2f)]
    [SerializeField] float p1XHairSensitivity = 1f;
    [Range(0f, 2f)]
    [SerializeField] float p2XHairSensitivity = 1f;
    Vector2 p1XHairPos, p2XHairPos;

    InputHandler inputHandler;
    SimpleHitscan hitscan;
    Health playerHealth;

    float screenXMin, screenXMax, screenYMin, screenYMax;

    public bool isP1Firing, isP2Firing;
    bool isP1Cover, isP2Cover;
    bool isP1CoverPressed = false, isP2CoverPressed = false;
    [SerializeField] bool isInCover = true;

    Coroutine setP1Firing, setP2Firing;
    Coroutine setP1CoverUp, setP2CoverUp;
    Coroutine setP1CoverDown, setP2CoverDown;

    public static Action<Vector3> OnFire;
    public static Action onReload;

    private void Awake()
    {
        inputHandler = FindObjectOfType<InputHandler>();
        hitscan = GetComponent<SimpleHitscan>();
        playerHealth = GetComponent<Health>();
    }

    // Start is called before the first frame update
    void Start()
    {
        p1XHairPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
        p2XHairPos = new Vector2(Screen.width / 2f, Screen.height / 2f);

        screenXMin = player1XHair.sizeDelta.x / 2f;
        screenXMax = Screen.width - (player1XHair.sizeDelta.x / 2f);
        screenYMin = player1XHair.sizeDelta.y / 2f;
        screenYMax = Screen.height - (player1XHair.sizeDelta.y / 2f);
    }

    private void OnEnable()
    {
        inputHandler.OnP1Fire += FireP1;
        inputHandler.OnP1Cover += CoverP1;
        inputHandler.OnP2Fire += FireP2;
        inputHandler.OnP2Cover += CoverP2;
    }

    private void OnDisable()
    {
        inputHandler.OnP1Fire -= FireP1;
        inputHandler.OnP1Cover -= CoverP1;
        inputHandler.OnP2Fire -= FireP2;
        inputHandler.OnP2Cover -= CoverP2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseManager.IsPaused)
        {
            p1XHairPos += inputHandler.player1CursorDelta * p1XHairSensitivity;
            p2XHairPos += inputHandler.player2CursorDelta * p2XHairSensitivity;

            //keep crosshairs in screen bounds
            p1XHairPos = new Vector2(Mathf.Clamp(p1XHairPos.x, screenXMin, screenXMax), Mathf.Clamp(p1XHairPos.y, screenYMin, screenYMax));
            p2XHairPos = new Vector2(Mathf.Clamp(p2XHairPos.x, screenXMin, screenXMax), Mathf.Clamp(p2XHairPos.y, screenYMin, screenYMax));

            player1XHair.position = p1XHairPos;
            player2XHair.position = p2XHairPos;
        }
    }

    public void FireP1()
    {
        //TODO: replace 50f w/player_constants.minXHairDifference
        if (Vector3.Distance(player1XHair.position, player2XHair.position) < 50f)
        {
            if (isP2Firing)
            {
                //Debug.Log("p1: successful fire");

                //do successful fire stuff 
                hitscan.Shoot(FindMidpoint(p1XHairPos, p2XHairPos));
                StopCoroutine(setP2Firing);
                isP2Firing = false;
            }
            else
            {
                setP1Firing = StartCoroutine(SetP1Firing());
            }
        } else
        {
            Debug.Log("p1: crosshairs not close enough");
        }
    }

    public void FireP2()
    {
        //TODO: replace 50f w/player_constants.minXHairDifference
        if (Vector3.Distance(player1XHair.position, player2XHair.position) < 50f)
        {
            if (isP1Firing)
            {
                //Debug.Log("p2: successful fire");

                //do successful fire stuff here
                hitscan.Shoot(FindMidpoint(p1XHairPos, p2XHairPos));
                StopCoroutine(setP1Firing);
                isP1Firing = false;
            }
            else
            {
                setP2Firing = StartCoroutine(SetP2Firing());
            }
        }
        else
        {
            Debug.Log("p2: crosshairs not close enough");
        }
    }

    public void CoverP1()
    {
        isP1CoverPressed = !isP1CoverPressed;
        if (isInCover && isP1CoverPressed)
        {
            if (isP2Cover)
            {
                Debug.Log("p1: successful cover press");
                isInCover = false;
                playerHealth.SetCoverStatus();

                //do successful cover stuff here
                hitscan.Reload();
                StopCoroutine(setP2CoverUp);
                isP2Cover = false;
            }
            else
            {
                setP1CoverUp = StartCoroutine(SetP1Cover());
            }
        }
        else if (!isInCover && !isP1CoverPressed)
        {
            if (isP2Cover)
            {
                Debug.Log("p1: successful cover release");
                isInCover = true;
                playerHealth.SetCoverStatus();

                //do successful cover stuff here
                StopCoroutine(setP2CoverDown);
                isP2Cover = false;
            }
            else
            {
                setP1CoverDown = StartCoroutine(SetP1Cover());
            }
        }
    }

    public void CoverP2()
    {
        isP2CoverPressed = !isP2CoverPressed;
        if (isInCover && isP2CoverPressed)
        {
            if (isP1Cover)
            {
                Debug.Log("p2: successful cover press");
                isInCover = false;
                playerHealth.SetCoverStatus();

                //do successful cover stuff here
                hitscan.Reload();
                StopCoroutine(setP1CoverUp);
                isP1Cover = false;
            }
            else
            {
                setP2CoverUp = StartCoroutine(SetP2Cover());
            }
        }
        else if (!isInCover && !isP2CoverPressed)
        {
            if (isP1Cover)
            {
                Debug.Log("p2: successful cover release");
                isInCover = true;
                playerHealth.SetCoverStatus();

                //do successful cover stuff here
                StopCoroutine(setP1CoverDown);
                isP1Cover = false;
            }
            else
            {
                setP2CoverDown = StartCoroutine(SetP2Cover());
            }
        }
    }

    IEnumerator SetP1Firing()
    {
        isP1Firing = true;
        yield return new WaitForSeconds(0.3f);
        isP1Firing = false;

        if (!isP2Firing)
        {
            Debug.Log("player 2 failed to press fire in time");
        }
    }

    IEnumerator SetP2Firing()
    {
        isP2Firing = true;
        yield return new WaitForSeconds(0.3f);
        isP2Firing = false;

        if (!isP1Firing)
        {
            Debug.Log("player 1 failed to press fire in time");
        }
    }

    IEnumerator SetP1Cover()
    {
        isP1Cover = true;
        yield return new WaitForSeconds(0.75f);
        isP1Cover = false;

        if (!isP2Cover)
        {
            Debug.Log("player 2 failed to press cover in time");
        }
    }

    IEnumerator SetP2Cover()
    {
        isP2Cover = true;
        yield return new WaitForSeconds(0.75f);
        isP2Cover = false;

        if (!isP1Cover)
        {
            Debug.Log("player 1 failed to press cover in time");
        }
    }

    Vector3 FindMidpoint(Vector3 a, Vector3 b)
    {
        Vector3 aToB = a - b;
        float mag = aToB.magnitude;
        return b + aToB.normalized * (mag / 2f);
    }
}
