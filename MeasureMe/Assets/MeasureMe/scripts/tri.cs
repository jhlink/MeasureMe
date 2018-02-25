﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class tri : MonoBehaviour
{


    public Camera cam;

    public GameObject aimer;

    public float centerDist = .5f;
    public float maxDist = 30f;

    public LayerMask planeLayer;
    public LayerMask deleteMask;

    public ParticleSystem touchParticle;
    public ParticleSystem particle;




    public List<GameObject> placeOjects;


    public GameObject button;
    private GameObject newOb;
    public Text text;



    public GameObject objectToCreate;

    private Image currImage;
    private Color activeColor;
    private Color inactiveColor;


    public float distInInch;
    public float distInMeters;

    public GameObject lineOb;
    private LineRenderer lr;

    private bool isDeleting = false;
    private bool isPlacing = false;
    private bool searchingForParticles = false;


    public string distanceString;

    // public LineRenderer LLR;



    Vector3 hitPoint;
    Vector3 hitPointCenter;
    Vector3 center;

    Vector3 startPoint;




    // Use this for initialization
    void Start()
    {
        aimer.transform.position = center;
        currImage = button.GetComponent<Image>();
        activeColor = new Color(0, 255, 255, 255);
        inactiveColor = new Color(150, 150, 150, 150);
        lr = lineOb.GetComponent<LineRenderer>();



    }

    // Update is called once per frame
    void Update()
    {
       /* GameObject[] particlesInScene = GameObject.FindGameObjectsWithTag("particle");

        if(searchingForParticles == true && particlesInScene.Length > 0){
            for (int i = 0; i < particlesInScene.Length; i++)
            {
                Destroy(particlesInScene[i]);
            }
        }
        */

        center = new Vector3(Screen.width / 2, Screen.height / 2,0);
        aimer.transform.position = center;
        if (distInInch != null)
        {

            distanceString = distInInch.ToString();
            text.text = distanceString + " inches";
        }

        if (placeOjects.Count == 1 && isDeleting != true)
        {

            lr.enabled = true;
            lr.startWidth = 0.005f;
            lr.SetPosition(0, placeOjects[0].transform.position);//placeOjects[0].GetComponent<LineRenderer>();
            lr.SetPosition(1, hitPointCenter);//placeOjects[0].GetComponent<LineRenderer>();
            distInMeters = (Vector3.Distance(placeOjects[0].transform.position, hitPoint));
            distInInch = distInMeters * 39.3701f;
        }


        else if (placeOjects.Count == 2 && isDeleting != true)
        {

            lr.enabled = true;
            lr.startWidth = 0.005f;
            lr.SetPosition(0, placeOjects[0].transform.position);//placeOjects[0].GetComponent<LineRenderer>();
            lr.SetPosition(1, placeOjects[1].transform.position);//placeOjects[0].GetComponent<LineRenderer>();
            distInMeters = (Vector3.Distance(placeOjects[0].transform.position, (placeOjects[1].transform.position)));
            distInInch = distInMeters * 39.3701f;
        }

        else
        {
            lr.enabled = false;
        }


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray rayCenter = Camera.main.ScreenPointToRay(center);



        RaycastHit hit;
        RaycastHit hitCenter;


        //we'll try to hit one of the plane collider gameobjects that were generated by the plugin
        //effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent

        //if(Input.mousePosition = 




         if (Physics.Raycast(ray, out hit, maxDist, planeLayer))
        {
            hitPoint = hit.point;
            if (Input.GetMouseButtonUp(0))
            {
                if (isDeleting == false && isPlacing == false && placeOjects.Count < 2)
                {
                    createObject(hitPoint);
                    particle = Instantiate(touchParticle, hit.point, Quaternion.identity);
                    StartCoroutine(delayKillingParticles());
                    particle.tag = "particle";
                }
            }
        }


        if (Physics.Raycast(rayCenter, out hitCenter, maxDist, planeLayer))

        {
            //lr = cam.GetComponent<LineRenderer>();
            //lr.enabled = true;
            //lr.SetPosition(0, hit.point);
            //currImage.color = activeColor;
            //Debug.Log("you are raycasting onto 'planeLayer'");
            hitPointCenter = hitCenter.point;
            //button.SetActive(true);
            //hitPoint = hit.point;

            /* if (Input.GetMouseButtonUp(0))
             {

                 createObject(hitPoint);
                 if (Input.touchCount == 0){
                     startPoint = hit.point;
                 }*/
        }


        else
        {
            currImage.color = inactiveColor;
        }


    }


    void createObject(Vector3 point)
    {
        StartCoroutine(placeObject(point));
    }

    /*
    public void connectDots()
    {5r5
        Debug.Log("dots connecting!");
        var points = new Vector3[placeOjects.Count];
        for (int i = 0; i < placeOjects.Count; i++)
        {
            LineRenderer lr = placeOjects[0].GetComponent<LineRenderer>();
            lr.SetPositions(points);
            lr.startWidth = .005f;
            lr.startColor = Color.green;
        }
    }
*/

    public void placeAtCenter()
    {
        if (isDeleting == false && isPlacing == false && placeOjects.Count < 2)
        {
            createObject(hitPointCenter);
        }
    }


    public void deleteObjs()
    {
        Debug.Log("deleted");
        StartCoroutine(deleteThoseObs());
    }


    IEnumerator placeObject(Vector3 point)
    {
        isPlacing = true;
        newOb = Instantiate(objectToCreate, point, Quaternion.identity);
        newOb.tag = "dot";
        placeOjects.Add(newOb);
        yield return new WaitForSeconds(.5f);
        isPlacing = false;


    }


    IEnumerator deleteThoseObs()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("dot");
        isDeleting = true;
        for (int i = 0; i < placeOjects.Count; i++)
        {
            placeOjects.Remove(placeOjects[i]);
            Destroy(markers[i]);
        }
        yield return new WaitForSeconds(1f);
        isDeleting = false;
    }

    IEnumerator delayKillingParticles()
    {
        searchingForParticles = false;
        yield return new WaitForSeconds(2f);
        searchingForParticles = true;
    }



}
