using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Messages.Server.SoundMessages;

public class Tutorial : MonoBehaviour
{
    [SerializeField] CommonSounds _commonSound;
	public static GameObject botGO;
    public bool deleteGO;
    public enum Phase
    {
        SpawnMove,
        Equip,
        Id,
        PdaInId,
        Light,
        Mask,
        NoAir,
        Food,
        FireArm,
        Heal,
        Farm,
        Evac,
        Leave

    }

    public Phase tutoPhase;

    public static Lang_Bot langBot;
    private void Start()
    {
        //load languages file
        langBot = new Lang_Bot(Path.Combine(Application.persistentDataPath, "languages/Lang_Bot_" + GameManager.Instance.language + ".xml.xml"), GameManager.Instance.language);
        //langBot = new Lang_Bot(Path.Combine(Application.persistentDataPath, "languages/Lang_Bot_" + GameManager.Instance.language + ".xml"), GameManager.Instance.language);
        //UI.ControlTabs.Instance.gameObject.SetActive(false);
    }

    ///Send message to chat depending on the phase
    public void Message(GameObject GO)
    {
        if (tutoPhase != Phase.Leave)
        {
            string message = Tutorial.langBot.GetString(tutoPhase.ToString());
            Chat.AddLocalMsgToChat(message, GO);
            var bot = botGO.GetComponentInChildren<Animator>();

            AudioSourceParameters audioSourceParameters = new AudioSourceParameters(pitch: UnityEngine.Random.Range(0.8f, 1.2f));
            switch (tutoPhase)

            {
                case Phase.PdaInId:
                    //SoundManager.PlayNetworkedAtPos(CommonSounds.Instance.BreakStone, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                    SoundManager.PlayNetworkedAtPos(_commonSound.BreakStone, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                    bot.SetTrigger("DIALOG");
                    break;
                case Phase.Light:
                    //SoundManager.PlayNetworkedAtPos(CommonSounds.Instance.ElectricShock, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                    SoundManager.PlayNetworkedAtPos(_commonSound.ElectricShock, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                    bot.SetTrigger("DIALOG");
                    break;
                case Phase.NoAir:
                    //SoundManager.PlayNetworkedAtPos(CommonSounds.Instance.ExplosionDistant1, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                    SoundManager.PlayNetworkedAtPos(_commonSound.ExplosionDistant1, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                    bot.SetTrigger("DIALOG");
                    break;
                case Phase.Evac:
                    //SoundManager.PlayNetworkedAtPos(CommonSounds.Instance.ExplosionCreak1, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                    SoundManager.PlayNetworkedAtPos(_commonSound.ExplosionCreak1, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                    bot.SetTrigger("DIALOG");
                    break;
                case Phase.Food:
					//SoundManager.PlayNetworkedAtPos(CommonSounds.Instance.EatFood, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
					SoundManager.PlayNetworkedAtPos(_commonSound.EatFood, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
					bot.SetTrigger("DIALOG");
					break;
                case Phase.Heal:
					//SoundManager.PlayNetworkedAtPos(CommonSounds.Instance.EatFood, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
					SoundManager.PlayNetworkedAtPos(_commonSound.EatFood, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
					bot.SetTrigger("DIALOG");
					break;
                case Phase.Id:
					SoundManager.PlayNetworkedAtPos(_commonSound.BreakStone, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
					bot.SetTrigger("DIALOG");
					break;
                case Phase.Leave:
					SoundManager.PlayNetworkedAtPos(_commonSound.ExplosionCreak1, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
					bot.SetTrigger("DIALOG");
					break;
                case Phase.Mask:
					SoundManager.PlayNetworkedAtPos(_commonSound.GlassKnock, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
					bot.SetTrigger("DIALOG");
					break;
                case Phase.FireArm:
					bot.SetTrigger("DIALOG");
					break;
				case Phase.Farm:
					bot.SetTrigger("DIALOG");
					break;
				case Phase.SpawnMove:
					bot.SetTrigger("DIALOG");
					break;
			}
        }
        else
        {
            GameManager.Instance.onTuto = false;
            GameManager.Instance.DisconnectExpected = true;
            CustomNetworkManager.Instance.StopHost();
            SceneManager.LoadScene("Lobby");
        }
    }
}
