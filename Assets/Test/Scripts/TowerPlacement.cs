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
            towerAtMousePosition(towerPrefabs[0]);
        }
    }

    // Function to check the position 
    private void towerAtMousePosition(GameObject towerPrefab)
    {
        Vector3 mousePos = Input.mousePosition;
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
        Plane groundPlane = new Plane(Vector3.up, groundTransform.position);
        RaycastHit hit;

        //float rayDistance = 0;

        //groundPlane.Raycast(mouseRay, out rayDistance);

        if (Physics.SphereCast(mouseRay, 5, out hit))
            if (Input.GetMouseButtonDown(0) && hit.transform.tag != "Untagged")
            {
                Vector3 castPoint = hit.point;

                castPoint.y += (towerPrefab.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(towerPrefab, castPoint, Quaternion.identity);
                towerNotPlaced = false;
            }

    }

}
