﻿using System.Collections;
using UnityEngine;

/// <summary>
///     Message that tells client which UI action to perform
/// </summary>
public class UpdateHungerStateMessage : ServerMessage
{
	public HungerState State;

	public override IEnumerator Process()
	{
		MetabolismSystem metabolismSystem = PlayerManager.LocalPlayer.GetComponent<MetabolismSystem>();

		metabolismSystem.HungerState = State;

		yield return null;
	}

	public static UpdateHungerStateMessage Send(GameObject recipient, HungerState state)
	{
		UpdateHungerStateMessage msg = new UpdateHungerStateMessage
		{
			State = state
		};
		msg.SendTo(recipient);
		return msg;
	}
}