using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manifest
{

    public enum Category {

        FOOD,
        DRINK

    }

    public static GameObject SearchForClosest(GameObject objFrom, Category category) {

        if (_manifest.ContainsKey(category)) {

            List<GameObject> objs = _manifest[category];

            GameObject best = objs[0];

            foreach (GameObject obj in objs) {

                if (Vector3.Distance(objFrom.transform.position, obj.transform.position)
                    < Vector3.Distance(objFrom.transform.position, best.transform.position))
                {

                    best = obj;

                }

            }

            return best;

        } else {

            return null;

        }
    
    }

    private static Dictionary<Category, List<GameObject>> _manifest = new Dictionary<Category, List<GameObject>>();

    public static void Register(GameObject obj) {

        if (obj.GetComponent<Item>() != null) {

            Item item = obj.GetComponent<Item>();

            if (item.isFood) { _AddToCategoryList(obj, Category.FOOD); }
            if (item.isDrink) { _AddToCategoryList(obj, Category.DRINK); }

        }

    }

    public static void DeRegister(GameObject obj) {

        if (obj.GetComponent<Item>() != null) {

            Item item = obj.GetComponent<Item>();

            if (item.isFood) { _RemoveFromCategoryList(obj, Category.FOOD); }
            if (item.isDrink) { _RemoveFromCategoryList(obj, Category.DRINK); }

        }

    }

    private static void _EnsureListCreation(Category category) {

        if (!_manifest.ContainsKey(category)) {

            _manifest[category] = new List<GameObject>();

        }

    }

    private static void _AddToCategoryList(GameObject obj, Category category) {

        _EnsureListCreation(category);

        if (!_manifest[category].Contains(obj)) {

            _manifest[category].Add(obj);

        }

    }

    private static void _RemoveFromCategoryList(GameObject obj, Category category) {

        if (_manifest.ContainsKey(category) && _manifest[category].Contains(obj)) {

            _manifest[category].Remove(obj);

            if (_manifest[category].Count == 0) {

                _manifest.Remove(category);

            }

        }

    }

}
