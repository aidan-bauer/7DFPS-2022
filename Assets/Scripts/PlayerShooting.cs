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

    [SerializeField] InputHandler inputHandler;

    float screenXMin, screenXMax, screenYMin, screenYMax;

    bool isP1Firing, isP2Firing;
    bool isXHairClose;

    Coroutine setP1Firing, setP2Firing;

    private void Awake()
    {
        inputHandler = FindObjectOfType<InputHandler>();
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
        inputHandler.OnP1Cover += Cover;
        inputHandler.OnP2Fire += FireP2;
        inputHandler.OnP2Cover += Cover;
    }

    private void OnDisable()
    {
        inputHandler.OnP1Fire -= FireP1;
        inputHandler.OnP1Cover -= Cover;
        inputHandler.OnP2Fire -= FireP2;
        inputHandler.OnP2Cover -= Cover;
    }

    // Update is called once per frame
    void Update()
    {
        p1XHairPos += inputHandler.player1CursorDelta * p1XHairSensitivity;
        p2XHairPos += inputHandler.player2CursorDelta * p2XHairSensitivity;

        //keep crosshairs in screen bounds
        p1XHairPos = new Vector2(Mathf.Clamp(p1XHairPos.x, screenXMin, screenXMax), Mathf.Clamp(p1XHairPos.y, screenYMin, screenYMax));
        p2XHairPos = new Vector2(Mathf.Clamp(p2XHairPos.x, screenXMin, screenXMax), Mathf.Clamp(p2XHairPos.y, screenYMin, screenYMax));

        player1XHair.position = p1XHairPos;
        player2XHair.position = p2XHairPos;
    }

    public void FireP1()
    {
        //TODO: replace 50f w/player_constants.minXHairDifference
        Debug.Log(Vector3.Distance(player1XHair.position, player2XHair.position));
        if (Vector3.Distance(player1XHair.position, player2XHair.position) < 50f)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(p1XHairPos.x + (Screen.width / 2f), p1XHairPos.y + (Screen.height / 2f), 0));
            RaycastHit p1Hit;

            if (Physics.Raycast(ray, out p1Hit))
            {
                //Debug.Log(p1Hit.transform.name);
            }

            if (isP2Firing)
            {
                Debug.Log("p1: successful fire");
                //do successful fire stuff here
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
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(p2XHairPos.x + (Screen.width / 2f), p2XHairPos.y + (Screen.height / 2f), 0));
            RaycastHit p2Hit;

            if (Physics.Raycast(ray, out p2Hit))
            {
                //Debug.Log(p2Hit.transform.name);
            }

            if (isP1Firing)
            {
                Debug.Log("p2: successful fire");
                //do successful fire stuff here
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

    public void Cover()
    {
        Debug.Log("Player is out of cover");
    }

    IEnumerator SetP1Firing()
    {
        isP1Firing = true;
        yield return new WaitForSeconds(0.3f);
        isP1Firing = false;

        if (!isP2Firing)
        {
            Debug.Log("player 2 failed to press in time");
        }
    }

    IEnumerator SetP2Firing()
    {
        isP2Firing = true;
        yield return new WaitForSeconds(0.3f);
        isP2Firing = false;

        if (!isP1Firing)
        {
            Debug.Log("player 1 failed to press in time");
        }
    }
}
