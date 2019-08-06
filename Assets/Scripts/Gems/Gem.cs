using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gem : MonoBehaviour, IPointerClickHandler

{
    [SerializeField] SpriteRenderer gemSprite;
    [SerializeField] public float fallSpeed;

    public int type;
    public int column;
    public int line;
    public Vector3 targetPosition;

    private Rigidbody2D gemRigidbody;

    private void Start()
    {
        gemSprite = this.GetComponent<SpriteRenderer>();
        gemRigidbody = this.GetComponent<Rigidbody2D>();
    }

    public void ColorChange(float color)
    {
        gemSprite.color = new Color(color, color, color);
    }

    public void ChangeType(int newType,Sprite sprite)
    {
        type = newType;
        gemSprite.sprite = sprite;
    }

    private void FixedUpdate()
    {
        if (transform.transform.position != targetPosition)
        {
            gemRigidbody.MovePosition(Vector3.Lerp(transform.position, targetPosition, fallSpeed));
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (type < 5)
        {
            GemsSelector.SelectGem(this);
        }
        else
        {
            ActivateGem();
        }
    }

    public void ActivateGem()
    {
        this.gameObject.SetActive(false);
        SpecialGemActivator.ActivateGem(this);
    }
}
