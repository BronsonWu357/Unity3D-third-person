using UnityEngine;


namespace Game.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        //��ͷ�ĸ���Ŀ��
        [SerializeField] Transform followTarget;

        //��ͷ������Ŀ��ľ���
        [SerializeField] float distance = 5;

        //��ͷY����ת����
        float rotationY = 0f;

        //��ͷX����ת����
        float rotationX = 0f;

        //���ֱ�Ƕ�
        [SerializeField] float maxVerticalAngle = 45f;

        //��С��ֱ�Ƕ�
        [SerializeField] float minVerticalAngle = -45f;

        //��ͷƫ��
        [SerializeField] Vector3 framingOffset = Vector3.zero;

        //����ƫ��
        [SerializeField] Vector3 playerOffset = Vector3.zero;

        //��ͷ��ת�ٶ�
        [SerializeField] float rotationSpeedX = 0f;
        [SerializeField] float rotationSpeedY = 0f;

        public bool canRotate = true;


        public void Start()
        {
            //�������Ϊ���أ����������
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }


        // ����Ĵ���ÿһ֡����ִ��
        // ִ��Ƶ��ȡ������Ϸ֡��
        public void Update()
        {

            if (canRotate)
            {
                //��ֵΪ���X�����ת�Ƕ�
                rotationY += Input.GetAxis("Camera X") * rotationSpeedX;

                //��ֵΪ���Y�����ת�Ƕ�
                rotationX += Input.GetAxis("Camera Y") * rotationSpeedY;
            }
            //������ת�Ƕȵķ�Χ
            rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

            //�����ܵ���ת����
            // ��ŷ����ת��Ϊ��Ԫ��
            var rotationTarget = Quaternion.Euler(rotationX, rotationY, 0);

            //��������������ƫ��
            var offset = rotationTarget * framingOffset;

            //��ͷ����
            //����λ�� = ����� + �������ƫ��
            var focusPosition = followTarget.position + playerOffset + offset;

            transform.position = focusPosition - rotationTarget * new Vector3(0, 0, distance);

            //����ͷ����ת���ݸ�ֵΪ��ת����
            transform.rotation = rotationTarget;
        }


        // �����汾��ÿ�ε���ʱ���㵱ǰ rotationY ��ֵ
        //public Quaternion PlanerRotation()
        //{
        //    Quaternion rotation = Quaternion.Euler (0, rotationY, 0);

        //    return rotation;
        //}


        // ���԰汾��ÿ�η���ʱ���㵱ǰ rotationY ��ֵ  
        public Quaternion PlanerRotation => Quaternion.Euler(0, rotationY, 0);


        public void SetFollowTarget(GameObject gameObject)
        {
            followTarget = gameObject.transform;
        }
    }
}
