using Leap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMDashboard : MonoBehaviour {

    Controller c;

	// Use this for initialization
	void Start () {
        c = new Controller();
        c.Connect += this.OnServiceConnect;
        c.Device += this.OnConnect;
        c.FrameReady += this.OnFrame;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnServiceConnect(object sender, ConnectionEventArgs args)
    {
        print("Service Connected");
    }

    public void OnConnect(object sender, DeviceEventArgs args)
    {
        print("Connected");
    }

    public void OnFrame(object sender, FrameEventArgs args)
    {
        // right hand
        if (this.IsSwiping(false, true, args.frame))
        {
            print("right hand swipe left");
        }
        else if (this.IsSwiping(false, false, args.frame))
        {
            print("right hand swipe right");
        }

        // left hand
        if (this.IsSwiping(true, true, args.frame))
        {
            print("left hand swipe left");
        }
        else if (this.IsSwiping(true, false, args.frame))
        {
            print("left hand swipe right");
        }
    }

    private bool IsSwiping(bool handIsLeft, bool directionIsLeft, Frame frame)
    {
        String sDirection = String.Empty;
        if (frame.Hands.Count == 0) { return false; }
        Hand hand = null;
        foreach (Hand h in frame.Hands) 
        {
            if (handIsLeft && h.IsLeft)
            {
                hand = h;
            }
            if (!handIsLeft && !h.IsLeft)
            {
                hand = h;
            }
        }
        if (hand == null) { return false; }
        List<Finger> fingerList = hand.Fingers;
        if (Math.Abs(hand.PalmVelocity.x) < 1000)
        {
            return false;
        }
        else
        {
            if (hand.PalmVelocity.x > 0) { sDirection = "Right"; }
            else { sDirection = "Left"; }
        }
        int numOfExtendedFingers = 0;
        foreach (Finger f in fingerList) {
            if (f.IsExtended)
            {
                numOfExtendedFingers++;
            }
        }
        if (numOfExtendedFingers < 4) { return false; }

        if (sDirection.Equals("Right"))
        {
            if (directionIsLeft)
            {
                return false;
            }
            return true; // right
        }
        else if (sDirection.Equals("Left"))
        {
            if (directionIsLeft)
            {
                return true;
            }
            return false; // right
        }

        return false;
    }
}
