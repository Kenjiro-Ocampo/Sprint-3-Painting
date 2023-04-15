using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickControl : MonoBehaviour
{
    private Vector3 HitPosition;                // Vector var storing the hit position
    private RaycastHit hit;                     // Struct used by the raycast to retrive information
    private Color primColor;                    // Store the color of objects
    private GameObject primObject;              // Acutal object being used
    private GameObject primObjectContainer;     // Container for a chosen option not really needed but it makes it easier to understand.

    /*
        UNITY DOCS: https://docs.unity3d.com/ScriptReference/SerializeField.html

        SerializedFied is similar to a macro in C/C++
        Unity actually runs itself on C++, and even though
        Scripts are ran in C# the two need a wrapper so the
        two languages can communicate.

        UNREAL (5.1) DOCS:  https://docs.unrealengine.com/5.1/en-US/unreal-engine-uproperties/
                            https://docs.unrealengine.com/5.1/en-US/ufunctions-in-unreal-engine/

        Something similar to this is the macro used in Unreal Engine
        (UFUNCTION | UPROPERTY | UCLASS | UENUM) Like Unity, Unreal uses these to
        help define variables and structures to use within the editor, and
        any other "secondary" properties it might have.

        With this the editor can acutally see the variables within
        a C# script, and from there assignments can also be done 
    */

    /*
        As a note to whomever, the GameObjects are assigned through the editor
        and not within the script. You can assign them within script, but that is frankly
        harder to read and to iterate from.
    */

    [SerializeField] public GameObject primObject0; // Sphere
    [SerializeField] public GameObject primObject1; // Cube
    [SerializeField] public GameObject primObject2; // Cylinder
    [SerializeField] public GameObject primObject3; // The Custom shape in this case a sword

    // Using the TextMeshPro package for clearer text that is acutally anti-aliasted unlike the default 
    private TMP_Text CanvasShape; // Structure for the shape dropdown.
    private TMP_Text CanvasColor; // Structure for the color dropdown.
    [SerializeField] private TextMeshProUGUI MousePosition; // Structure for mouse position text

    /* 
        Mask for layered instaction used by the mouse. You can think of this as layers in photoshop that 
        define what is being interacted with by the player or other game mechanics.
    */

    [SerializeField] private LayerMask clickfield; 

    /*
        You could do the offsets automatically but the code would be harder to read for everyone.
    */
    [SerializeField] public int xOffset; // Offset in the x direction for the mouse position text becuae the canvas can be anywhere. 
    [SerializeField] public int yOffset; // Offset in the y direction for the mouse position text becuae the canvas can be anywhere.

    void Start()
    {
        //Defaults values
        HitPosition = -Vector3.one; // Reverse the vector in the oppsite direction | <1,1,1> -> <-1,-1,-1> | (-) is overloaded

        primColor = Color.red; // Default color used

        yOffset = 329; // Default y offset
        xOffset = 470; // Default x offset

        primObjectContainer = GameObject.CreatePrimitive(PrimitiveType.Sphere); // Default shape used
    }

    // Update is called once per frame
    void Update()
    {
        // Changes the text of the postion of the mouse 
        MousePosition.text = "Mouse Position: (" + (Input.mousePosition.x - xOffset) + ", " + (Input.mousePosition.y - yOffset) + ")";

        // UNITY DOCS: https://docs.unity3d.com/ScriptReference/Input.GetButton.html
        // The Input.GetButton("Fire1") is a virutal button defined in the editor (can be changed) and continues to return true even when the button is held down.
        if (Input.GetButton("Fire1"))
        {
            // Essentially fires a line from the camera to the postion of where the mouse points to
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Checks to see if the raycast has hit a specified layermask 
            if(Physics.Raycast(raycast, out hit, 100f, clickfield))
            {
                // Extracts the postion that the raycast has hit
                HitPosition = hit.point;

                // Debug raycast stuff
                //Debug.DrawRay(Camera.main.transform.position, HitPosition, Color.red);

                // Instances or rather "Instantiates" a new GameObject if 
                primObject = Instantiate(primObjectContainer) as GameObject;

                // Gets the rendering component of the object and the material and reassigns a new color defined from the refernece function
                // in the options menu.
                primObject.GetComponent<Renderer>().material.color = primColor; 

                // Changes the position of the instanced GameObjects to the point the user clicks
                primObject.transform.position = HitPosition;
            }
        }
        Destroy(primObject, 3); // Removes a GameObject instance from the scene after 3 seconds
    }

// The following functions below are referenced within the editor these work by running the function only choosing an option.
// When called the UI passes in the index of the chosen option. 
    public void CanvasReturner(int index)
    {
        switch (index)
        {
            case 0:
                this.primObjectContainer = primObject0;
                break;
            case 1:
                this.primObjectContainer = primObject1;
                break;
            case 2:
                this.primObjectContainer = primObject2;
                break;
            case 3:
                this.primObjectContainer = primObject3;
                break;
            default:
                this.primObjectContainer = primObject0;
                break;
        }
    }

// Same as the function above.
    public void PrimColor(int index)
    {
        switch (index)
        {
            case 0:
                this.primColor = Color.red;
                break;
            case 1:
                this.primColor = Color.green;
                break;
            case 2:
                this.primColor = Color.blue;
                break;
            default:
                this.primColor = Color.red;
                break;
        }
    }
}
