using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerCode : MonoBehaviour
{
    public SpriteRenderer renderer;
    [SerializeField] Rigidbody2D rb;

    private void Start()
    {
        StartCoroutine(GameManager.Instance.CameraShake());
        float angularForce = Random.Range(-180, 180);
        float horizontalForce = Random.Range(-50, 50);
        float verticalForce = Random.Range(30, 50);

        rb.angularVelocity = angularForce;
        rb.AddForce(new Vector2(horizontalForce, verticalForce), ForceMode2D.Impulse);
        StartCoroutine(RemoveDummy());
    }

    private IEnumerator RemoveDummy()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
