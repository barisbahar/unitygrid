using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class SwipeMenuLevels : MonoBehaviour
{
    public GameObject scrollbar;
    public VideoPlayer videoPlayer;
    public VideoClip level1,level2,level3;
    public RenderTexture rendertexture;
    public RawImage rawImage;

    float scroll_pos = 0;
    float[] pos;
    int a;

    private void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.5f, 1.5f), 0.1f);
                for (a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        transform.GetChild(a).GetComponent<Button>().interactable = false;
                        
                    }
                    else
                    {
                        if (transform.GetChild(a).GetComponent<Image>().maskable == true)
                        {
                            transform.GetChild(a).GetComponent<Button>().interactable = true;
                        }
                        
                        if (a == 0)
                        {
                            rawImage.GetComponent<RawImage>().texture = rendertexture;
                            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
                        }
                        else if (a == 1)
                        {

                            rawImage.GetComponent<RawImage>().texture = rendertexture;
                        }
                        else if (a == 2)
                        {

                            rawImage.GetComponent<RawImage>().texture = rendertexture;
                        }
                        else
                        {
                            ResetScreen();
                        }
                    }
                }
            }
        }
    }
    public void ResetScreen()
    {
        rawImage.GetComponent<RawImage>().texture = null;
    }
    public void Right()
    {
        scroll_pos = scrollbar.GetComponent<Scrollbar>().value += 0.1f;
    }
    public void Left()
    {
        scroll_pos = scrollbar.GetComponent<Scrollbar>().value -= 0.1f;
    }
}
