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

    float screenXMin, screenXMax, screenYMin, screenYMax;

    private void Awake()
    {
        inputHandler = FindObjectOfType<InputHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        p1XHairPos = Vector2.zero;

        screenXMin = -(Screen.width / 2f) + (player1XHair.sizeDelta.x / 2f);
        screenXMax = (Screen.width / 2f) - (player1XHair.sizeDelta.x / 2f);
        screenYMin = -(Screen.height / 2f) + (player1XHair.sizeDelta.y / 2f);
        screenYMax = (Screen.height / 2f) - (player1XHair.sizeDelta.y / 2f);
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

        p1XHairPos = new Vector2(Mathf.Clamp(p1XHairPos.x, screenXMin, screenXMax), Mathf.Clamp(p1XHairPos.y, screenYMin, screenYMax));
        p2XHairPos = new Vector2(Mathf.Clamp(p2XHairPos.x, screenXMin, screenXMax), Mathf.Clamp(p2XHairPos.y, screenYMin, screenYMax));

        player1XHair.anchoredPosition = p1XHairPos;
        player2XHair.anchoredPosition = p2XHairPos;
    }

    public void FireP1()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(p1XHairPos.x + (Screen.width / 2f), p1XHairPos.y + (Screen.height / 2f), 0));
        RaycastHit p1Hit;

        if (Physics.Raycast(ray, out p1Hit))
        {
            Debug.Log(p1Hit.transform.name);
        }

        Debug.Log("fire p1");
    }

    public void FireP2()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(p2XHairPos.x + (Screen.width / 2f), p2XHairPos.y + (Screen.height / 2f), 0));
        RaycastHit p2Hit;

        if (Physics.Raycast(ray, out p2Hit))
        {
            Debug.Log(p2Hit.transform.name);
        }

        Debug.Log("fire p2");
    }

    public void Cover()
    {
        Debug.Log("Player is out of cover");
    }
}
