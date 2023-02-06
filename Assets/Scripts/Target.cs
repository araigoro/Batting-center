using System.Collections;
using UnityEngine;

public class Target
{
    /// <summary>
    /// �q�b�g�^�[�Q�b�g�̃��C���[��
    /// </summary>
    private const string hitTargetLayerName = "HitTarget";

    /// <summary>
    /// ��\���ɂ���܂ł̎���(�P�ʁF�b)
    /// </summary>
    private const float aliveSeconds = 4.0f;

    /// <summary>
    /// �^�[�Q�b�g�v���n�u��GameObject
    /// </summary>
    public GameObject TargetGameObject { get; private set; }

    /// <summary>
    /// �^�[�Q�b�g��Collider
    /// ���^�[�Q�b�g�ɂ���ăR���C�_�[�̎�ނ��قȂ�̂ŁACollider�^�ŒT���ĕێ����Ă���
    /// </summary>
    private Collider targetCollider;

    /// <summary>
    /// �^�[�Q�b�g��Rigitbody
    /// </summary>
    private Rigidbody targetRigitbody;

    /// <summary>
    /// �\�������H
    /// </summary>
    public bool IsDisplay
    {
        get { return TargetGameObject.activeSelf ? false : true; }
    }

    /// <summary>
    /// �C�j�V�����C�U
    /// </summary>
    /// <param name="gameObject">�^�[�Q�b�g�̃v���n�u��GameObject</param>
    public Target(GameObject gameObject)
    {
        TargetGameObject = gameObject;

        // GetComponent�͏d���̂ŁA�R���|�[�l���g���擾���ĕێ����Ă���
        targetCollider = TargetGameObject.GetComponent<Collider>();
        targetRigitbody = TargetGameObject.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// �I�u�W�F�N�g��ڕW�n�_�ɔ�΂�
    /// </summary>
    /// <param name="targetPosition">�ڕW�n�_</param>
    /// <param name="angle">�p�x</param>
    public void MoveParabola(Vector3 targetPosition, float angle)
    {
        var startPosition = TargetGameObject.transform.position;
        var velocity = CalcVelocity(startPosition, targetPosition, angle);
        targetRigitbody.AddForce(velocity * targetRigitbody.mass, ForceMode.Impulse);
    }

    /// <summary>
    /// �\���^��\����ݒ肷��
    /// </summary>
    /// <param name="isDisplay">true:�\�� / false:��\��</param>
    public void SetDisplay(bool isDisplay)
    {
        TargetGameObject.SetActive(isDisplay);

        if (isDisplay == true)
        {
            // �\��
            targetCollider.enabled = true;
        }
    }

    /// <summary>
    /// ��莞�Ԍ�ɃI�u�W�F�N�g���\���ɂ���R���[�`��
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator Collect()
    {
        yield return new WaitForSeconds(aliveSeconds);

        targetRigitbody.velocity = Vector3.zero;
        SetDisplay(false);
    }

    /// <summary>
    /// �I�u�W�F�N�g�������ʒu�ɖ߂�(���X�|�[������)
    /// </summary>
    /// <param name="targetPosition"></param>
    public void Respawn(Vector3 targetPosition)
    {
        TargetGameObject.transform.position = targetPosition;
    }

    /// <summary>
    /// �I�u�W�F�N�g�͑łĂ�^�[�Q�b�g�I�u�W�F�N�g���H
    /// </summary>
    /// <returns>true: �^�[�Q�b�g / false: ��^�[�Q�b�g</returns>
    public bool IsHitTarget()
    {
        return LayerMask.LayerToName(TargetGameObject.layer) == hitTargetLayerName;
    }

    /// <summary>
    /// �n�_�ƏI�_�A�ł��グ�p�x����A���x�����߂�
    /// </summary>
    /// <param name="startPosition">�n�_</param>
    /// <param name="endPosition">�I�_</param>
    /// <param name="angle">�p�x</param>
    /// <returns>���x</returns>
    private Vector3 CalcVelocity(Vector3 startPosition, Vector3 endPosition, float angle)
    {
        var rad = angle * Mathf.PI / 180;
        var diffX = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), new Vector2(endPosition.x, endPosition.z));
        var diffY = startPosition.y - endPosition.y;
        var initVelocity = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(diffX, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (diffX * Mathf.Tan(rad) + diffY)));

        if (float.IsNaN(initVelocity))
        {
            return Vector3.zero;
        }

        return (new Vector3(endPosition.x - startPosition.x, diffX * Mathf.Tan(rad), endPosition.z - startPosition.z).normalized * initVelocity);
    }

    /// <summary>
    /// �R���C�_�[�𖳌��ɂ���
    /// </summary>
    public void ColliderOff()
    {
        targetCollider.enabled = false;
    }

    /// <summary>
    /// �I�u�W�F�N�g��Z�l���A�Ώۂ�Z�l�����傫�����H
    /// </summary>
    /// <param name="targetPositionZ">�Ώۂ�Z�l</param>
    /// <returns>true: �傫�� / false: ������������</returns>
    public bool IsLargePositionZ(float targetPositionZ)
    {
        return TargetGameObject.transform.position.z > targetPositionZ;
    }

    /// <summary>
    /// �I�u�W�F�N�g��Z�l���A�Ώۂ�Z�l�������������H
    /// </summary>
    /// <param name="targetPositionZ">�Ώۂ�Z�l</param>
    /// <returns>true: ������ / false: �������傫��</returns>
    internal bool IsSmallPositionZ(float targetPositionZ)
    {
        return TargetGameObject.transform.position.z < targetPositionZ;
    }
}
