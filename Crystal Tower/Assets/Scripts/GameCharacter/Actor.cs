using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    protected const int MOVING_INTERVAL = 256;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private float moveTime = 0.01f;
    private float inverseMoveTime;

    protected virtual void AttemptMove(int xDir, int yDir)
    {
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + new Vector2(xDir, yDir);

        // 移動判定用 衝突レイヤーを全て入れる
        int LayerObj = LayerMask.GetMask(new string[] { "Entity", "Wall" });
        // 攻撃判定用 HPの存在するオブジェクトを置くレイヤーを全て入れる
        int LayerEntity = LayerMask.GetMask(new string[] { "Entity" });

        this.rb = GetComponent<Rigidbody2D>();
        this.boxCollider = GetComponent<BoxCollider2D>();
        
        // 誤作動防止のため自身の衝突判定をオフにする
        boxCollider.enabled = false;

        RaycastHit2D hitObj = Physics2D.Linecast(startPosition, endPosition, LayerObj);
        RaycastHit2D hitEntity = Physics2D.Linecast(startPosition, endPosition, LayerEntity);

        boxCollider.enabled = true;

        if (hitObj.transform == null)
        {
            StartCoroutine(Movement(endPosition));
        }
        else if (hitEntity.transform != null)
        {
            GameObject hitComponent = hitEntity.transform.gameObject;
            OnCantMove(hitComponent);
        }
    }

    protected IEnumerator Movement(Vector3 endPosition)
    {
        float sqrRemainingDistance = (transform.position - endPosition).sqrMagnitude;
        inverseMoveTime = 1f / moveTime;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb.position, endPosition, inverseMoveTime * Time.deltaTime);
            rb.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - endPosition).sqrMagnitude;
            yield return null;
        }
    }

    protected abstract void OnCantMove(GameObject hitComponent);
}
