using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kairos
{
    public enum TransitionParameter
    {
      Move,
      Run,
    }
    public class CharacterControl : MonoBehaviour
  {
      public float Speed;
      public float RunSpeed;
      public float spd;
      public Animator animator;
      void Update()
      {
        if (VirtualInputManger.Instance.Shift)
        {
          animator.SetBool(TransitionParameter.Run.ToString(), true);
          spd = RunSpeed;
        }

        if (!VirtualInputManger.Instance.Shift)
        {
          animator.SetBool(TransitionParameter.Run.ToString(), false);
          spd = Speed;
        }

        if (VirtualInputManger.Instance.MoveRight && VirtualInputManger.Instance.MoveLeft)
        {
          animator.SetBool(TransitionParameter.Move.ToString(), false);
          return;
        }
        if (VirtualInputManger.Instance.MoveUp && VirtualInputManger.Instance.MoveDown)
        {
          animator.SetBool(TransitionParameter.Move.ToString(), false);
          return;
        }

        if (!VirtualInputManger.Instance.MoveRight && !VirtualInputManger.Instance.MoveLeft && !VirtualInputManger.Instance.MoveUp && !VirtualInputManger.Instance.MoveDown )
        {
          animator.SetBool(TransitionParameter.Move.ToString(), false);
        }

        if (VirtualInputManger.Instance.MoveRight)
        {
          this.gameObject.transform.Translate(Vector3.forward * spd * Time.deltaTime);
          this.gameObject.transform.rotation = Quaternion.Euler(0f,0f,0f);
          animator.SetBool(TransitionParameter.Move.ToString(), true);
        }

        if (VirtualInputManger.Instance.MoveLeft)
        {
          this.gameObject.transform.Translate(Vector3.forward * spd * Time.deltaTime);
          this.gameObject.transform.rotation = Quaternion.Euler(0f,180f,0f);
          animator.SetBool(TransitionParameter.Move.ToString(), true);
        }

        if (VirtualInputManger.Instance.MoveUp)
        {
          this.gameObject.transform.Translate(Vector3.forward * spd * Time.deltaTime);
          this.gameObject.transform.rotation = Quaternion.Euler(0f,270f,0f);
          animator.SetBool(TransitionParameter.Move.ToString(), true);
        }

        if (VirtualInputManger.Instance.MoveDown)
        {
          this.gameObject.transform.Translate(Vector3.forward * spd * Time.deltaTime);
          this.gameObject.transform.rotation = Quaternion.Euler(0f,90f,0f);
          animator.SetBool(TransitionParameter.Move.ToString(), true);
        }

      }
  }
}
