using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public Node[,] startpos;
    public Node[,] positions;
    public CreateMap[] maplist;
    public CreateMap mapinfo;
    public Slider slider;
    public GameObject cameraprop;
    private Node[,] nodes;
    public int indexvertical=0,indexhorizontal=0;
    private bool blueplane;
    public Button mainmenubtn;
    private Vector2 starttouchpos;
    private Vector2 currentpos;
    private Vector2 endttouchpos;
    private bool stoptouch = false;
     float swiperange;
     float taprange;
     public int direction;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("MapList") < maplist.Length)
            mapinfo = maplist[PlayerPrefs.GetInt("MapList", 0)];
        else
            SceneManager.LoadScene("MainMenu");
    }
    private void Start()
    {
        CreateGrid();
        Transform cubeprefab=Instantiate(mapinfo.player,new Vector3(nodes[0, 0].cellPosition.x, 0.3f, nodes[0, 0].cellPosition.z), Quaternion.identity);
        if (mapinfo.obstacle == true)
        {
            Transform obstacleprefab = Instantiate(mapinfo.obstacleprefab, new Vector3(nodes[mapinfo.x,mapinfo.z].cellPosition.x,0.3f,nodes[mapinfo.x,mapinfo.z].cellPosition.z), Quaternion.identity);
        obstacleprefab.gameObject.GetComponent<Obstacle>().grid = this;
        }
        cubeprefab.gameObject.GetComponent<Movement>().grid = this;
        slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        cameraprop = GameObject.FindGameObjectWithTag("CameraProperties");
        cameraprop.GetComponent<CinemachineVirtualCamera>().LookAt = cubeprefab;
        cameraprop.GetComponent<CinemachineVirtualCamera>().Follow = cubeprefab;
        startpos = nodes;
        startpos[indexhorizontal, indexvertical].obj.GetComponent<MeshRenderer>().material.color = Color.blue;
        startpos[indexhorizontal, indexvertical].paintindex=1;
        slider.maxValue = mapinfo.height*mapinfo.width;
        gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
        mainmenubtn = GameObject.FindGameObjectWithTag("MainMenuBtn").GetComponent<Button>();
        mainmenubtn.onClick.AddListener(MainMenuBtn);
    }
    private void Update()
    {
        Swipe();

    }
    private void CreateGrid()
    {
        nodes = new Node[mapinfo.width, mapinfo.height];
        var name = 0;
        for(int i=0;i<mapinfo.width;i++)
        {
            for(int j = 0; j < mapinfo.height; j++)
            {
                Vector3 worldPosition = new Vector3(x: i, y: 0, z: j);
                Transform obj = Instantiate(mapinfo.ground, worldPosition, Quaternion.identity);
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
    public void MainMenuBtn()
    {
        SceneManager.LoadScene("MainMenu");
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
