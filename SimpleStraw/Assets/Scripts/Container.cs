using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{

    private List<GameObject> _contents;
    public int capacity;

    public bool isFull {
        get {
            return _contents.Count >= capacity;
        }
    }

    public bool Contains(GameObject item) {

        foreach (GameObject obj in _contents) {

            return obj == item;

        }

        return false;
    
    }

    public bool ContainsType(System.Type type) {
    
        foreach (GameObject obj in _contents) {

            return obj.GetComponent(type) != null;

        }

        return false;
    
    }

    public bool Add(GameObject item) {

        if (!isFull && !_contents.Contains(item)) {

            _contents.Add(item);
            return true;

        }

        return false;

    }

    public GameObject Take(GameObject item) {

        if (_contents.Contains(item)) {

            _contents.Remove(item);

            return item;

        }

        return null;

    }

    public GameObject TakeType(System.Type type) {

        foreach (GameObject obj in _contents) {

            if (obj.GetComponent(type) != null) {

                _contents.Remove(obj);
                return obj;

            }

        }

        return null;
        
    }

    void Start() {

        _contents = new List<GameObject>();

    }

}