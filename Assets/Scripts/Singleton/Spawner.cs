using UnityEngine;

public class Spawner : SingletonBase<Spawner>
{
    public GameObject[] characters;
    public GameObject parent;

    protected override void Awake()
    {
        base.Awake();
    }

    public GameObject Spawn(Vector2 pos, int spawnNum)
    {
        GameObject spawnOjb = Instantiate(characters[spawnNum], pos, Quaternion.identity);
        spawnOjb.transform.SetParent(parent.transform);
        return spawnOjb;
    }

    public void Delete(GameObject obj)
    {
        Destroy(obj);
    }
}
