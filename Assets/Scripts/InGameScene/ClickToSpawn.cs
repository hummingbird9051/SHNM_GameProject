using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class ClickToSpawn : MonoBehaviour
{
    [SerializeField] private double clickYPos;
    [SerializeField] private float spawnYPos;
    [SerializeField] private Image previewImage;


    private Camera mainCamera;
    private GameObject spawnedObj;
    private GameObject nextObj;
    private GameObject previewObject;
    private Vector3 previewPosition;
    private bool spawnedFlag = false;
    private float timer;

    void Start()
    {
        mainCamera = Camera.main;
        previewPosition = new Vector3(0, spawnYPos, 0);
        nextObj = Spawner.Instance.Spawn(previewPosition, Random.Range(0, 3));
        nextObj.SetActive(false);
        previewImage.sprite = nextObj.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            if(!spawnedFlag) 
                Preview();

            if (mainCamera.ScreenToWorldPoint(Input.mousePosition).y <= clickYPos && Input.GetMouseButtonDown(0))
            {
                OnBeginDrag();
            }
            if (spawnedObj != null)
            {
                if (Input.GetMouseButton(0))
                {
                    OnDrag();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    OnEndDrag();
                    timer = 1f;
                }
            }
        }
        timer -= Time.deltaTime;
    }

    private void Preview()
    {
        previewObject = nextObj;
        previewObject.transform.position = previewPosition;
        previewObject.SetActive(true);
        previewObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        nextObj = Spawner.Instance.Spawn(new Vector3(0, spawnYPos, -1), Random.Range(0, 4));
        nextObj.SetActive(false);
        previewImage.sprite = nextObj.GetComponent<SpriteRenderer>().sprite;

        spawnedFlag = true;
    }
    private void OnBeginDrag()
    {
        spawnedObj = previewObject;
        previewObject = null;
        spawnedObj.transform.position = new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, spawnYPos, -1);
    }

    private void OnDrag()
    {
        Vector3 mousePos = new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, spawnYPos, -1);
        if (mousePos.x > mainCamera.orthographicSize * mainCamera.aspect - 15 * spawnedObj.transform.localScale.x)
        {
            mousePos.x = mainCamera.orthographicSize * mainCamera.aspect - 15 * spawnedObj.transform.localScale.x;
        }
        else if (mousePos.x < -mainCamera.orthographicSize * mainCamera.aspect + 15 * spawnedObj.transform.localScale.x)
        {
            mousePos.x = -mainCamera.orthographicSize * mainCamera.aspect + 15 * spawnedObj.transform.localScale.x;
        }

        spawnedObj.transform.position = mousePos;
    }

    private void OnEndDrag()
    {
        previewPosition = spawnedObj.transform.position;
        spawnedObj.GetComponent<Rigidbody2D>().gravityScale = 1f;
        spawnedObj = null;
        spawnedFlag = false;
    }
}
