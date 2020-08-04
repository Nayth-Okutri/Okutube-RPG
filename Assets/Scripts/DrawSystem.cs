using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DrawSystem : MonoBehaviour
{
    public GameObject Brush;
    public float BrushSize = 0.1f;
    // Start is called before the first frame update
    public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    public LineRenderer lineRenderer2;
    public EdgeCollider2D edgeCollider;
    public List<Vector2> fingerPositions;

    int mouseButtonState = 0;
    float timer;

    private Vector3[] Form1;
    private Vector3[] Form2;

    public void CompareThat()
    {
        Vector3[] lineBuffer1;
        Vector3[] lineBuffer2;
        //Array.Resize(ref lineBuffer1, lineRenderer.positionCount);
        //Array.Resize(ref lineBuffer2, lineRenderer2.positionCount);
        lineBuffer1 = new Vector3[lineRenderer.positionCount];
        lineBuffer2 = new Vector3[lineRenderer2.positionCount];
        lineRenderer.GetPositions(lineBuffer1);
        lineRenderer2.GetPositions(lineBuffer2);

        /* float diff = DifferenceBetweenLines(lineBuffer1, lineBuffer2);
         const float threshold = 5f;

         Debug.Log(diff < threshold ? "Pretty close! " + diff .ToString(): "Not that close..." + diff.ToString());*/
        float diff = DifferenceBetweenLines(lineBuffer1, Form1);
        const float threshold = 5f;
        float diff2 = DifferenceBetweenLines(lineBuffer1, Form2);
        //Debug.Log(diff < threshold ? "Pretty close! " + diff.ToString() : "Not that close..." + diff.ToString()); */
        Debug.Log("identique a ligne 1 " + diff.ToString() + " ou identique cercle : " + diff2.ToString());
    }
    void Start()
    {
        timer = 10f;
        Form1 = new Vector3[3];
        Form1[0] = new Vector3(10f,10f,0f);
        Form1[1] = new Vector3(Screen.width/2, Screen.height / 2, 0f);
        Form1[2] = new Vector3(Screen.width , Screen.height , 0f);
        Form1[0] = new Vector3(-9, -4, 0f);
        Form1[1] = new Vector3(0, 0, 0f);
        Form1[2] = new Vector3(9, 4, 0f);
        //cercle
        Form2 = new Vector3[4];
        Form2[0] = new Vector3(Screen.width / 2, 3*Screen.height/4, 0f);
        Form2[1] = new Vector3(3*Screen.width / 4, Screen.height / 2, 0f);
        Form2[2] = new Vector3(Screen.width/2, Screen.height/4, 0f);
        Form2[3] = new Vector3(Screen.width/4, Screen.height/2, 0f);
        Form2[0] = new Vector3(0,3, 0f);
        Form2[1] = new Vector3(3,0, 0f);
        Form2[2] = new Vector3(0,-3, 0f);
        Form2[3] = new Vector3(-3, 0, 0f);

    }

    // Update is called once per frame
    void Update()
    {

        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                mouseButtonState = 0;
                timer = 10f;
            }
        }
        /*if (Input.GetMouseButton(0))
        {
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mRay, out hit))
            {
                var go = Instantiate(Brush, hit.point + Vector3.up * 0.1f, Quaternion.identity, transform);
                go.transform.localScale = Vector3.one * BrushSize;

            }
        }*/
        /*if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {

            Plane objPlan = new Plane(Camera.main.transform.forward * -1, this.transform.position);

            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlan.Raycast(mRay, out rayDistance))
                this.transform.position = mRay.GetPoint(rayDistance);
                
                    
          }*/

        if (Input.GetMouseButtonDown(0))
            {
                if (mouseButtonState == 0)
                {
                    CreateLine();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
               if (mouseButtonState == 1)  mouseButtonState++;
            }
            if (Input.GetMouseButton(0))
            {
                if (mouseButtonState == 1)
                {
                    Debug.Log(Input.mousePosition.ToString());
                    if (Input.mousePosition.x < -500 || Input.mousePosition.y > Screen.height || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0)
                    {
                        return;
                    }
                    Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .1f)
                    {
                        UpdateLine(tempFingerPos);
                    }
                }
            }

        if (Input.GetMouseButtonDown(0))
        {
            if (mouseButtonState == 2)
            {
                CreateLine2();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (mouseButtonState == 3) mouseButtonState++;
        }
        if (Input.GetMouseButton(0))
        {
            if (mouseButtonState == 3)
            {
                Debug.Log(Input.mousePosition.ToString());
                if (Input.mousePosition.x < 100 || Input.mousePosition.y > 420 || Input.mousePosition.x > 660 || Input.mousePosition.y < 7)
                {
                    return;
                }
                Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .1f)
                {
                    UpdateLine2(tempFingerPos);
                }
            }
        }





    }

    void CreateLine()
    {
        mouseButtonState++;
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
        fingerPositions.Clear();
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);
        edgeCollider.points = fingerPositions.ToArray();
    }

    void UpdateLine(Vector2 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
        edgeCollider.points = fingerPositions.ToArray();
    }

    void CreateLine2()
    {
        mouseButtonState++;
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer2 = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
        fingerPositions.Clear();
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        lineRenderer2.SetPosition(0, fingerPositions[0]);
        lineRenderer2.SetPosition(1, fingerPositions[1]);
        edgeCollider.points = fingerPositions.ToArray();
    }

    void UpdateLine2(Vector2 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);
        lineRenderer2.positionCount++;
        lineRenderer2.SetPosition(lineRenderer2.positionCount - 1, newFingerPos);
        edgeCollider.points = fingerPositions.ToArray();
    }



    float DifferenceBetweenLines(Vector3[] drawn, Vector3[] toMatch)
    {
        float sqrDistAcc = 0f;
        float length = 0f;

        Vector3 prevPoint = toMatch[0];

        foreach (var toMatchPoint in WalkAlongLine(toMatch))
        {
            sqrDistAcc += SqrDistanceToLine(drawn, toMatchPoint);
            length += Vector3.Distance(toMatchPoint, prevPoint);

            prevPoint = toMatchPoint;
        }

        return sqrDistAcc / length;
    }

    /// <summary>
    /// Move a point from the beginning of the line to its end using a maximum step, yielding the point at each step.
    /// </summary>
    IEnumerable<Vector3> WalkAlongLine(IEnumerable<Vector3> line, float maxStep = .1f)
    {
        using (var lineEnum = line.GetEnumerator())
        {
            if (!lineEnum.MoveNext())
                yield break;

            var pos = lineEnum.Current;

            while (lineEnum.MoveNext())
            {
                Debug.Log(lineEnum.Current);
                var target = lineEnum.Current;
                while (pos != target)
                {
                    yield return pos = Vector3.MoveTowards(pos, target, maxStep);
                }
            }
        }
    }

    static float SqrDistanceToLine(Vector3[] line, Vector3 point)
    {
        return ListSegments(line)
            .Select(seg => SqrDistanceToSegment(seg.a, seg.b, point))
            .Min();
    }

    static float SqrDistanceToSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
    {
        var projected = ProjectPointOnLineSegment(linePoint1, linePoint1, point);
        return (projected - point).sqrMagnitude;
    }

    /// <summary>
    /// Outputs each position of the line (but the last) and the consecutive one wrapped in a Segment.
    /// Example: a, b, c, d --> (a, b), (b, c), (c, d)
    /// </summary>
    static IEnumerable<Segment> ListSegments(IEnumerable<Vector3> line)
    {
        using (var pt1 = line.GetEnumerator())
        using (var pt2 = line.GetEnumerator())
        {
            pt2.MoveNext();

            while (pt2.MoveNext())
            {
                pt1.MoveNext();

                yield return new Segment { a = pt1.Current, b = pt2.Current };
            }
        }
    }
    struct Segment
    {
        public Vector3 a;
        public Vector3 b;
    }

    //This function finds out on which side of a line segment the point is located.
    //The point is assumed to be on a line created by linePoint1 and linePoint2. If the point is not on
    //the line segment, project it on the line using ProjectPointOnLine() first.
    //Returns 0 if point is on the line segment.
    //Returns 1 if point is outside of the line segment and located on the side of linePoint1.
    //Returns 2 if point is outside of the line segment and located on the side of linePoint2.
    static int PointOnWhichSideOfLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
    {
        Vector3 lineVec = linePoint2 - linePoint1;
        Vector3 pointVec = point - linePoint1;

        if (Vector3.Dot(pointVec, lineVec) > 0)
        {
            return pointVec.magnitude <= lineVec.magnitude ? 0 : 2;
        }
        else
        {
            return 1;
        }
    }

    //This function returns a point which is a projection from a point to a line.
    //The line is regarded infinite. If the line is finite, use ProjectPointOnLineSegment() instead.
    static Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVec, Vector3 point)
    {
        //get vector from point on line to point in space
        Vector3 linePointToPoint = point - linePoint;
        float t = Vector3.Dot(linePointToPoint, lineVec);
        return linePoint + lineVec * t;
    }

    //This function returns a point which is a projection from a point to a line segment.
    //If the projected point lies outside of the line segment, the projected point will
    //be clamped to the appropriate line edge.
    //If the line is infinite instead of a segment, use ProjectPointOnLine() instead.
    static Vector3 ProjectPointOnLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
    {
        Vector3 vector = linePoint2 - linePoint1;
        Vector3 projectedPoint = ProjectPointOnLine(linePoint1, vector.normalized, point);

        switch (PointOnWhichSideOfLineSegment(linePoint1, linePoint2, projectedPoint))
        {
            case 0:
                return projectedPoint;
            case 1:
                return linePoint1;
            case 2:
                return linePoint2;
            default:
                //output is invalid
                return Vector3.zero;
        }
    }


}

//https://stackoverflow.com/questions/57275082/how-to-compare-between-two-lines
public class LineCompare : MonoBehaviour
{

}
