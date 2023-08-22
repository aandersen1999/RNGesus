using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiceScript : MonoBehaviour
{
    [SerializeField] private bool dragged;
    [SerializeField] private Canvas canvas;
    public int diceValue;
    [SerializeField] private Image diceImage;
    public float targetPosition;
    public Animator diceAnimator;

    private void Start()
    {
        diceImage.sprite = CanvasScript.Instance.diceSprites[diceValue - 1];
    }

    private void Update()
    {
        if (transform.position.x != targetPosition)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPosition, 0.05f), transform.position.y);
        }

        if (!GameManager.Instance.gameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject == gameObject)
                {
                    dragged = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                CanvasScript.Instance.SortDices(null);
                dragged = false;
            }

            if (dragged)
            {
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);

                float mouseX = canvas.transform.TransformPoint(pos).x;
                float firstPosition = CanvasScript.Instance.dicePositions[0].position.x - 25;
                float lastPosition = CanvasScript.Instance.dicePositions[CanvasScript.Instance.dicePositions.Count - 1].position.x + 25;


                transform.position = new Vector3(Mathf.Clamp(mouseX, firstPosition, lastPosition), transform.position.y);

                CanvasScript.Instance.SortDices(gameObject);
            }
        }
    }
}
