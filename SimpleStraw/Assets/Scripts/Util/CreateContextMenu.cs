using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ContextMenu
{

    public class OptionReference : MonoBehaviour {
        public Option option;
    }
    
    ///<summary>Represents a single option within a menu. Has a click event & name.</summary>
    public class Option {

        //The prefab to use when creating the GameObject version of the Option
        private static GameObject optionPrefab;

        //What happens when the option is clicked
        public readonly UnityAction OnClick;

        //The name of the option, displayed to the player
        public readonly string name;

        ///<summary>Creates an option with the given name and click event.</summary>
        ///<param name="name">The name of the option, which is what the player will see</param>
        ///<param name="OnClick">What happens when this option is clicked. Use () => {  } lambda for this.</param>
        public Option(string name, UnityAction OnClick) {
            this.name = name;
            this.OnClick = OnClick;
        }

        ///<summary>Builds the actual object (button, text) from this option.</summary>
        ///<returns>Returns a GameObject holding in its children the button and text. Needs to be put on a canvas to work.</returns>
        public GameObject BuildObject(Vector3 position) {

            if (optionPrefab == null) {

                optionPrefab = (GameObject)Resources.Load("Option");

            }

            GameObject optionObj = GameObject.Instantiate(optionPrefab, position, Quaternion.identity);

            //Set the button text as the name
            Text txt = optionObj.GetComponentInChildren<Text>();
            txt.text = name;

            /*
                NOTE:
                The button's onClick event is not set here. It is set later when building a full
                context menu so the menu can be destroyed when one option is clicked.
            */

            return optionObj;

        }

    }

    public class ContextMenuBuilder {

        private static GameObject contextMenuPrefab;

        public static GameObject BuildContextMenu(Vector3 center, params Option[] options) {

            if (contextMenuPrefab == null) {

                contextMenuPrefab = (GameObject)Resources.Load("Context Menu");

            }

            GameObject menu = GameObject.Instantiate(contextMenuPrefab, center, Quaternion.identity);

            Vector3 trueCenter = new Vector3(center.x, center.y - (options.Length / 2));

            for (int i = 0; i < options.Length; i++) {

                Vector3 optionPosition = new Vector3(trueCenter.x, trueCenter.y + i);

                GameObject optionObj = options[i].BuildObject(optionPosition);
                optionObj.AddComponent<OptionReference>();
                optionObj.GetComponent<OptionReference>().option = options[i];

                UnityAction onClick = () => { optionObj.GetComponent<OptionReference>().option.OnClick.Invoke(); GameObject.Destroy(menu); };

                optionObj.GetComponentInChildren<Button>().onClick.AddListener(onClick);

                optionObj.transform.SetParent(menu.GetComponentInChildren<Canvas>().gameObject.transform);

            }

            return menu;

        }

    } 

}
