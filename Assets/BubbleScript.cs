using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class BubbleScript : MonoBehaviour
{
    public GameObject parent;
    public float life;
    public string fullText;
    private int textLoc;
    private StringBuilder curText;
    private VoiceBox voiceBox;

    //Components
    private Text bubbleText;
    private Image bubbleImage;

    // Start is called before the first frame update
    void Start()
    {
        curText = new StringBuilder();
        voiceBox = parent.GetComponent<VoiceBox>();
        textLoc = 0;
        bubbleImage = gameObject.GetComponentInChildren<Image>();
        bubbleText = gameObject.GetComponentInChildren<Image>().gameObject.GetComponentInChildren<Text>();
        bubbleText.text = fullText;
        voiceBox.SayWord(fullText);
    }

    private float delay = .04f;
    private float curDelay = 0;

    // Update is called once per frame
    void Update()
    {

        //Billboard
        //transform.LookAt(transform.position - Camera.main.transform.position);
        transform.rotation = Camera.main.transform.rotation;

        //Death
        life -= Time.deltaTime;
        if (life <= 0 && bubbleText.color.a <= 0) {
            Destroy(gameObject);
        } else if (life <= 0) {
            bubbleText.color = new Color(bubbleText.color.r,bubbleText.color.g,bubbleText.color.b,bubbleText.color.a - Time.deltaTime);
        }

    }
}
