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
        inputHandler.OnP1Fire += Fire;
        inputHandler.OnP1Cover += Cover;
        inputHandler.OnP2Fire += Fire;
        inputHandler.OnP2Cover += Cover;
    }

    private void OnDisable()
    {
        inputHandler.OnP1Fire -= Fire;
        inputHandler.OnP1Cover -= Cover;
        inputHandler.OnP2Fire -= Fire;
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

    public void Fire()
    {
        Debug.Log("fire");
    }

    public void Cover()
    {
        Debug.Log("Player is out of cover");
    }
}
