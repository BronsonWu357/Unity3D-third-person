using UnityEngine;


namespace Game.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        //镜头的跟随目标
        [SerializeField] Transform followTarget;

        //镜头跟跟随目标的距离
        [SerializeField] float distance = 5;

        //镜头Y轴旋转变量
        float rotationY = 0f;

        //镜头X轴旋转变量
        float rotationX = 0f;

        //最大垂直角度
        [SerializeField] float maxVerticalAngle = 45f;

        //最小垂直角度
        [SerializeField] float minVerticalAngle = -45f;

        //镜头偏移
        [SerializeField] Vector3 framingOffset = Vector3.zero;

        //人物偏移
        [SerializeField] Vector3 playerOffset = Vector3.zero;

        //镜头旋转速度
        [SerializeField] float rotationSpeedX = 0f;
        [SerializeField] float rotationSpeedY = 0f;

        public bool canRotate = true;


        public void Start()
        {
            //将光标设为隐藏，并锁定光标
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }


        // 这里的代码每一帧都会执行
        // 执行频率取决于游戏帧率
        public void Update()
        {

            if (canRotate)
            {
                //赋值为鼠标X轴的旋转角度
                rotationY += Input.GetAxis("Camera X") * rotationSpeedX;

                //赋值为鼠标Y轴的旋转角度
                rotationX += Input.GetAxis("Camera Y") * rotationSpeedY;
            }
            //限制旋转角度的范围
            rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

            //定义总的旋转变量
            // 将欧拉角转换为四元数
            var rotationTarget = Quaternion.Euler(rotationX, rotationY, 0);

            //根据相机方向计算偏移
            var offset = rotationTarget * framingOffset;

            //镜头焦点
            //焦点位置 = 跟随点 + 相机方向偏移
            var focusPosition = followTarget.position + playerOffset + offset;

            transform.position = focusPosition - rotationTarget * new Vector3(0, 0, distance);

            //将镜头的旋转数据赋值为旋转总量
            transform.rotation = rotationTarget;
        }


        // 方法版本：每次调用时计算当前 rotationY 的值
        //public Quaternion PlanerRotation()
        //{
        //    Quaternion rotation = Quaternion.Euler (0, rotationY, 0);

        //    return rotation;
        //}


        // 属性版本：每次访问时计算当前 rotationY 的值  
        public Quaternion PlanerRotation => Quaternion.Euler(0, rotationY, 0);


        public void SetFollowTarget(GameObject gameObject)
        {
            followTarget = gameObject.transform;
        }
    }
}
