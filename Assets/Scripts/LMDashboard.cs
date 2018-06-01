using Leap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMDashboard : MonoBehaviour {

    public GameObject screen;
    public GameObject icons;
    public float iconsOffset = 1;
    private bool iconsInView = false;
    private Vector3 iconsOriginalPosition;
    private Controller c;
    private int activeMenu = 0; // 0 = no menu, 1 = menu 1, 2 = menu 2, 3 = menu 3

	// Use this for initialization
	void Start () {
        c = new Controller();
        c.Connect += this.OnServiceConnect;
        c.Device += this.OnConnect;
        c.FrameReady += this.OnFrame;
        this.iconsOriginalPosition = icons.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (this.iconsInView)
        {
            Vector3 offset = new Vector3(this.iconsOffset,0,0);
            this.icons.transform.position = Vector3.Lerp(this.icons.transform.position, this.iconsOriginalPosition + offset, 0.3f);
        }
        else
        {
            Vector3 offset = new Vector3(0, 0, 0);
            this.icons.transform.position = Vector3.Lerp(this.icons.transform.position, this.iconsOriginalPosition + offset, 0.3f);
        }
	}

    public void IconButton1Pressed()
    {
        this.MenuOpenCloseControl(1);
    }

    public void IconButton2Pressed()
    {
        this.MenuOpenCloseControl(2);
    }

    public void IconButton3Pressed()
    {
        this.MenuOpenCloseControl(3);
    }

    private void MenuOpenCloseControl(int menuNum)
    {
        if (this.activeMenu == menuNum)
        {
            this.activeMenu = 0;
            this.screen.SetActive(false);
            return;
        }
        this.activeMenu = menuNum;
        this.screen.SetActive(true);
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
        if (this.IsSwiping(false, false, args.frame) || this.IsSwiping(true, false, args.frame))
        {
            this.iconsInView = true;
        }

        if (this.IsSwiping(false, true, args.frame) || this.IsSwiping(true, true, args.frame))
        {
            this.iconsInView = false;
        }

        // Example Codes:
        /*
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
        }*/
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
