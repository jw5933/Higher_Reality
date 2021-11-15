using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    //  time to move one unit
    [Range(1f, 5f)]
    [SerializeField] private float playerSpeed = 3f;

    // click indicator
    Cursor cursor;
    // cursor AnimationController
    //private Animator cursorAnimController;

    // pathfinding fields
    private Clickable[] clickables;
    PathFinder pathfinder;
    Graph graph;
    private Node currNode;
    private Node nextNode;

    // flags
    private bool isMoving;
    private bool isControlEnabled;

    [SerializeField]private Animator playerAnim;

    //rune and interactables
    private Rune myRune;
    [SerializeField] private Transform runeTransform;
    [SerializeField] private Renderer myRenderer;
    private AudioManager am;

    // get-sets
    public Vector3 runePos{get{return runeTransform.position;}}
    public Node node{get{return currNode;}}
    public Rune rune{get{return myRune;} set{myRune = value;}}
    public bool checkIsMoving{get{return isMoving;}}

    public Material mat{set{myRenderer.material = value;}}
    public bool setcontrolEnabled{set{isControlEnabled = value;}}
    private void Awake()
    {
        //  initialize fields
        clickables = FindObjectsOfType<Clickable>();
        pathfinder = FindObjectOfType<PathFinder>();
        graph = FindObjectOfType<Graph>();
        cursor = FindObjectOfType<Cursor>();
        am = FindObjectOfType<AudioManager>();
        // isControlEnabled = true;
        isMoving = false;
    }

    private void Start()
    {
        // always start on a Node
        snapToNearestNode();

        // automatically set the Graph's StartNode 
        if (pathfinder != null)
        {
            currNode = graph.findClosestNodeAt(transform.position);
            nextNode = currNode;
        }

        //listen to all clickEvents
        foreach (Clickable c in clickables)
        {
            c.clickAction += OnClick;
        }
    }

    private void OnDisable()
    {
        // unsubscribe from clickEvents when disabled
        foreach (Clickable c in clickables)
        {
            c.clickAction -= OnClick;
        }
    }

    private void OnClick(Clickable clickable, Vector3 position){
        // Debug.Log("Should be handling click");
        if (!isControlEnabled || clickable == null || pathfinder == null){
            return;
        }

        // if we are already moving and we click again, stop all previous Animation/motion
        if (isMoving){
            StopAllCoroutines();
        }

        // find the best path to the any Nodes under the Clickable; gives the user some flexibility
        List<Node> newPath = pathfinder.findPath(currNode, clickable.childNode);
        // Debug.Log("Curr node: " + currNode.name + " End node: " + clickable.childNode.name);

        // if we have a valid path, follow it
        if (newPath.Count > 0){
            // show a marker for the mouse click
            if (cursor != null){
                am.playClickerSound();
                cursor.showCursor(position, true);
            }
            StartCoroutine(followPathRoutine(newPath));
        }
        else{
            // otherwise, invalid path, stop movement
            isMoving = false;
            // if (cursor != null)
            //     cursor.showCursor(clickable.childNode.transform.position, false);
            updateAnimation(false);
        }
    }

    private IEnumerator followPathRoutine(List<Node> path){
        // start moving
        isMoving = true;

        if (path == null || path.Count <= 0)
        {
            Debug.Log("PLAYERCONTROLLER followPathRoutine: invalid path");
        }
        else
        {
            updateAnimation(true);

            // loop through all Nodes
            for (int i = 0; i < path.Count; i++)
            {
                nextNode = path[i];
                // aim at the Node after that to minimize flipping
                int nextAimIndex = Mathf.Clamp(i + 1, 0, path.Count - 1);
                Node aimNode = path[nextAimIndex];
                faceNextPosition(transform.position, aimNode.transform.position);

                // move to the next Node
                yield return StartCoroutine(moveToRoutine(transform.position, nextNode));
            }
        }
        isMoving = false;
        updateAnimation(false);

    }

    //movement is based off of distance between a and b (removes stuttering if the player clicks the platform multiple times)
    //also removes differing speed between blocks if the player wants to change locations in the middle of movement
    IEnumerator moveToRoutine(Vector3 a, Node targetNode){
        Vector3 b = targetNode.transform.position;
        float step = (playerSpeed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f){
            t += step; // Goes from 0 to 1, incrementing by step each time
            transform.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        transform.parent = targetNode.transform;
        currNode = targetNode;
        yield return null;
    }

    // snap the Player to the nearest Node in Game view
    public void snapToNearestNode()
    {
        Node nearestNode = graph?.findClosestNodeAt(transform.position);
        // Debug.Log("Nearest node to click is " + nearestNode.name);
        if (nearestNode != null)
        {
            currNode = nearestNode;
            transform.position = nearestNode.transform.position;
        }
    }

    // turn face the next Node, always projected on a plane at the Player's feet
    public void faceNextPosition(Vector3 startPosition, Vector3 nextPosition)
    {
        if (Camera.main == null)
        {
            return;
        }

        // convert next Node world space to screen space
        Vector3 nextPositionScreen = Camera.main.WorldToScreenPoint(nextPosition);

        // convert next Node screen point to Ray
        Ray rayToNextPosition = Camera.main.ScreenPointToRay(nextPositionScreen);

        // plane at player's feet
        Plane plane = new Plane(Vector3.up, startPosition);

        // distance from camera (used for projecting point onto plane)
        float cameraDistance = 0f;

        // project the nextNode onto the plane and face toward projected point
        if (plane.Raycast(rayToNextPosition, out cameraDistance))
        {
            Vector3 nextPositionOnPlane = rayToNextPosition.GetPoint(cameraDistance);
            Vector3 directionToNextNode = nextPositionOnPlane - startPosition;
            if (directionToNextNode != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToNextNode);
            }
        }
    }

    // toggle between Idle and Walk animations
    private void updateAnimation(bool state)
    {
        if (playerAnim != null){
            playerAnim?.SetBool("Walking", state);
        }
    }

    // have we reached a specific Node?
    public bool hasReachedNode(Node node)
    {
        if (pathfinder == null || graph == null || node == null)
        {
            return false;
        }

        float distanceSqr = (node.transform.position - transform.position).sqrMagnitude;

        return (distanceSqr < 0.01f);
    }

    // have we reached the end of the graph?
    public bool hasReachedGoal()
    {
        if (graph == null)
        {
            return false;
        }
        return hasReachedNode(pathfinder.endNode);
    }

    //  enable/disable controls
    public void enableControls(bool state)
    {
        isControlEnabled = state;
    }

}


/*
 * Copyright (c) 2020 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
