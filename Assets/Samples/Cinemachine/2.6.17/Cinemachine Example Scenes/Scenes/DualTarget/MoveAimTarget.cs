using UnityEngine;
using Cinemachine;
using RootMotion.FinalIK;
public class MoveAimTarget : MonoBehaviour
{
    public CinemachineBrain Brain;
    public Collider[] colliders;
    public bool collide_obj = false;
    public RectTransform ReticleImage;
    public Vector3 IKPos = Vector3.zero;
    [Tooltip("How far to raycast to place the aim target")]
    public float AimDistance;

    [Tooltip("Objects on these layers will be detected")]
    public LayerMask CollideAgainst;

    [TagField]
    [Tooltip("Obstacles with this tag will be ignored.  "
        + "It's a good idea to set this field to the player's tag")]
    public string IgnoreTag = string.Empty;

    /// <summary>The Vertical axis.  Value is -90..90. Controls the vertical orientation</summary>
    [Header("Axis Control")]
    [Tooltip("The Vertical axis.  Value is -90..90. Controls the vertical orientation")]
    [AxisStateProperty]
    public AxisState VerticalAxis;

    /// <summary>The Horizontal axis.  Value is -180..180.  Controls the horizontal orientation</summary>
    [Tooltip("The Horizontal axis.  Value is -180..180.  Controls the horizontal orientation")]
    [AxisStateProperty]
    public AxisState HorizontalAxis;

    private void OnValidate()
    {
        VerticalAxis.Validate();
        HorizontalAxis.Validate();
        AimDistance = Mathf.Max(1, AimDistance);
    }

    private void Reset()
    {
        AimDistance = 200;
        ReticleImage = null;
        CollideAgainst = 1;
        IgnoreTag = string.Empty;

        VerticalAxis = new AxisState(-20, 20, true, true, 5f, 0.1f, 0.1f, "Mouse Y", false);
        VerticalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
        HorizontalAxis = new AxisState(-180, 180, true, false, 10f, 0.1f, 0.1f, "Mouse X", false);
        HorizontalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
    }

    private void OnEnable()
    {
        //CinemachineCore.CameraUpdatedEvent.RemoveListener(PlaceReticle);
        //CinemachineCore.CameraUpdatedEvent.AddListener(PlaceReticle);
        if(GamePlay.Instance.localPlayer)
        Brain = GamePlay.Instance.localPlayer.GetComponent<CinemachineBrain>();
    }

    private void OnDisable()
    {
        //CinemachineCore.CameraUpdatedEvent.RemoveListener(PlaceReticle);
    }

    private void Update()
    {
        if (Brain == null)
        {
            if (GamePlay.Instance.localPlayer)
                Brain = GamePlay.Instance.localPlayer.GetComponent<CinemachineBrain>();
            return;
        }


        //if (UIPlayManager.Instance.RotateLeftButton.GetComponent<ArrowButtonHandler>().isPressedFlag || UIPlayManager.Instance.RotateRightButton.GetComponent<ArrowButtonHandler>().isPressedFlag)
        {
            //transform.Rotate(Vector3.up, Time.deltaTime);
            //HorizontalAxis.Update(Time.deltaTime);
            //VerticalAxis.Update(Time.deltaTime);
        }

        if (UIPlayManager.Instance.ThirdCameraToggle.isOn)
        {
            PlaceTarget();
        }
        else if (GamePlay.Instance.localPlayer.selectedAnt)
        {
            LayerMask layerMask = 1 << 9;
            float sightDistance = GamePlay.Instance.localPlayer.selectedAnt.sightDistance;
            var camPos = GamePlay.Instance.localPlayer.selectedAnt.LandMinePoint.transform.position;
            camPos += new Vector3(0, GamePlay.Instance.localPlayer.selectedAnt.eyePoint.y, 0);
            float yDelta = (GamePlay.Instance.localPlayer.selectedAnt.eyePoint.y);
            Vector3 camPos1 = camPos;
            if (Physics.Raycast(camPos, GamePlay.Instance.localPlayer.selectedAnt.transform.forward, out RaycastHit hit, layerMask))
            {
                if (Vector3.Distance(camPos, hit.point) < sightDistance)
                {
                    camPos1 = hit.point;
                    if(hit.point.y > (Terrain.activeTerrain.SampleHeight(camPos1) + yDelta))
                    {
                        camPos1 = hit.point;
                    }
                    else
                    {
                        camPos1 = hit.point + new Vector3(0, yDelta, 0);
                    }
                }
                else
                {
                    Vector3 pos0 = camPos + GamePlay.Instance.localPlayer.selectedAnt.transform.forward * sightDistance;
                    float y0 = Terrain.activeTerrain.SampleHeight(pos0);
                    Vector3 pos1 = new Vector3(pos0.x, y0 + yDelta, pos0.z);
                    Vector3 dir = (pos1 - camPos);
                    float length = Vector3.Distance(pos1, camPos);
                    RaycastHit[] hits = Physics.RaycastAll(camPos, dir, sightDistance);
                    int count = hits.Length;
                    foreach(RaycastHit hi in hits)
                    {
                        if(hi.transform.root.gameObject == GamePlay.Instance.localPlayer.selectedAnt.gameObject)
                        {
                            count--;
                        }
                    }

                    if(count == 0)
                    {
                        camPos1 = pos1;
                    }
                    else
                    {
                        camPos1 = pos0;
                    }
                }
            }
            else
            {
                Vector3 pos0 = camPos + GamePlay.Instance.localPlayer.selectedAnt.transform.forward * sightDistance;
                float y0 = Terrain.activeTerrain.SampleHeight(pos0);
                Vector3 pos1 = new Vector3(pos0.x, y0 + yDelta, pos0.z);
                Vector3 dir = (pos1 - camPos);
                float length = Vector3.Distance(pos1, camPos);
                RaycastHit[] hits = Physics.RaycastAll(camPos, dir, length);
                int count = hits.Length;
                foreach (RaycastHit hi in hits)
                {
                    if (hi.transform.root.gameObject == GamePlay.Instance.localPlayer.selectedAnt.gameObject || hi.transform.tag == "Bullet")
                    {
                        count--;
                    }
                }

                if (count == 0)
                {
                    camPos1 = pos1;
                }
                else if(count > 0)
                {
                    camPos1 = pos0;
                }
            }

            Vector3 pos = new Vector3(camPos1.x, Terrain.activeTerrain.SampleHeight(camPos1) + yDelta, camPos1.z);
            Vector3 pos_1 = camPos1;
            float dis = Vector3.Distance(pos, camPos);
            colliders = Physics.OverlapSphere(GamePlay.Instance.localPlayer.selectedAnt.transform.position + GamePlay.Instance.localPlayer.selectedAnt.eyePoint, sightDistance);
            float deltaDis = sightDistance;
            float dis1 = sightDistance;
            
            foreach (Collider hit1 in colliders)
            {
                Vector3 vec1 = hit1.transform.position - GamePlay.Instance.localPlayer.selectedAnt.transform.position;
                Vector3 vec2 = GamePlay.Instance.localPlayer.selectedAnt.transform.forward;
                dis1 = Vector3.Distance(hit1.transform.root.position, camPos);
                if (hit1.transform.root.tag == "Ant" && hit1.transform.root.gameObject != GamePlay.Instance.localPlayer.selectedAnt.gameObject && 
                    (Vector2.Angle(new Vector2(vec1.x, vec1.z), new Vector2(vec2.x, vec2.z)) < 15f) && dis1 < deltaDis)
                {
                    //if ((pos.y > camPos.y && (hit1.transform.root.position.y + yDelta) > pos.y) || pos.y < camPos.y)
                    {
                        if(hit1.transform.root.GetComponent<AntController>())
                            pos_1 = hit1.transform.root.position + new Vector3(0, hit1.transform.root.GetComponent<AntController>().eyePoint.y, 0);// yDelta, 0);
                        else
                            pos_1 = hit1.transform.root.position + new Vector3(0, yDelta, 0);
                        deltaDis = dis1;
                    }
                }
            }

            if (Joy.Instance.aimHolded == false || Joy.Instance.rightPerformed)
            {
                transform.position = Vector3.Lerp(transform.position, pos_1, Time.deltaTime * 10);
                GamePlay.Instance.localPlayer.selectedAnt.GetComponent<AimIK>().solver.target.transform.position = transform.position;

                Debug.DrawLine(GamePlay.Instance.localPlayer.selectedAnt.transform.position + new Vector3(0, GamePlay.Instance.localPlayer.selectedAnt.eyePoint.y, 0), transform.position, Color.red, 0.3f, false);

            }
            if(GamePlay.Instance.localPlayer.selectedAnt.GetComponent<LookAtIK>().enabled == false)
                GamePlay.Instance.localPlayer.selectedAnt.GetComponent<LookAtIK>().enabled = true;
        }
    }

    public void PlaceTarget()
    {
        var rot = Quaternion.Euler(VerticalAxis.Value, HorizontalAxis.Value, 0);
        
        var camPos = Brain.CurrentCameraState.RawPosition;
        transform.position = GetProjectedAimTarget(camPos + rot * Vector3.forward, camPos);

        if (GamePlay.Instance.localPlayer.selectedAnt)
        {
            var _IKPos = GamePlay.Instance.localPlayer.selectedAnt.AimIKRawPosition;
            {
               //IKPos = GetProjectedAimTarget(_IKPos + rot * Vector3.forward, _IKPos);
            }
        }
    }

    private Vector3 GetProjectedAimTarget(Vector3 pos, Vector3 camPos)
    {
        var origin = pos;
        var fwd = (pos - camPos).normalized;
        pos += AimDistance * fwd;

        if (CollideAgainst != 0 && RaycastIgnoreTag(
            new Ray(origin, fwd), 
            out RaycastHit hitInfo, AimDistance, CollideAgainst))
        {
            collide_obj = true;
            pos = hitInfo.point;
        }
        else
        {
            collide_obj = false;
        }

        return pos;
    }

    private bool RaycastIgnoreTag(
        Ray ray, out RaycastHit hitInfo, float rayLength, int layerMask)
    {
        const float PrecisionSlush = 0.001f;
        float extraDistance = 0;
        while (Physics.Raycast(
            ray, out hitInfo, rayLength, layerMask,
            QueryTriggerInteraction.Ignore))
        {

            if (IgnoreTag.Length == 0 || !hitInfo.collider.CompareTag(IgnoreTag))
            {

                hitInfo.distance += extraDistance;
                return true;
            }

            // Ignore the hit.  Pull ray origin forward in front of obstacle
            Ray inverseRay = new Ray(ray.GetPoint(rayLength), -ray.direction);
            if (!hitInfo.collider.Raycast(inverseRay, out hitInfo, rayLength))
                break;
            float deltaExtraDistance = rayLength - (hitInfo.distance - PrecisionSlush);
            if (deltaExtraDistance < PrecisionSlush)
                break;
            extraDistance += deltaExtraDistance;
            rayLength = hitInfo.distance - PrecisionSlush;
            if (rayLength < PrecisionSlush)
                break;
            ray.origin = inverseRay.GetPoint(rayLength);

        }
        return false;
    }

    void PlaceReticle(CinemachineBrain brain)
    {
        if (brain == null || brain != Brain || ReticleImage == null || brain.OutputCamera == null)
            return;
        PlaceTarget(); // To eliminate judder
        CameraState state = brain.CurrentCameraState;
        var cam = brain.OutputCamera;
        var r = cam.WorldToScreenPoint(transform.position);
        var r2 = new Vector2(r.x - cam.pixelWidth * 0.5f, r.y - cam.pixelHeight * 0.5f);
        ReticleImage.anchoredPosition = r2;
    }
}
