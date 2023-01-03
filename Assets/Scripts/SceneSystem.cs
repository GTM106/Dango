using Cysharp.Threading.Tasks;
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
    Scenes _prebScene;
    Scenes _currentScene;
    Scenes _currentIngameScene;

    [SerializeField] ChangeLightmap _changeLightmap;

    [SerializeField] Scenes startScene;

    private void Awake()
    {
        Instance = this;

        _currentScene = startScene;
        Load(startScene);
    }

    public bool Load(Scenes scene)
    {
        int index = (int)scene;

        //���d���[�h��h��
        if (_scenes[index] != null && _scenes[index].activeSelf) return false;

        _prebScene = _currentScene;
        _currentScene = scene;

        ChangeLightmap(scene);

        if (_scenes[index] == null) _scenes[index] = Instantiate(_sceneRoots[index]);
        else _scenes[index].SetActive(true);

        return true;
    }

    public bool UnLoad(Scenes scene, bool destroy)
    {
        //���[�h����Ă��Ȃ��Ȃ�e��
        if (_scenes[(int)scene] == null) return false;

        if (destroy) Destroy(_scenes[(int)scene]);
        else _scenes[(int)scene].SetActive(false);

        return true;
    }

    public async void ReLoad(Scenes scene)
    {
        UnLoad(scene, true);

        await UniTask.Yield();
        await UniTask.Yield();

        Load(scene);
    }

    private void ChangeLightmap(Scenes scene)
    {
        if(scene is >=Scenes.Stage1 and <= Scenes.Stage3)
        {
            _changeLightmap.StageLight();
        }
        else if(scene is Scenes.Tutorial)//Tutorial�̎d�l�ύX�ŕς��
        {
            _changeLightmap.TutorialLight();
        }
    }

    public Scenes PrebScene => _prebScene;
    public Scenes CurrentIngameScene => _currentIngameScene;
    public void SetIngameScene(Scenes scene) => _currentIngameScene = scene;
}
