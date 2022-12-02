using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Messages.Server.SoundMessages;

public class Tutorial : MonoBehaviour
{

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

    public Phase TutoPhase;

    public static Lang_Bot LangBot;
    private static Lang_Bot defaultEN;
    public bool IsInGame;
    [SerializeField] private List<AddressableReferences.AddressableAudioSource> TutoSound;

    public static GameObject NetworkTabsGO;
    public static GameObject AdminUIGO;

    private int SoundCD = 0;
    [SerializeField] private int ui = 15;
    private void Start()
    {
        //load languages file
        LangBot = new Lang_Bot(Path.Combine(Application.streamingAssetsPath, "languages/Lang_Bot_"+GameManager.Instance.language+".xml"), GameManager.Instance.language);
        defaultEN = new Lang_Bot(Path.Combine(Application.streamingAssetsPath, "languages/Lang_Bot_English.xml"), "English");
        //UI.ControlTabs.Instance.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(IsInGame == true)
        {
            if(SoundCD > ui / Time.deltaTime)
            {
                int randomAmbi = Random.Range(0, 3);
                AudioSourceParameters audioSourceParameters = new AudioSourceParameters(pitch: UnityEngine.Random.Range(0.8f, 1.2f));
                SoundManager.PlayNetworkedAtPos(TutoSound[randomAmbi], PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                SoundCD = 0;
            }
            else
            {
                SoundCD += 1;
            }
        }
    }

    ///Send message to chat depending on the phase
    public void Message(GameObject GO)
    {
        int randomBotBubble = Random.Range(0,2);
            if(randomBotBubble == 0)
                ChatBubbleManager.ShowAChatBubble(Tutorial.botGO.transform, "<btzzt>", ChatModifier.Mute);
            else
                ChatBubbleManager.ShowAChatBubble(Tutorial.botGO.transform, "<!!!>", ChatModifier.Mute);
        if(TutoPhase != Phase.Leave)
        {
            string message = Tutorial.LangBot.GetString(TutoPhase.ToString());
            if(message == "" || message == null)   
                message = Tutorial.defaultEN.GetString(TutoPhase.ToString());

            Chat.AddLocalMsgToChat(message, GO);
            
            AudioSourceParameters audioSourceParameters = new AudioSourceParameters(pitch: UnityEngine.Random.Range(0.8f, 1.2f));
            switch(TutoPhase)
            {
                case Phase.SpawnMove:
                    SoundManager.PlayNetworkedAtPos(CommonSounds.Instance.ambigen8, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                    SoundManager.PlayNetworkedAtPos(TutoSound[3], PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                break;
                case Phase.Evac :
                    SoundManager.PlayNetworkedAtPos(CommonSounds.Instance.ExplosionCreak1, PlayerList.Instance.InGamePlayers[0].GameObject.RegisterTile().WorldPosition, audioSourceParameters, sourceObj: gameObject);
                break;
            }
        }
        else
        {
            GameManager.Instance.onTuto = false;
            GameManager.Instance.DisconnectExpected = true;
            Tutorial.NetworkTabsGO.SetActive(true);
            Tutorial.AdminUIGO.SetActive(true);
            CustomNetworkManager.Instance.StopHost();
            SceneManager.LoadScene("Lobby");
        }
    }
}
