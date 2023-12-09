using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using InputDevice = UnityEngine.InputSystem.InputDevice;

public class HookshotHandle : MonoBehaviour
{
    public LineRenderer lr;
    [SerializeField] public InputActionReference Trriger;
    public GameObject rightController;
    private Rigidbody rb;
    private CharacterController characterController;



    [Header("hookshot")]
    private bool hookshotStart;
    public Vector3 HookshotPos;
    private SpringJoint joint;
    [SerializeField] public Transform currentPos;
    [SerializeField] public Transform tip;
    [SerializeField] public LayerMask grappleAble;

    private void Awake()
    {
        hookshotStart = false;
        lr.enabled = false;
        rightController = GameObject.Find("Right Controller");
        lr = GetComponent<LineRenderer>();
        //joint = GetComponent<SpringJoint>();
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
      
    }

    private void Update()
    {
        HookshotStart();
        if (hookshotStart)
        {
            HookshotMove();
        }
        print(hookshotStart);
    }

    private void LateUpdate()
    {
        DrawRope();
        Delete();

    }

    public bool isreached;

    public void HookshotStart()
    {
        if (onTrigger()&& !hookshotStart)
        {
            if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out RaycastHit hit, 100f, grappleAble))
            {
                hookshotStart = true;
                HookshotPos = hit.point;
            }

        }
    }
    public void HookshotMove()
    {
        lr.enabled = true;
                
        hookshotStart=true;
        Vector3 hookshotDir = (HookshotPos - transform.position).normalized;

        //joint = gameObject.AddComponent<SpringJoint>();
        //joint.autoConfigureConnectedAnchor = false;
        //joint.connectedAnchor = HookshotPos;

        characterController.Move(hookshotDir * 40f * Time.deltaTime);

        //float distanceFromPoint = Vector3.Distance(transform.position, HookshotPos);

        //joint.maxDistance = distanceFromPoint * 0.5f;
        //joint.minDistance = distanceFromPoint * 0.25f;
        //joint.spring = 100f;
        //joint.damper = 200f;
        //joint.massScale = 1f;


        isreached = Vector3.Distance(transform.position, HookshotPos) < 4f;
       
        if (isreached)
        { 
             Delete();
            hookshotStart = false;

         }
            
        }
       

    public bool onTrigger()
    {
        UnityEngine.XR.InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        return device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerButtonState) && triggerButtonState;
    }

    public void DrawRope()
    {
   
            lr.SetPosition(0, tip.position);
            lr.SetPosition(1, HookshotPos);

        
    }

    public void Delete()
    {
        if(!hookshotStart) {  
            lr.enabled = false;
        }

    }
}
