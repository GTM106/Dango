using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//����͒P��V�[���ł̎����ɂȂ邽�߁A�V�[���̂悤�ɓǂݍ��ޕK�v������܂���
//�������A���ׂĂ��X�e�[�W��ɑ��݂��Ă��܂��ƃ����������̕��H���Ă��܂��܂�
//�Ƃ����̂����͂ŉ�������̂����̃X�N���v�g�ł�

#if UNITY_EDITOR
[CustomEditor(typeof(SceneSystem))]
public class SceneSystemOnGUI : Editor
{
    private SceneSystem sceneSystem;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        sceneSystem = target as SceneSystem;

        EditorGUILayout.HelpBox("�v���n�u��ǉ�����ۂ͏��Ԃɒ��ӂ��Ă��������B", MessageType.Info);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Scenes : Element " + (int)sceneSystem.debugScenes);
        EditorGUILayout.Separator();

        base.OnInspectorGUI();

    }
}
#endif


class SceneSystem : MonoBehaviour
{
    public enum Scenes
    {
        Title,
        Tutorial,
        Menu,
        StageSelect,
        Stage1,
        Stage2,
        Stage3,
        InGamePause,
        Success,
        GameOver,
        Option,
        Ex,

        [InspectorName("")]
        Max,
    }

#if UNITY_EDITOR
    /// <summary>�f�o�b�O�p�ł��B�g�p�֎~</summary>
    public Scenes debugScenes;
#endif

    public static SceneSystem Instance { get; private set; }

    [SerializeField] GameObject[] _sceneRoots = new GameObject[(int)Scenes.Max];
    GameObject[] _scenes = new GameObject[(int)Scenes.Max];

    private void Awake()
    {
        Instance = this;
        //Load(Scenes.Option);
        //Load(Scenes.Stage1);
        //Load(Scenes.Title);
        Load(Scenes.Menu);
    }

    public bool Load(Scenes scene)
    {
        _scenes[(int)scene] = Instantiate(_sceneRoots[(int)scene]);
        return true;
    }

    public bool UnLoad(Scenes scene)
    {
        Destroy(_scenes[(int)scene]);
        return true;
    }
}
