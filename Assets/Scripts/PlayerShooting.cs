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
    public bool canPlayerShoot;
    [SerializeField] Vector3 coverOffset = Vector3.up;
    [SerializeField] Vector3 inCoverPos, outCoverPos;
    Vector3 lerpPos;
    [SerializeField] float coverProgress = 0;   //0 = in cover, 1 = out of cover
    float coverTransitionTimer = 0;
    Vector2 p1XHairPos, p2XHairPos;

    InputHandler inputHandler;
    SimpleHitscan hitscan;
    Health playerHealth;
    

    float screenXMin, screenXMax, screenYMin, screenYMax;

    bool isP1Firing, isP2Firing;
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

        p1XHairSensitivity = PlayerPrefs.GetFloat("player1Sens", 1f);
        p2XHairSensitivity = PlayerPrefs.GetFloat("player2Sens", 1f);
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

        StageManager.OnStageBegin += SetPlayerCoverPos;
        StageManager.OnStageBegin += ChangeCanPlayerShoot;
        StageManager.OnStageComplete += ChangeCanPlayerShoot;
    }

    private void OnDisable()
    {
        inputHandler.OnP1Fire -= FireP1;
        inputHandler.OnP1Cover -= CoverP1;
        inputHandler.OnP2Fire -= FireP2;
        inputHandler.OnP2Cover -= CoverP2;

        StageManager.OnStageBegin -= SetPlayerCoverPos;
        StageManager.OnStageBegin -= ChangeCanPlayerShoot;
        StageManager.OnStageComplete -= ChangeCanPlayerShoot;
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

    private void FixedUpdate()
    {
        if (!PauseManager.IsPaused)
        {
            if (!isInCover)
            {
                if (coverTransitionTimer < 1f)
                {
                    coverTransitionTimer += Time.fixedDeltaTime * Manager.constants.coverStateChangeSpeed;
                    coverProgress = Mathf.Clamp01(coverTransitionTimer / 1f);
                    transform.position = Vector3.Lerp(inCoverPos, outCoverPos, coverProgress);
                }
            }
            else
            {
                if (coverTransitionTimer > 0f)
                {
                    coverTransitionTimer -= Time.fixedDeltaTime * Manager.constants.coverStateChangeSpeed;
                    coverProgress = Mathf.Clamp01(coverTransitionTimer / 1f);
                    transform.position = Vector3.Lerp(inCoverPos, outCoverPos, coverProgress);
                }
            }
        }
    }

    public void FireP1()
    {
        if (!isInCover && canPlayerShoot)
        {
            if (Vector3.Distance(player1XHair.position, player2XHair.position) < Manager.constants.minXHairDifference)
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
            }
            else
            {
                Debug.Log("p1: crosshairs not close enough");
            }
        }
    }

    public void FireP2()
    {
        if (!isInCover && canPlayerShoot)
        {
            if (Vector3.Distance(player1XHair.position, player2XHair.position) < Manager.constants.minXHairDifference)
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
    }

    public void CoverP1()
    {
        isP1CoverPressed = !isP1CoverPressed;
        if (isInCover && isP1CoverPressed && canPlayerShoot)
        {
            if (isP2Cover)
            {
                //Debug.Log("p1: successful cover press");
                isInCover = false;

                playerHealth.SetCoverStatus(isInCover);

                //do successful cover stuff here
                StopCoroutine(setP2CoverUp);
                isP2Cover = false;
            }
            else
            {
                setP1CoverUp = StartCoroutine(SetP1Cover());
            }
        }
        else if (!isInCover && !isP1CoverPressed && canPlayerShoot)
        {
            if (isP2Cover)
            {
                //Debug.Log("p1: successful cover release");
                isInCover = true;

                playerHealth.SetCoverStatus(isInCover);

                //do successful cover stuff here
                StopCoroutine(setP2CoverDown);
                hitscan.Reload();
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
        if (isInCover && isP2CoverPressed && canPlayerShoot)
        {
            if (isP1Cover)
            {
                //Debug.Log("p2: successful cover press");
                isInCover = false;

                playerHealth.SetCoverStatus(isInCover);

                //do successful cover stuff here
                StopCoroutine(setP1CoverUp);
                isP1Cover = false;
            }
            else
            {
                setP2CoverUp = StartCoroutine(SetP2Cover());
            }
        }
        else if (!isInCover && !isP2CoverPressed && canPlayerShoot)
        {
            if (isP1Cover)
            {
                //Debug.Log("p2: successful cover release");
                isInCover = true;

                playerHealth.SetCoverStatus(isInCover);

                //do successful cover stuff here
                hitscan.Reload();
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
        yield return new WaitForSeconds(Manager.constants.maxFireInterval);
        isP1Firing = false;

        if (!isP2Firing)
        {
            Debug.Log("player 2 failed to press fire in time");
        }
    }

    IEnumerator SetP2Firing()
    {
        isP2Firing = true;
        yield return new WaitForSeconds(Manager.constants.maxFireInterval);
        isP2Firing = false;

        if (!isP1Firing)
        {
            Debug.Log("player 1 failed to press fire in time");
        }
    }

    IEnumerator SetP1Cover()
    {
        isP1Cover = true;
        yield return new WaitForSeconds(Manager.constants.maxCoverInterval);
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

    public void SetPlayerCoverPos(int stage)
    {
        inCoverPos = transform.position;
        outCoverPos = transform.position + coverOffset;
        isInCover = true;
    }

    Vector3 FindMidpoint(Vector3 a, Vector3 b)
    {
        Vector3 aToB = a - b;
        float mag = aToB.magnitude;
        return b + aToB.normalized * (mag / 2f);
    }

    public void ChangeCanPlayerShoot(int stage)
    {
        canPlayerShoot = !canPlayerShoot;
    }
}
