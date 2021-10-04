using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

//actual scale: x: .02389 y: 0.5 z: .02389
public class FlagPoleScript : MonoBehaviour {
    Controller controller;
    bool touchingPalm = false;
    Vector3 initPos;
    Quaternion initRot;
	// Use this for initialization
	void Start () {
        controller = new Controller();
        initPos = gameObject.transform.position;
        initRot = gameObject.transform.rotation;
        Debug.Log("TEst");
	}
	
	// Update is called once per frame
	void Update () {
        Frame frame = controller.Frame();
        List<Hand> handsInFrame = frame.Hands;
        if (Input.GetKeyDown("g"))
        {
            if (gameObject.GetComponent<Rigidbody>().useGravity)
                gameObject.GetComponent<Rigidbody>().useGravity = false;
            else
                gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
        if (Input.GetKeyDown("space"))
        {
            foreach (Hand h in handsInFrame)
                Debug.Log("X angle:" + (h.PalmNormal.Pitch * (180/Mathf.PI)) + " Y angle:" + (h.PalmNormal.Yaw * (180 / Mathf.PI)) 
                    + " Z angle:" + (h.PalmNormal.Roll * (180 / Mathf.PI)));
        }
        if (Input.GetKeyDown("r"))
        {
            gameObject.GetComponent<ConfigurableJoint>().connectedBody = null;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0);
            gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0);
            gameObject.transform.position = initPos;
            gameObject.transform.rotation = new Quaternion(initRot.x, initRot.y, initRot.z, initRot.w);
        }
        Debug.Log(gameObject.GetComponent<ConfigurableJoint>().connectedBody);
        foreach (Hand h in handsInFrame)
        {
            GameObject handGameObject;
            if (h.IsLeft)
                handGameObject = GameObject.Find("RigidRoundHand_L");
            else
                handGameObject = GameObject.Find("RigidRoundHand_R");
            if (InGrasp(h))
            {
                //figure out a way to make the flag pole be flush with palm
                //maybe change the rotatin of the flag to be the same as the rotation of the palm?
                
                if (gameObject.GetComponent<ConfigurableJoint>().connectedBody != handGameObject.GetComponent<Rigidbody>())
                //if (handGameObject.GetComponent<ConfigurableJoint>().connectedBody == null)
                    Debug.Log("Attached");
                //gameObject.GetComponent<ConfigurableJoint>().connectedBody = handGameObject.transform.GetChild(5).gameObject.GetComponent<Rigidbody>();
                //gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //handGameObject.GetComponent<ConfigurableJoint>().connectedBody = gameObject.GetComponent<Rigidbody>();
                gameObject.GetComponent<ConfigurableJoint>().connectedBody = handGameObject.GetComponent<Rigidbody>();
            }
            else
            {
                if (gameObject.GetComponent<ConfigurableJoint>().connectedBody != null)
                //if (handGameObject.GetComponent<ConfigurableJoint>().connectedBody != null)
                    Debug.Log("Detach");
                gameObject.GetComponent<ConfigurableJoint>().connectedBody = null;
                //gameObject.GetComponent<Rigidbody>().isKinematic = false;
                //handGameObject.GetComponent<ConfigurableJoint>().connectedBody = null;
                //Debug.Log("Detach");
            }
        }
	}

	void OnCollisionEnter(Collision col)
	{
        if (col.gameObject.name == "palm")
        {
            //the palm is facing up when normal vector z is ~180 , down when ~0
            touchingPalm = true;
        }
        if (col.gameObject.name == "Ground")
            Debug.Log("Hit ground");
	}
    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name == "palm")
            touchingPalm = false;
    }

    bool InGrasp(Hand hand) //checks to see if flag is inside of a hand's grasp ex: if hand is making a circle, is the flag inside of the circle
	{
        /*maybe do it so based on the direction of the finger, you check if the flag pole's position's x or y or z is less than or more than that of the finger
		so if like the palm is facing up the index finger is also facing up, but pointing away from person, so you check to see if the flag's y pos is greater than
		the fingers y pos to see if flag is above that finger, 
		will have to check each bone in each finger in each hand...ugh. i guess i'll start with thumb
        time to start with the thumb.
        there is a grabStrength and a grabAngle method in the Hand class, I might be able to use those
        to get the flag's position, get gameObject's transform component
        */
        if (hand.GrabAngle < Mathf.PI)// / 2.0) //calculate actual angle threshold later, 
            return false;
        if (!touchingPalm) //maybe change it so that it's not that it's not touching but if its within a certain radius
            return false;
            //get the hands normal vector and use that with the flag position to see if its actually inside the palm
		return true;
	}
	bool TouchingFinger(Finger finger)
	{
        //use palm's normal direction vector to find if the flag's position needs to be less than or greater than the finger's position
        //aka does t need to be above or below the finger
        //if palm normal vector is pointing right, that means the flag has to be somewhere to the right of the hand/finger for it to return true

        //base case: if its not colliding with said finger
        return false;
	}

    void IgnoreFingers(Hand h, bool cond)
    {
        //Physics.IgnoreCollision()
        
    }
}