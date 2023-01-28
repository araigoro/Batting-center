using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBall : MonoBehaviour
{
    [SerializeField]
    private GameObject[] battingTargets;

    [SerializeField]
    private AudioClip shotSound;

    private const int shotInterval = 2000;

    private const int shotSpeed = 270;

    private const float destroyTime = 5.0f;

    // ������I�u�W�F�N�g�̃C���f�b�N�X
    private int throwTargetIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (IsThrowsThisFrame(Time.frameCount))
        {
            ThrowObject(InstantiateNextObject());
        }
    }

    /// <summary>
    /// ���̃t���[���Ŏ��̃I�u�W�F�N�g�𓊂��邩�H
    /// </summary>
    /// <param name="frameCount">�t���[����</param>
    /// <returns>true:������ / false:�����Ȃ�</returns>
    private bool IsThrowsThisFrame(int frameCount)
    {
        return frameCount % shotInterval == 0;
    }

    /// <summary>
    /// ���ɓ�����I�u�W�F�N�g���C���X�^���X������
    /// </summary>
    /// <returns>�v���n�u��GameObject</returns>
    private GameObject InstantiateNextObject()
    {
        GameObject gameObject = Instantiate(battingTargets[throwTargetIndex], transform.position, Quaternion.identity);

        // �o�^���ɃI�u�W�F�N�g��؂�ւ���
        throwTargetIndex = (throwTargetIndex + 1) % battingTargets.Length;

        return gameObject;
    }

    /// <summary>
    /// �w�肵���I�u�W�F�N�g�𓊂���
    /// </summary>
    /// <param name="target">������I�u�W�F�N�g�̃v���n�u</param>
    private void ThrowObject(GameObject target)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();

        // �Ƃ肠�����K���ɉ�]������
        targetRb.angularVelocity = new Vector3(0.5f, 0.5f, 0.5f);

        targetRb.AddForce(transform.forward * shotSpeed);
        AudioSource.PlayClipAtPoint(shotSound, transform.position);
        Destroy(target, destroyTime);
    }
}
