using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
 
 
public class Starfield : MonoBehaviour
{
    public int MaxStars = 100;
    public float StarSize = 1f;
    public float StarSizeRange = 0.1f;
    public float FieldWidth = 20f;
    public float FieldHeight = 25f;
    public bool Colorize = false;
	
    float xOffset;
    float yOffset;

    public GameObject prefabParticle;
    private GameObject[] stars;
    Transform theCamera;
 
    void Start () 
    {
        theCamera = Camera.main.transform;	
    }
	
    void Awake ()
    {
        stars = new GameObject[MaxStars];
        for (int i = 0; i < MaxStars; i++)
        {
            stars[i] = Instantiate(prefabParticle);
        }
 
        xOffset = FieldWidth * 0.5f;																										// Offset the coordinates to distribute the spread
        yOffset = FieldHeight * 0.5f;																										// around the object's center
	
        for ( int i=0; i<MaxStars; i++ )
        {
            float randSize = Random.Range( 1f-StarSizeRange/2, 1f+StarSizeRange/2);						// Randomize star size within parameters
            float randRotation = Random.Range(0f, 360f);						// Randomize star size within parameters
            stars[ i ].transform.position = GetRandomInRectangle( FieldWidth, FieldHeight ) + transform.position;
            stars[ i ].transform.localScale = new Vector3(StarSize * randSize, StarSize * randSize, 1f);
            stars[ i ].transform.Rotate(new Vector3(0, 0, 1), randRotation);
        }
    }
 
 
    // GetRandomInRectangle
    //----------------------------------------------------------
    // Get a random value within a certain rectangle area
    //
    Vector3 GetRandomInRectangle ( float width, float height )
    {
        float x = Random.Range( 0, width );
        float y = Random.Range( 0, height );
        return new Vector3 ( x - xOffset , y - yOffset, 0 );
    }
    
    void Update ()
    {
        for ( int i=0; i<MaxStars; i++ )
        { 
            Vector3 pos = stars[i].transform.position;
            if (pos.x < theCamera.position.x - xOffset)
            {
                pos.x += FieldWidth;
            }
            else if (pos.x > theCamera.position.x + xOffset)
            {
                pos.x -= FieldWidth;
            }
 
            if (pos.y < theCamera.position.y - yOffset)
            {
                pos.y += FieldHeight;
            }
            else if (pos.y > theCamera.position.y + yOffset)
            {
                pos.y -= FieldHeight;
            }
            stars[i].transform.position = pos;
        }
    }
}