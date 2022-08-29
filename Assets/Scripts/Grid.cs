using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    Movement movement;
    public Transform gridPrefab;
    public Transform cube,obstacle;
    public Node[,] startpos;
    public Node[,] positions;
    public GameObject Cam;
    public Slider slider;
    [SerializeField] public int height;
    [SerializeField] public int width;
    private Node[,] nodes;
    public int indexvertical=0,indexhorizontal=0;
    private bool blueplane;

    private Vector2 starttouchpos;
    private Vector2 currentpos;
    private Vector2 endttouchpos;
    private bool stoptouch = false;
    public float swiperange;
    public float taprange;
    public int direction;

    private void Start()
    {
        CreateGrid();
        Transform cubeprefab=Instantiate(cube,new Vector3(nodes[0, 0].cellPosition.x, 0.3f, nodes[0, 0].cellPosition.z), Quaternion.identity);
        if(SceneManager.GetActiveScene().buildIndex>1)
        Instantiate(obstacle,obstacle.gameObject.transform.position, Quaternion.identity);
        cubeprefab.gameObject.GetComponent<Movement>().grid = this;
        Cam.GetComponent<CinemachineVirtualCamera>().LookAt = cubeprefab;
        Cam.GetComponent<CinemachineVirtualCamera>().Follow = cubeprefab;
        startpos = nodes;
        startpos[indexhorizontal, indexvertical].obj.GetComponent<MeshRenderer>().material.color = Color.blue;
        startpos[indexhorizontal, indexvertical].paintindex=1;
        slider.maxValue = height *width;
        gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");        
    }
    private void Update()
    {
        Swipe();

    }
    private void CreateGrid()
    {
        nodes = new Node[width, height];
        var name = 0;
        for(int i=0;i<width;i++)
        {
            for(int j = 0; j < height; j++)
            {
                Vector3 worldPosition = new Vector3(x: i, y: 0, z: j);
                Transform obj = Instantiate(gridPrefab, worldPosition, Quaternion.identity);
                obj.name = "Cell" + name;
                nodes[i, j] = new Node(isPlaceable: true, worldPosition, obj,0,false);
                name++;
            }
        }
    }
    public void PaintGrid()
    {
        startpos[indexhorizontal, indexvertical].obj.GetComponent<MeshRenderer>().material.color = Color.blue;
        foreach (var item in startpos)
        {
            if(item.paintindex == 5)
            {
                item.paintindex = 1;
            }
        }
        startpos[indexhorizontal, indexvertical].paintindex = 5;
    }

    public void Swipe()
    {
        if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            starttouchpos = Input.GetTouch(0).position;
        }
        if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentpos = Input.GetTouch(0).position;
            Vector2 distance = currentpos - starttouchpos;
        if (!stoptouch)
        {
                if (distance.x < -swiperange)
                {
                    direction = 0;
                    stoptouch = true;
                }
                else if (distance.x > swiperange)
                {
                    direction = 1;
                    stoptouch = true;
                }
                else if (distance.y > swiperange)
                {

                    direction = 2;
                    stoptouch = true;
                }
                else if (distance.y < -swiperange)
                {
                    direction = 3;
                    stoptouch = true;
                }
        }
        }
        if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stoptouch = false;
            endttouchpos = Input.GetTouch(0).position;
            Vector2 distance = endttouchpos - starttouchpos;
            if(Mathf.Abs(distance.x)<taprange&& Mathf.Abs(distance.y) < taprange)
            {

            }
        }
    }
}

public class Node
{
    public bool isPlaceable;
    public Vector3 cellPosition;
    public Transform obj;
    public int paintindex;
    public bool drop;
    public Node(bool isPlaceable,Vector3 cellPosition,Transform obj,int paintindex,bool drop)
    {
        this.isPlaceable = isPlaceable;
        this.cellPosition = cellPosition;
        this.obj = obj;
        this.paintindex = paintindex;
        this.drop = drop;
    }
    public Node()
    {
        
    }
}
