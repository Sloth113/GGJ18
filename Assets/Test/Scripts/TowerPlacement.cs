using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{

    [SerializeField] private List<GameObject> towerPrefabs;
    [SerializeField] private Transform groundTransform;

    private bool towerNotPlaced = false;

	// Use this for initialization
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad1) && towerPrefabs[0] != null)
        {
            towerNotPlaced = true;
        }

        if (towerNotPlaced == true)
        {
            Vector3 mouseCast = towerAtMousePosition();
            if (Input.GetMouseButtonDown(0))
            {
                mouseCast.y += (towerPrefabs[0].GetComponent<Renderer>().bounds.size.y/2);
                Instantiate(towerPrefabs[0], mouseCast, Quaternion.identity);
                towerNotPlaced = false;
            }
        }
    }

    // Function to check the position 
    private Vector3 towerAtMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
        Plane groundPlane = new Plane(Vector3.up, groundTransform.position);

        float rayDistance = 0;

        groundPlane.Raycast(mouseRay, out rayDistance);
        Vector3 castPoint = mouseRay.GetPoint(rayDistance);

        return castPoint;
    }

}
