using Unity.Cinemachine;
using UnityEngine;

public class CinemaController : MonoBehaviour
{
    [SerializeField] private CinemachineFollow cinemachineFollow;
    [SerializeField] private float zoomSpeed;

    [SerializeField] private float followOffsetX;
    [SerializeField] private float followOffsetY;
    [SerializeField] private float followOffsetZ;

    public void Update()
    {
        float x = Mathf.Lerp(cinemachineFollow.FollowOffset.x, followOffsetX, Time.deltaTime * zoomSpeed);
        float y = Mathf.Lerp(cinemachineFollow.FollowOffset.y, followOffsetY, Time.deltaTime * zoomSpeed);
        float z = Mathf.Lerp(cinemachineFollow.FollowOffset.z, followOffsetZ, Time.deltaTime * zoomSpeed);

        cinemachineFollow.FollowOffset.Set(x, y, z);
    }


    public void CinemaZoomIn()
    {
        followOffsetX = 0;
        followOffsetY = 1;
        followOffsetZ = 3;
    }

    public void CinemaZoomOut()
    {
        followOffsetX = 0;
        followOffsetY = 4;
        followOffsetZ = 12;
    }
}
