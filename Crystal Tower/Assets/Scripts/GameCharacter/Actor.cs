using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private float moveTime = 0.01f;
    private float inverseMoveTime;
    protected bool isMoving = false;

    protected Coroutine _moving;

    protected virtual void Start()
    {
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.rb = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        int layerObjects = LayerMask.GetMask(new string[] { "Wall", "Entity" });

        boxCollider.enabled = false;

        hit = Physics2D.Linecast(start, end, layerObjects);

        boxCollider.enabled = true;

        if (hit.transform is null && !isMoving)
        {
            _moving = StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;

        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon && isMoving)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb.position, end, inverseMoveTime * Time.deltaTime);
            rb.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        rb.MovePosition(end);
        isMoving = false;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform is null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }


    protected abstract void OnCantMove<T>(T hitComponent)where T : Component;
}
