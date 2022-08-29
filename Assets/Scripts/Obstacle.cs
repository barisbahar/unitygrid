using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    private int rotate,positionx=0;
    public int speed;
    public bool positionswitch=false;
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            rotate += 2;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, rotate * Time.deltaTime * speed, 0));
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (positionx > 20 && gameObject.transform.position.x>20)
            {
                positionswitch = true;
            }
            else if (positionx < 0 &&gameObject.transform.position.x<0)
            {
                positionswitch = false;
            }
            if (positionswitch == false)
            {
                positionx++;
            }
            else
            {
                positionx--;
            }
            gameObject.transform.position = new Vector3(positionx*Time.deltaTime*3,transform.position.y , transform.position.z);
        }
        
    }
}
