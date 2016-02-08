using UnityEngine;
using System.Collections;
using NAudio.Midi;
using System.Collections.Generic;

public enum MidiType
{
	NOTE
}


public static class MidiFileReader{

	public static MidiNoteData ParseNote(MidiFile midi)
	{
		int bpm = 0;

		Dictionary<Difficulty, List<NoteMidiEvent>> midiEventLists = new Dictionary<Difficulty, List<NoteMidiEvent>>();

		List<NoteMidiEvent>[] midiEventList = new List<NoteMidiEvent>[3];

		for (int i = 0; i < midiEventList.Length; i++)
		{
			midiEventList[i] = new List<NoteMidiEvent>();
		}
		List<TempoEvent> tempo = new List<TempoEvent>();

		float period = 60f / 128f;

		float speed = 1f / midi.DeltaTicksPerQuarterNote;
		string currentState = "";
	
		string tempDJTimeNoteString = "";
		string tempNoteString = "";
		Difficulty tempDiff = Difficulty.NONE;

		int currentIndexDrop = 0;
		for (int i = 0; i < midi.Tracks; i++)
		{
			tempDJTimeNoteString = "";
			tempNoteString = "";
			int noteCount = 0;

			for (int j = 0; j < midi.Events[i].Count; j++)
			{
				if (midi.Events[i][j].CommandCode == MidiCommandCode.MetaEvent)
				{
					MetaEvent metaEventNote = (MetaEvent)midi.Events[i][j];
					if (metaEventNote.MetaEventType == MetaEventType.SequenceTrackName)
					{
						string[] splitTrackName = metaEventNote.ToString().Split(' ');
						currentState = splitTrackName[2];
					}
					else if (metaEventNote.MetaEventType == MetaEventType.SetTempo)
					{
						string[] splitTempo = metaEventNote.ToString().Split(' ');
						period = 60f / float.Parse(splitTempo[2].Substring(0, splitTempo[2].Length - 3));
						bpm = int.Parse(splitTempo[2].Substring(0, splitTempo[2].Length - 3));
					}
				}
				else if (midi.Events[i][j].CommandCode == MidiCommandCode.NoteOn)
				{
					var t_note = (NoteOnEvent)midi.Events[i][j];

					if (currentState.Contains("notes"))
					{
						int midiSlot = 0;
						string[] split = currentState.Split('-');
						if (split[1] == "easy")
						{
							tempDiff = Difficulty.EASY;
							midiSlot = 0;
						}
						if (split[1] == "medium")
						{
							tempDiff = Difficulty.NORMAL;
							midiSlot = 1;
						}
						if (split[1] == "hard")
						{
							tempDiff = Difficulty.HARD;
							midiSlot = 2;
						}

						if (t_note.Velocity > 0)// Not Use for Now
							//if (t_note.Velocity >= 100)// Not Use for Now
						{
							var tmpMidiEvent = new NoteMidiEvent();
							float noteHoldTime = 0;
							float noteTime = (t_note.AbsoluteTime * period * speed);
							int lane = t_note.NoteNumber % 12;
							if (t_note.NoteLength > 2)
							{
								noteHoldTime = (t_note.NoteLength * period * speed);
							}
							/*
							bool isInBuild = false;

							float noteBeat;
							float buildStartBeat;
							float buildEndBeat;

							foreach (var item in buildPhases)
							{
								noteBeat = (int)Mathf.Ceil(t_note.AbsoluteTime / midi.DeltaTicksPerQuarterNote * 2) / 2;

								buildStartBeat = (int)Mathf.Ceil(item.Key / midi.DeltaTicksPerQuarterNote * 2) / 2;
								buildEndBeat = (int)Mathf.Ceil(item.Value / midi.DeltaTicksPerQuarterNote * 2) / 2;

								if (noteBeat >= buildStartBeat && noteBeat < buildEndBeat)
								{
									isInBuild = true;

									//Sinoze.Engine.Logger.Log("S " + buildStartBeat + " E "+ buildEndBeat + " noteBeat " + noteBeat + " t_note.NoteLength "+ t_note.NoteLength);
								}
							}
							*/
							if (true)//(!isInBuild)
							{
								tmpMidiEvent.id = noteCount;
								tmpMidiEvent.hitTime = noteTime;
								tmpMidiEvent.length = noteHoldTime;
								tmpMidiEvent.lane = lane;

								midiEventList[midiSlot].Add(tmpMidiEvent);
								noteCount++;

								//Sinoze.Engine.Logger.Log("--- noteTime " + noteTime);
							}
						}
						else
						{
							float noteTime = (t_note.AbsoluteTime * period * speed);
							Debug.LogError("\t\t\tVelo " + t_note.Velocity + " | " + noteTime);
						}
					}
				}
			}
		}

		var tmp = new MidiNoteData();
		tmp.bpm = bpm;

		midiEventLists.Add(Difficulty.EASY, midiEventList[0]);
		midiEventLists.Add(Difficulty.NORMAL, midiEventList[1]);
		midiEventLists.Add(Difficulty.HARD, midiEventList[2]);

		tmp.midiEvents = midiEventLists;

		return tmp;
	}
}
