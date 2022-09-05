using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float rollspeed;
    public bool ismoving;
    private bool isrotate;
    public int redplane, blueplane, otherplane;
    public Grid grid;
    public int redCount;
    public float x, z;
    public int nextsceneload;
    private int levelindex;

    private void Start()
    {
        for (int i = 0; i < grid.maplist.Length; i++)
        {
            if (grid.mapinfo == grid.maplist[i])
            {
                levelindex = i;
            }
        }
        nextsceneload = levelindex + 2;
    }

    private void Update()
    {

        if (ismoving) return;
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) ||grid.direction==0) && grid.startpos[grid.indexhorizontal, grid.indexvertical].cellPosition.x > 0)
        {
            if (grid.startpos[grid.indexhorizontal-1, grid.indexvertical].drop == false)
            {
                grid.indexhorizontal--;
                Assemble(Vector3.left);
                grid.direction = -1;
            }

        }
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)|| grid.direction==1) && ismoving == false && grid.startpos[grid.indexhorizontal, grid.indexvertical].cellPosition.x < grid.mapinfo.height - 1)
        {
            if (grid.startpos[grid.indexhorizontal+1, grid.indexvertical].drop == false)
            {
                grid.indexhorizontal++;
                Assemble(Vector3.right);
                grid.direction = -1;

            }

        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)||grid.direction==2) && ismoving == false && grid.startpos[grid.indexhorizontal, grid.indexvertical].cellPosition.z < grid.mapinfo.width - 1)
        {
            if (grid.startpos[grid.indexhorizontal, grid.indexvertical+1].drop == false)
            {
                grid.indexvertical++;
                Assemble(Vector3.forward);
                grid.direction = -1;

            }

        }
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)||grid.direction==3) && ismoving == false && grid.startpos[grid.indexhorizontal, grid.indexvertical].cellPosition.z > 0)
        {
            if (grid.startpos[grid.indexhorizontal, grid.indexvertical-1].drop == false)
            {
                grid.indexvertical--;
                Assemble(Vector3.back);
                grid.direction = -1;

            }

        }
        if (grid.slider.value >= grid.mapinfo.levelcount)   
        {
            if (grid.mapinfo.levelComplete ==0)
            {
                grid.mapinfo.levelComplete = 1;
                PlayerPrefs.SetInt("MapList", PlayerPrefs.GetInt("MapList") + 1);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            if (nextsceneload > PlayerPrefs.GetInt("LevelAt"))
            {
                PlayerPrefs.SetInt("LevelAt", nextsceneload);
            }
        }

        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 0.5f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        redCount = 0;
        otherplane=0;
        ismoving = true;
        for (int i = 0; i < (90 / rollspeed); i++)
        {
            transform.RotateAround(anchor, axis, rollspeed);
            yield return new WaitForSeconds(0.01f);
        }
        ismoving = false;
        grid.PaintGrid();
        foreach (var item in grid.startpos)
        {
            if (item.paintindex == 2)
            {
                item.paintindex = 0;
            }
            else if(item.paintindex == 0&&item.drop==false)
            {
                x = item.cellPosition.x;
                z = item.cellPosition.z;
            }
        }
        CheckPaint((int)x ,(int) z);
        foreach (var item in grid.startpos)
        {
            if (item.paintindex == 0)
            {
                otherplane++;
            }
        }
        if(otherplane>0 && redCount > 0)
        {
            if (otherplane < redCount)
            {
                foreach (var item in grid.startpos)
                {
                    if (item.paintindex == 0||item.paintindex==1)
                    {
                        item.obj.GetComponent<MeshRenderer>().material.color = Color.blue;
                        item.paintindex = 1;
                        item.obj.GetComponent<Rigidbody>().useGravity = true;
                        item.obj.GetComponent<Rigidbody>().isKinematic = false;
                        if (item.drop == false)
                        grid.slider.value+=1;
                        item.drop = true;
                    }
                }
            }
            else if (redCount < otherplane)
            {
                foreach (var item in grid.startpos)
                {
                    if (item.paintindex == 2|| item.paintindex==1)
                    {
                        item.obj.GetComponent<MeshRenderer>().material.color = Color.blue;
                        item.paintindex = 1;
                        item.obj.GetComponent<Rigidbody>().useGravity = true;
                        item.obj.GetComponent<Rigidbody>().isKinematic = false;
                        if(item.drop==false)
                        grid.slider.value+=1;
                        item.drop = true;                 
                    }
                }
            }
            else
            { }
        }
    }
    public void CheckPaint(int horizontal, int vertical)
    {

        if (horizontal < 0 || horizontal > grid.mapinfo.width - 1 || vertical < 0 || vertical > grid.mapinfo.height - 1)
        {
            return;
        }
        else
        {
            if (grid.startpos[horizontal, vertical].paintindex == 0)
            {
                grid.startpos[horizontal, vertical].paintindex = 2;
                redCount++;
                CheckPaint(horizontal + 1, vertical);
                CheckPaint(horizontal - 1, vertical);
                CheckPaint(horizontal, vertical + 1);
                CheckPaint(horizontal, vertical - 1);
            }
            else
            {
                return;
            }
        }
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
