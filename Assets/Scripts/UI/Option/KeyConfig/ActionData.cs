using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TM.Input.KeyConfig
{
    class ActionData
    {
        //���̃f�[�^�ɑΉ����铮�I�ȃA�N�V����
        KeyData.GameAction _action;

        //���݂̂��̃A�N�V�����ɑ΂���L�[
        List<KeyData.GamepadKey> _currentKeys = new();

        //���I�ȃA�N�V�����̃��t�@�����X(example : Player/Move)
        //���ꂾ���Ő��䂷��̂͒����I�ł͂Ȃ�����GameAction���̗p���Ă��܂�
        InputActionReference _actionReference = ScriptableObject.CreateInstance<InputActionReference>();

        //�A�N�V�����}�b�v�̖��O�B�z�肳��閼�O��"Player"��UI��
        string _actionMapName;

        public ActionData(KeyData.GameAction action, List<KeyData.GamepadKey> key, InputActionAsset asset, string actionMapName)
        {
            _action = action;
            _currentKeys = key;
            _actionMapName = actionMapName;
            SetActionReference(action, asset);
        }
        public ActionData(KeyData.GameAction action, KeyData.GamepadKey[] key, InputActionAsset asset, string actionMapName)
        {
            _action = action;
            _currentKeys.AddRange(key);
            _actionMapName = actionMapName;
            SetActionReference(action, asset);
        }
        public ActionData(KeyData.GameAction action, KeyData.GamepadKey key, InputActionAsset asset, string actionMapName)
        {
            _action = action;
            _currentKeys.Add(key);
            _actionMapName = actionMapName;
            SetActionReference(action, asset);
        }

        //ActionReference�𓮓I�ɐݒ肷��֐��B�O���g�p�͑z�肵�Ă��܂���B
        private void SetActionReference(KeyData.GameAction action, InputActionAsset asset)
        {
            _actionReference.Set(asset, _actionMapName, ToActionName(action, asset));
        }

        //�A�Z�b�g�̃A�N�V�������ɍ��킹�ĕϊ�����֐��B�O���g�p�͑z�肵�Ă��܂���B
        private string ToActionName(KeyData.GameAction action, InputActionAsset asset)
        {
            return asset.FindActionMap(_actionMapName).actions[(int)action].name;
        }

        /// <summary>
        /// �w��̃L�[���A�N�V�����Ɋ܂�ł��邩���肷�郁�\�b�h
        /// </summary>
        /// <param name="key">���肵�����L�[</param>
        /// <returns>true:�܂�ł���</returns>
        public bool IsContainsKey(KeyData.GamepadKey key)
        {
            return _currentKeys.Contains(key);
        }

        public KeyData.GameAction Action => _action;
        public InputActionReference ActionReference => _actionReference;
        public List<KeyData.GamepadKey> Keys => _currentKeys;
    }
}