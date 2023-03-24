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

        // �ړ�����p �Փ˃��C���[��S�ē����
        int LayerObj = LayerMask.GetMask(new string[] { "Entity", "Wall" });
        // �U������p HP�̑��݂���I�u�W�F�N�g��u�����C���[��S�ē����
        int LayerEntity = LayerMask.GetMask(new string[] { "Entity" });

        this.rb = GetComponent<Rigidbody2D>();
        this.boxCollider = GetComponent<BoxCollider2D>();
        
        // ��쓮�h�~�̂��ߎ��g�̏Փ˔�����I�t�ɂ���
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
