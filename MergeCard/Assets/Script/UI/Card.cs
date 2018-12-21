using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour {
    [HideInInspector]
    public GameObject mask = null;
    [HideInInspector]
    public Image mySprite;
    [HideInInspector]
    public int myNum;
    [HideInInspector]
    public Transform pro;
    [HideInInspector]
    public bool isReady = false;
    [HideInInspector]
    public bool isMoving = false;
    [HideInInspector]
    public bool isCanMove = false;
    [HideInInspector]
    public bool merging = false;
    [HideInInspector]
    public bool isSeted = false;
    private Vector3 canvas;


    void OnTriggerEnter2D(Collider2D other) {
        GameView.GetInstance.CollisionEnter(other.gameObject, gameObject);
    }

    void OnTriggerStay2D(Collider2D other) {
        GameView.GetInstance.CollisionStay(other.gameObject, gameObject);
    }

    void OnTriggerExit2D(Collider2D other) {
        GameView.GetInstance.CollisionExit(other.gameObject, gameObject);
    }

    // Use this for initialization
    void Start() {
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        trigger.triggers = new List<EventTrigger.Entry>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback = new EventTrigger.TriggerEvent();

        entry.callback.AddListener((data) => {
            SetCanMove(true);
        });
        trigger.triggers.Add(entry);


        Button cardBtn = gameObject.GetComponent<Button>();
        cardBtn.onClick.AddListener(() => {
            GameView.GetInstance.cutOff(this);
        });
    }

    //设置为已经放置的状态
    public void Seted() {
        isSeted = true;
    }

    //设置为已经放置的状态
    public void SetCanMove(bool isCan) {
        isCanMove = isCan;
    }



    // Update is called once per frame
    void Update() {
        if (isSeted || !isCanMove) {
            isMoving = false;
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (isReady) {
                isMoving = true;
            }
        }
        else if (Input.GetMouseButton(0)) {
            if (isMoving) {
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                Vector3 realPos = new Vector3(pos.x * canvas.x, pos.y * canvas.y, pos.z);
                GameView.GetInstance.cardMoving(transform, realPos);
            }
        }
        else if (Input.GetMouseButtonUp(0)) {
            if (isMoving) {
                isCanMove = false;
                GameView.GetInstance.MoveOver(this);
                isMoving = false;
            }
        }
    }
}
