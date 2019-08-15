using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour{
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private int _createOnStart;

    private List<GameObject> _pooledObjects = new List<GameObject>();

    private void Start(){
        for (int i = 0; i < _createOnStart; i++){
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject(){
        GameObject obj = Instantiate(_objectPrefab);
        obj.SetActive(false);
        _pooledObjects.Add(obj);
        
        return obj;
    }

    public GameObject GetObject(){
        GameObject obj = _pooledObjects.Find(x => x.activeInHierarchy == false);

        if (!obj)
            obj = CreateNewObject();
        
        obj.SetActive(true);

        return obj;
    }
}
