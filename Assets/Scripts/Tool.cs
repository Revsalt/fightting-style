using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : Interactable
{
    public AnimatableObjects myAnimatableObject;
    [Space]
    public bool isPosX; 
    public bool isPosY; 
    public bool isRot;

    public override void ToggleSelect()
    {
        base.ToggleSelect();
    }

    float z = 0;
    float _z = 0;
    float x = 0;
    float _x = 0;
    float y = 0;
    float _y = 0;
    private void Update()
    {
        if (isRot)
            Rotation();
        if (isPosX)
            PositionX();
        if (isPosY)
            PositionY();
    }

    void PositionX()
    {
        Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                x = myAnimatableObject.transform.position.x;
                _x = mouseScreenPosition.x;
            }

            if (Input.GetMouseButton(0))
            {
                myAnimatableObject.transform.position = new Vector3((x + -(_x - mouseScreenPosition.x)), myAnimatableObject.transform.position.y, myAnimatableObject.transform.position.z);
                transform.parent.parent.transform.position = new Vector3(myAnimatableObject.transform.position.x, myAnimatableObject.transform.position.y, transform.parent.parent.transform.position.z);
            }
        }
    }

    void PositionY()
    {
        Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                y = myAnimatableObject.transform.position.y;
                _y = mouseScreenPosition.y;
            }

            if (Input.GetMouseButton(0))
            {
                myAnimatableObject.transform.position = new Vector3(myAnimatableObject.transform.position.x, (y + -(_y - mouseScreenPosition.y)), myAnimatableObject.transform.position.z);
                transform.parent.parent.transform.position = new Vector3(myAnimatableObject.transform.position.x, myAnimatableObject.transform.position.y, transform.parent.parent.transform.position.z);
            }
        }
    }

    void Rotation()
    {
        Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float AngleRad = Mathf.Atan2(mouseScreenPosition.y - transform.position.y, mouseScreenPosition.x - transform.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;

        if (isSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                z = AngleDeg;
                _z = myAnimatableObject.transform.eulerAngles.z;
            }

            if (Input.GetMouseButton(0))
            {
                transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
                myAnimatableObject.transform.rotation = Quaternion.Euler(0, 0, _z + (AngleDeg - z));
            }
        }
    }
}
