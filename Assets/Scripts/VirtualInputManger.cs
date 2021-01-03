using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kairos
{
  public class VirtualInputManger : Singleton<VirtualInputManger>
  {
    public bool MoveRight;
    public bool MoveLeft;
    public bool MoveUp;
    public bool MoveDown;
    public bool Shift;
  }

}
