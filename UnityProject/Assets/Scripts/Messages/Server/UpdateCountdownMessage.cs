﻿using System.Collections;

/// <summary>
///     Message that tells client the status of the preround countdown
/// </summary>
public class UpdateCountdownMessage : ServerMessage
{
	public bool Started;
	public float Time;

	public override IEnumerator Process()
	{
		UIManager.Display.preRoundWindow.GetComponent<GUI_PreRoundWindow>().SyncCountdown(Started, Time);
		yield return null;
	}

	public static UpdateCountdownMessage Send(bool started, float time)
	{
		UpdateCountdownMessage msg = new UpdateCountdownMessage
		{
			Started = started,
			Time = time
		};
		msg.SendToAll();
		return msg;
	}
}