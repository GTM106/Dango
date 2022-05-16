using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Plater1 : MonoBehaviour
{
    #region inputSystem
    private Vector2 moveAxis;
    private Vector2 roteAxis;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private GameObject PlayerCamera;
    private Vector3 Cameraforward;
    private Vector3 idou;
    public float angle;


    //�ړ�����
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveAxis = context.ReadValue<Vector2>().normalized;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveAxis = Vector2.zero;
            //�������������Ɨǂ�����
        }
    }

    //�W�����v����
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    //�c�q�e
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //Logger.Log("�c�q�e");
            for (int i = 0; i < Maxdango; i++)
            {
                //Logger.Log(i + "�Ԗڂ̐F" + dangos[i]);
            }
            if (dangoNum != 0)
            {
                Logger.Log(dangos[dangoNum - 1]);

                dangos[dangoNum - 1] = DangoType.None;
                dangoNum--;
            }
        }
    }

    //�˂��h��
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (dangoNum >= Maxdango)
            {
                Logger.Warn("�h����c�q�̐��𒴂��Ă��܂��B");
                return;
            }
            //Logger.Log("�˂��h���I");
            spitManager.canStab = true;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0, 2.2f);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(90f, 0, 0);
            var dangoType = spitManager.GetDangoType();
            if (dangoType != DangoType.None && dangoNum <= Maxdango)
            {
                dangos[dangoNum] = dangoType;
                dangoNum++;
            }
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            spitManager.canStab = false;
            spitManager.gameObject.transform.rotation = Quaternion.identity;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0.4f, 1.1f);
        }
    }

    //�H�ׂ�
    public void OnEatDango(InputAction.CallbackContext context)
    {
        if (dangoNum == 0) return;
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Logger.Log("�H�׃`���[�W�J�n�I�I");
                break;
            case InputActionPhase.Performed:
                Logger.Log("�H�ׂ��I");
                float value;
                var a = DangoRole.CheckRole(dangos, out value);
                switch (a)
                {
                    case RoleType.None:
                        break;

                    case RoleType.Buff:
                        BuffType type = (BuffType)dangos[0];
                        switch (type)
                        {
                            case BuffType.AttackUp:
                                _attackPower += value;
                                break;
                            case BuffType.MotionSpdUp:
                                _attackSpeed += value;
                                break;
                            case BuffType.HpUp:
                                _hitPoint += value;
                                break;
                            case BuffType.JumpPowerUp:
                                jumpPower += value;
                                break;
                            case BuffType.MoveSpdUp:
                                _moveSpeed += value;
                                break;
                            case BuffType.StrengthUp:
                                _strength += value;
                                break;
                            case BuffType.DebuffUp:
                                _Debuff += value;
                                break;

                            default:
                                break;
                        }
                        Logger.Log("�o�t����");
                        Logger.Log(type + "���オ����");
                        break;

                    case RoleType.Debuff:
                        DebuffType dtype = (DebuffType)Random.Range(1, 8);
                        Logger.Log("���k�c�q�𐶐����܂�...");

                        for (int i = 0; i < 99; i++)
                        {
                            if (!debuffs[(int)dtype - 1, i])
                            {
                                debuffs[(int)dtype - 1, i] = true;
                                break;
                            }
                        }
                        break;

                    case RoleType.Attack:
                        Logger.Log("�U������");

                        if (enemy != null)
                        {
                            enemy._hitPoint -= value;
                            Logger.Log(enemy._hitPoint);
                        }
                        break;

                }

                for (int i = 0; i < dangos.Length; i++)
                {
                    dangos[i] = DangoType.None;
                }

                dangoNum = 0;
                break;
        }
    }

    //��]
    public void OnRote(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            roteAxis = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            roteAxis = Vector2.zero;
        }

    }

    //���k�i�f�o�t�j����
    public void OnCompression(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //for(int i = 0; i < debuffs.; i++)
        }
    }

    #endregion

    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float jumpPower = 10f;
    [SerializeField] float _attackPower = 1f;
    [SerializeField] float _attackSpeed = 1f;
    [SerializeField] float _hitPoint = 100f;
    [SerializeField] float _strength = 1f;
    [SerializeField] float _Debuff = 1f;

    [SerializeField] SpitManager spitManager;
    DangoType[] dangos;
    int dangoNum = 0;
    int Maxdango = 7;
    [HideInInspector]
    public Plater1 enemy;
    //7��̃f�o�t�AStock��99�I�ȍl���B�Ȃ�\���̂Ŏ�������΂������B
    bool[,] debuffs = new bool[7, 99];


    private void OnEnable()
    {
        dangos = new DangoType[Maxdango];
        GameManager.SetPlayer(this.GetComponent<Plater1>());
        //��납�璲�ׂāAnull��������Ȃ��Bnull����Ȃ�������enemy�o�^
        if (GameManager.player[1] == null) return;

        enemy = GameManager.player[0];
        enemy.enemy = gameObject.GetComponent<Plater1>();

    }

    private void Start()
    {
    }

    private void Update()
    {
        if (_hitPoint <= 0) gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        //Vector3 move = new Vector3(moveAxis.x, 0, moveAxis.y);
        Vector3 move;
        angle = roteAxis.x;

        //�J�����̌������m�F�ACameraforward�ɑ��
        Cameraforward = Vector3.Scale(PlayerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        //�J�����̌��������Ƀx�N�g���̍쐬
        move = moveAxis.y * Cameraforward * _moveSpeed + moveAxis.x * PlayerCamera.transform.right * _moveSpeed;

        if (_rigidbody.velocity.magnitude < 8f)
            _rigidbody.AddForce(move * _moveSpeed*Time.deltaTime);
        //player�̌������J�����̕�����
        transform.rotation = Quaternion.Euler(0, PlayerCamera.transform.localEulerAngles.y+Time.deltaTime, 0);
    }
}
