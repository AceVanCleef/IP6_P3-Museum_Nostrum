using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserInputListener {

    InputMessage TakeInput(InputMessage im);
}
