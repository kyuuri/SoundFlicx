using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// <para> NoteOn Event in Track name contains "notes"  </para>
/// <para> e.g. "notes-easy", "notes-normal" </para>
/// <para> </para>
/// <para> Octave 1 Only </para>
/// <para> Chord = LaneID e.g. (C = 0, D = 1, E = 2, F = 3, G = 4, A = 5, B = 6) </para>
/// <para> EventTime = Time of Note </para>
/// <para> EventLength = Length of Note </para>
/// </summary>
public struct NoteMidiEvent
{
	public int id;
	public int lane;
	public float hitTime;
	public float length;
}

public struct MidiNoteData
{
	public int bpm;
	public Dictionary<Difficulty, List<NoteMidiEvent>> midiEvents;
}

public enum Difficulty
{
	NONE,
	EASY,
	NORMAL,
	HARD
}

