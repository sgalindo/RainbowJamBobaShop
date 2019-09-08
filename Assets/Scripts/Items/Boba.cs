using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boba
{
    public enum State { Empty, Pearls, Ready }
    public State state;

    public Boba()
    {
        state = State.Empty;
    }
}
