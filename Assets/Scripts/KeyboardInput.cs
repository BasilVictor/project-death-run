using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kairos
{
  public class KeyboardInput : MonoBehaviour
  {

      void Update()
      {
        if (Input.GetKey(KeyCode.LeftShift))
        {
          VirtualInputManger.Instance.Shift =true;
        }
        else
        {
          VirtualInputManger.Instance.Shift = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
          VirtualInputManger.Instance.MoveRight =true;
        }
        else
        {
          VirtualInputManger.Instance.MoveRight = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
          VirtualInputManger.Instance.MoveLeft=true;
        }
        else
        {
          VirtualInputManger.Instance.MoveLeft = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
          VirtualInputManger.Instance.MoveUp =true;
        }
        else
        {
          VirtualInputManger.Instance.MoveUp = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
          VirtualInputManger.Instance.MoveDown=true;
        }
        else
        {
          VirtualInputManger.Instance.MoveDown = false;
        }
      }
  }
}
