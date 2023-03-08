using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //�̱���
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return instance;
        }
    }
    private static InputManager instance;    

    // ��ư ���� �� ��� �Է� ���� ��
    public bool SlideInput => slideinput;
    bool slideinput = false;

    // ��ư Ŭ���� �����ӿ��� �Է� ���� ��
    public bool JumpInput => jumpinput; // || Input.GetButtonDown("Jump");
    bool jumpinput = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        jumpinput = false;
    }

    public void SetSlideInput(bool active)
    {
        slideinput = active;
    }

    public void SetJumpInput(bool active)
    {
        jumpinput = active;
    }
}
