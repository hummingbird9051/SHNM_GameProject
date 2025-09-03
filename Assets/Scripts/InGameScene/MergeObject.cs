using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MergeObject : MonoBehaviour
{
    public int objectID;
    public int scoreValue;

    public AudioClip popSound;

    private GameManager gameManager;
    private int time = 0;
    private bool isMerging = false;


    void OnEnable()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (objectID == 10)
        {
            time += 1;
            if(time > 200)
            {
                gameManager.GameClear();
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (isMerging || !gameObject.activeSelf)
        {
            return;
        }

        MergeObject otherMergeObject = other.collider.gameObject.GetComponent<MergeObject>();
        if (otherMergeObject != null && otherMergeObject.objectID == objectID)
        {
            if (this.GetInstanceID() < otherMergeObject.GetInstanceID())
            {
                this.isMerging = true;
                otherMergeObject.isMerging = true;
                GameObject nextObj = Spawner.Instance.Spawn(
                    (transform.position + other.gameObject.transform.position) / 2, 
                    objectID + 1);
                if (nextObj == null) return;
                if(popSound != null)
                {
                    AudioSource.PlayClipAtPoint(popSound, transform.position);
                    Debug.Log("pop");
                }
                Spawner.Instance.Delete(gameObject);
                Spawner.Instance.Delete(other.gameObject);
                GameEvent.RaiseScoreGained(scoreValue);
            }
        }
    }
}
