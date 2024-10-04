/*
    SettingsManager.cs 
    
    ���ʂ�X�N���[�����x�A����Ȃǂ̐ݒ���Ǘ�����N���X�B
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // �X�s�[�h�A�{�����[���ݒ�̃X���C�_�[
    [SerializeField]private Slider speedSlider;
    [SerializeField]private Slider volumeSlider;

    // �X�s�[�h�A�{�����[���ݒ�̒l��\������e�L�X�g
    [SerializeField]private Text speedText;
    [SerializeField]private Text volumeText;

    // ����ݒ�̃g�O��
    [SerializeField]private Toggle jaToggle;
    [SerializeField]private Toggle enToggle;

    // �ۑ�����Ă���v���C���[�f�[�^
    private JsonPlayerData playerSpeedData;
    private JsonPlayerData playerVolumeData;

    // �X�s�[�h�A�{�����[���̒l
    public static float speedValue;
    public static float volumeValue;

    // �v���X�{�^�������Œl�𑝂₷�֐� - �X�s�[�h
    public void AddSpeedValue()
    {
        speedSlider.value += 0.1f;
    }

    // �}�C�i�X�{�^�������Œl�����炷�֐� - �X�s�[�h
    public void ReduceSpeedValue()
    {
        speedSlider.value -= 0.1f;
    }

    // �v���X�{�^�������Œl�𑝂₷�֐� - �{�����[��
    public void AddVolumeValue()
    {
        volumeSlider.value += 0.1f;
    }

    // �}�C�i�X�{�^�������Œl�����炷�֐� - �{�����[��
    public void ReduceVolumeValue()
    {
        volumeSlider.value -= 0.1f;
    }

    // �ݒ�L�^����(�l�ύX���ɋL�^) - �X�s�[�h
    public void PlayerSpeedSettingsSave()
    {
        // �I�u�W�F�N�g�ɒl���i�[
        playerSpeedData = new JsonPlayerData();

        // �X�s�[�h�̒l���i�[
        playerSpeedData.speValue = speedSlider.value;

        // �I�u�W�F�N�g��JSON�`���ɕϊ�
        PlayerPrefs.SetString("SpeedValue", JsonUtility.ToJson(playerSpeedData));

        // �X���C�_�[�̒l��\��
        speedText.text = speedSlider.value.ToString("N1");
    }

    // �ݒ�L�^����(�l�ύX���ɋL�^) - �{�����[��
    public void PlayerVolumeSettingsSave()
    {
        // �I�u�W�F�N�g�ɒl���i�[
        playerVolumeData = new JsonPlayerData();

        // �{�����[���̒l���i�[
        playerVolumeData.volValue = volumeSlider.value;

        // �I�u�W�F�N�g��JSON�`���ɕϊ�
        PlayerPrefs.SetString("VolumeValue", JsonUtility.ToJson(playerVolumeData));

        // �X���C�_�[�̒l��\��
        volumeText.text = volumeSlider.value.ToString("N1");
    }

    // �ۑ�����Ă����l���擾���ăZ�b�g���鏈��
    private void SetPlayerData()
    {
        // playerSpeedData���ۑ�����Ă���Ύ擾
        if (playerSpeedData != null)
        {
            speedValue = playerSpeedData.speValue;
            speedSlider.value = speedValue;
        }
        // playerVolumeData���ۑ�����Ă���Ύ擾
        if (playerVolumeData != null)
        {
            volumeValue = playerVolumeData.volValue;
            volumeSlider.value = volumeValue;
        }
    }

    // �X�s�[�h�A�{�����[���̒l���Z�b�g���鏈��
    private void SetValue()
    {
        // �X�s�[�h�A�{�����[���̑��
        var val = speedSlider.value * 10f;  // �����_1���ȉ��؂�̂�
        speedValue = Mathf.Floor(val) / 10f;

        volumeValue = volumeSlider.value;
    }

    private void Init()
    {
        // �X�s�[�h�A�{�����[���̒l���X���C�_�[�̒l�ɐݒ�
        speedValue = speedSlider.value;
        volumeValue = volumeSlider.value;

        // �g�O���̐ݒ�
        if (GameManager.isJapanese)
        {
            jaToggle.isOn = true;
        }
        else
        {
            enToggle.isOn = true;
        }

        // �X�s�[�h�̒l���ۑ�����Ă��邩�m�F
        if (PlayerPrefs.HasKey("SpeedValue"))
        {
            // �ۑ�����Ă�����l���擾
            var jsonpData = PlayerPrefs.GetString("SpeedValue");

            // �擾����Json�f�[�^���I�u�W�F�N�g�ɕϊ�
            playerSpeedData = JsonUtility.FromJson<JsonPlayerData>(jsonpData);

            // �v���C���[�f�[�^����
            SetPlayerData();
        }

        // �{�����[���̒l���ۑ�����Ă��邩�m�F
        if (PlayerPrefs.HasKey("VolumeValue"))
        {
            // �ۑ�����Ă�����l���擾
            var jsonpData = PlayerPrefs.GetString("VolumeValue");

            // �擾����Json�f�[�^���I�u�W�F�N�g�ɕϊ�
            playerVolumeData = JsonUtility.FromJson<JsonPlayerData>(jsonpData);

            // �v���C���[�f�[�^����
            SetPlayerData();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // ����������
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // �X�s�[�h�A�{�����[���̒l���Z�b�g
        SetValue();
    }
}
