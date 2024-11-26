using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    private BoxCollider2D boxCollider;
    public InventoryObject inventory;
    private Vector3 moveDelta;
    private RaycastHit2D hit;
    public Animator animator;

    public static Player GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one player in the scene");
        }
        instance = this;
    }
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<Item>();
        if (item != null) {
            if (item.item.type == ItemType.Book)
            {
                Debug.Log("Book Received!");
                inventory.AddItem(item.item, 1);
                Destroy(collision.gameObject);
            }
            if (item.item.type == ItemType.Badge)
            {
                Debug.Log("Badge Received!");
                inventory.AddBadge(item.item, 1);
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
        inventory.BadgeContainer.Clear();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            Debug.Log("sav!");
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Debug.Log("lod!");
            inventory.Load();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (DialogueManager.instance.dialogueIsPlaying) 
        {
            return;
        }
        if (MenuManager.instance.menuIsActive)
        {
            return;
        }
        if (UIManager.instance.pauseisActive)
        {
            return;
        }


        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        
        moveDelta = new Vector3(x, y, 0);
        animator.SetFloat("speed", moveDelta.sqrMagnitude);

        //Sprite mirroring horizontal
        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1 ,1);

        hit = Physics2D.BoxCast(transform.position, boxCollider.size/2, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Characters", "Blockers"));

        if (hit.collider == null)
        {
            //transform ovement
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }
        else {
            Debug.Log("HIT");
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size/2, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Characters", "Blockers"));

        if (hit.collider == null)
        {
            //transform ovement
            transform.Translate(moveDelta.x * Time.deltaTime, 0 , 0);
        }
        else
        {
            Debug.Log("HIT");
        }
    }
}
