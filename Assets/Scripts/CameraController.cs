using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private Dictionary<int, string> ListOfCameraConfiners =new Dictionary<int, string>();
    private bool Killed;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Nay");
        if (player!=null)
        {
            var vcam = GetComponent<CinemachineVirtualCamera>();
            vcam.LookAt = player.transform;
            //vcam.Follow = player.transform;
        }

    }
    private void OnDestroy()
    {
        Killed = true;
    }
        public void SetConfinerForActiveScene()
    {

        GameObject player = GameObject.Find("Nay");
        GameObject Confiner;
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (ListOfCameraConfiners.ContainsKey(SceneIndex))
        {
            Confiner = GameObject.Find(ListOfCameraConfiners[SceneIndex]);
            //if ((gameObject != null))
            if (!Killed)
            {
                var vcamconfiner = GetComponent<CinemachineConfiner>();
                if( vcamconfiner != null)
                {
                    vcamconfiner.m_BoundingShape2D = Confiner.GetComponent<PolygonCollider2D>();
                    vcamconfiner.m_ConfineMode = CinemachineConfiner.Mode.Confine2D;

                    if (SceneIndex == 1) vcamconfiner.m_ConfineScreenEdges = false;
                    else vcamconfiner.m_ConfineScreenEdges = true;
                }
               
            }
        }
        else
        {
            if (!Killed)
            {
                var vcamconfiner = GetComponent<CinemachineConfiner>();
                if (vcamconfiner != null)
                {
                    vcamconfiner.m_BoundingShape2D = null;
                }
            }
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
      

    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
     
        SetConfinerForActiveScene();
    }
        void Awake()
    {
        if (!ListOfCameraConfiners.ContainsKey(1)) ListOfCameraConfiners.Add(1, "CameraConfinerAtelier");
        if (!ListOfCameraConfiners.ContainsKey(4))ListOfCameraConfiners.Add(4, "CameraConfinerWorldmap");
        if (!ListOfCameraConfiners.ContainsKey(6)) ListOfCameraConfiners.Add(2, "CameraConfinerJapex");
        if (!ListOfCameraConfiners.ContainsKey(8)) ListOfCameraConfiners.Add(7, "CameraConfinerTavern");
        SetConfinerForActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
