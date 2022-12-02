using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoActionPhase : Tutorial
{
    public GameObject TutoBot;
    public Transform SpawnPoint;
    public Tutorial TutoParent;
    public Layer FloorLayer;
    public TilemapDamage TMD;
    public TileChangeManager MetaTileMap;
    public Objects.Atmospherics.AirController AirController;
    public Objects.Wallmounts.FireAlarm FireAlarm;
    public Objects.Lighting.LightSource Light1;
    public Objects.Lighting.LightSource Light2;
    
    ///change phase + send message
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 8)
        {
            TutoParent.TutoPhase = this.TutoPhase;
            
            if(TutoPhase != Phase.SpawnMove)
            {
                Message(Tutorial.botGO);
                if(this.deleteGO)
                    Destroy(this.gameObject);
            }
        }
    }

    ///starting phase
    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 8 && TutoPhase == Phase.SpawnMove)
        {
            SpawnTutoBot();
        }
    }

    ///SPAWN PHASE
    private void SpawnTutoBot()
    {
        if(GameObject.Find("TutoBot") == null)
        {
            var interactableTiles = InteractableTiles.TryGetNonSpaceMatrix(new Vector3Int(18,62, 0), true).TileChangeManager.InteractableTiles;
            SpawnResult bot = Spawn.ServerPrefab(TutoBot, SpawnPoint.position, null, Quaternion.identity);
            Tutorial.botGO = bot.GameObject;
            Tutorial.botGO.GetComponent<Systems.MobAIs.MobFollow>().StartFollowing(PlayerList.Instance.InGamePlayers[0].GameObject);
            Tutorial.botGO.GetComponent<TutoBot>().tuto = TutoParent;
            this.Message(bot.GameObject);
            Tutorial.NetworkTabsGO = GameObject.Find("NetworkTabs (Top Right windows)");
            Tutorial.AdminUIGO = GameObject.Find("AdminUI");
            Tutorial.NetworkTabsGO.SetActive(false);
            Tutorial.AdminUIGO.SetActive(false);
            StartCoroutine(DestroyFloorTile(interactableTiles));
            Light1.ServerChangeLightState(ScriptableObjects.LightMountState.Broken);
            Light2.ServerChangeLightState(ScriptableObjects.LightMountState.Broken);
            TutoParent.IsInGame = true;
        }
        
    }

    private IEnumerator DestroyFloorTile(InteractableTiles interactableTiles)
    {
        yield return new WaitForSeconds(.5f);
        //interactableTiles.FloorLayer.TilemapDamage.ApplyDamage(1000f,AttackType.Bomb ,new Vector3Int(18,62,0));
        MetaTileMap.InteractableTiles.MetaTileMap.RemoveTile(MetaTileMap.InteractableTiles.WorldToCell(new Vector2Int(18, 62)));
        MetaTileMap.InteractableTiles.MetaTileMap.RemoveTile(MetaTileMap.InteractableTiles.WorldToCell(new Vector2Int(19, 62)));
        
        for(int i = 1; i < 5; i++)
        {
            MetaTileMap.InteractableTiles.MetaTileMap.RemoveTile(MetaTileMap.InteractableTiles.WorldToCell(new Vector2Int(14, 60 + i)));
            MetaTileMap.InteractableTiles.MetaTileMap.RemoveTile(MetaTileMap.InteractableTiles.WorldToCell(new Vector2Int(15, 60 + i)));
        }
        yield return new WaitForSeconds(4f);
        for(int j = 1; j < 5; j++)
        {
            MetaTileMap.InteractableTiles.MetaTileMap.SetTile(MetaTileMap.InteractableTiles.WorldToCell(new Vector2Int(14,60 + j)), TileType.Wall, "ReinforcedWall");
            MetaTileMap.InteractableTiles.MetaTileMap.SetTile(MetaTileMap.InteractableTiles.WorldToCell(new Vector2Int(15,60 + j)), TileType.Wall, "ReinforcedWall");
        }
        
        //MetaTileMap.InteractableTiles.ServerProcessInteraction(PlayerManager.LocalPlayerObject, MetaTileMap.InteractableTiles.WorldToCell(new Vector2Int(18, 62)).To2Int(), null, PlayerList.Instance.InGamePlayers[0].Script.DynamicItemStorage.GetActiveHandSlot(),PlayerList.Instance.InGamePlayers[0].Script.DynamicItemStorage.GetActiveHandSlot().ItemObject, Intent.Help,TileApply.ApplyType.HandApply);
        MetaTileMap.InteractableTiles.MetaTileMap.SetTile(interactableTiles.WorldToCell(new Vector2Int(19,62)), TileType.Base, "Lattice");
        
        AirController.RequestImmediateUpdate();
        FireAlarm.UpdateMe();
        if(this.deleteGO)
            Destroy(this.gameObject);
    }
}
