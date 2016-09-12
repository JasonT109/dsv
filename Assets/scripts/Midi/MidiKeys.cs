using UnityEngine;
using System.Collections;

public static class MidiKeys
{

    /** Return a MIDI key corresponding to the given hotkey. */
    public static int HotKey(string hotKey)
    {
        int id;
        return int.TryParse(hotKey, out id) ? id : 0;
    }

}
