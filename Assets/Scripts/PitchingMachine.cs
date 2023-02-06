using System.Collections.Generic;
using UnityEngine;


public class PitchingMachine : MonoBehaviour
{
    /// <summary>
    /// ���g�̃Q�[���I�u�W�F�N�g
    /// </summary>
    [SerializeField] private GameObject pitchingMachine;
    
    /// <summary>
    /// �^�[�Q�b�g�Ƃ��ē�����I�u�W�F�N�g�̃v���n�u
    /// </summary>
    [SerializeField] private GameObject[] targetPrefabs;
    
    /// <summary>
    /// �V���b�g��
    /// </summary>
    [SerializeField] private AudioClip soundShot;

    /// <summary>
    /// �^�[�Q�b�g�ێ��e�[�u��
    /// </summary>
    private List<Target> targetPool = new List<Target>();

    /// <summary>
    /// ������Ԋu(�P�ʁF�t���[��)
    /// </summary>
    private const int shotInterval = 2000;

    /// <summary>
    /// ������I�u�W�F�N�g�̊p�x
    /// </summary>
    private const float shotAngle = 30;

    /// <summary>
    /// �I�u�W�F�N�g���΂��ڕW�n�_
    /// </summary>
    private Vector3 strikePosition = new Vector3(0, 3, -1) / 10;

    private void Awake()
    {
        // ���ׂẴ^�[�Q�b�g�v���n�u�𐶐����āA�^�[�Q�b�g�v�[���ɒǉ�����
        CreateAllTargetPrefabs();
    }

    private void Update()
    {
        // ���Ԋu�œ�����
        if ((Time.frameCount % shotInterval) == 0)
        {
            ShotTarget();
        }
    }
    
    /// <summary>
    /// �����_���Ń^�[�Q�b�g��I��œ�����
    /// </summary>
    private void ShotTarget()
    {
        // �����_���Ŏ��ɓ�����^�[�Q�b�g��I��
        var target = SelectRandomTarget();
        
        if (target != null)
        {
            // �����ʒu�ɐݒ�
            target.Respawn(pitchingMachine.transform.position);
            target.SetDisplay(true);
            target.MoveParabola(strikePosition, shotAngle);

            // �����鉹��炷
            AudioSource.PlayClipAtPoint(soundShot, pitchingMachine.transform.position);

            // ��莞�Ԃŏ���
            StartCoroutine(target.Collect());
        }
    }

    /// <summary>
    /// ���ׂẴ^�[�Q�b�g�v���n�u�𐶐����āA�^�[�Q�b�g�v�[���ɒǉ�����
    /// </summary>
    private void CreateAllTargetPrefabs()
    {
        targetPool.Clear();

        // �o�^����Ă��邷�ׂẴ^�[�Q�b�g�v���n�u�ɕR�Â����ATarget�𐶐�����
        for (var index = 0; index < targetPrefabs.Length; index++)
        {
            // �^�[�Q�b�g�𐶐�
            var gameObject = Instantiate(targetPrefabs[index], transform.position, Quaternion.identity);
            var target = new Target(gameObject);
            target.SetDisplay(false);

            // �^�[�Q�b�g�v�[���ɒǉ�
            targetPool.Add(target);
        }
    }

    /// <summary>
    /// ��\���̃^�[�Q�b�g�̒����烉���_���ɑI��
    /// </summary>
    /// <returns>�^�[�Q�b�g(�I�ׂȂ������ꍇ��null)</returns>
    private Target SelectRandomTarget()
    {
        // �������[�v�ɂȂ�Ȃ��悤�ɑΏ�
        if (targetPool.Count == 0)
        {
            return null;
        }

        // �����_���ɑI��
        Target target;
        do
        {
            target = targetPool[UnityEngine.Random.Range(0, targetPool.Count)];
        } while (target.IsDisplay == false);

        return target;
    }
}
