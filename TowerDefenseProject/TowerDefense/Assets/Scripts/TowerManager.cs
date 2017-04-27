using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;

public class TowerManager : Singleton<TowerManager> {

	public TowerBtn towerBtnPressed{get; set;} 
	
	private List<Tower> TowerList = new List<Tower>();
	private List<Collider2D> BuildList = new List<Collider2D>();
	private Collider2D BuildTile;
	private SpriteRenderer towerSpriteRender;
	private Color bad = new Color(1,0,0,0.5f);
	private Color good = new Color(0,1,0,0.5f);

	// Use this for initialization
	void Start () {
		towerSpriteRender = GetComponent<SpriteRenderer>();
		towerSpriteRender.enabled = false;
		BuildTile = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) && towerBtnPressed != null)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider.tag == "BuildSite" && GetTotalMoney() >= GetTowerPrice())
            {
        		BuildTile = hit.collider;
				hit.collider.tag = "BuildSiteFull";

				RegisterBuildsite(BuildTile);
				placeTower(hit);
            }
        }

        if (towerSpriteRender.enabled) {
				followMouse();
				transform.position = new Vector2(transform.position.x, transform.position.y);
		}
	}

    private int GetTowerPrice()
    {
        return GetTowerBtnPressed().TowerPrice;
    }

    private TowerBtn GetTowerBtnPressed()
    {
        return towerBtnPressed;
    }

    private static int GetTotalMoney()
    {
        return GameManager.Instance.TotalMoney;
    }

	public void RegisterBuildsite(Collider2D buildTag) {
		BuildList.Add(buildTag);
	}

	public void RegisterTower(Tower tower) {
		TowerList.Add(tower);
	}

	public  void RenameTagsBuildSites() {
		foreach(Collider2D buildTag in BuildList) {
			buildTag.tag = "BuildSite";
		}
		BuildList.Clear();
	}

	public void DestroyAllTowers() {
		foreach(Tower tower in TowerList) {
			Destroy(tower.gameObject);
		}
		TowerList.Clear();
	}

    public void placeTower(RaycastHit2D hit) {
		if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null) {
			Tower newTower = Instantiate(towerBtnPressed.TowerObject);
			newTower.transform.position = hit.transform.position;
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);
			buyTower(towerBtnPressed.TowerPrice);
			RegisterTower(newTower);
			disableDragSprite();
			towerBtnPressed = null;
		}
	}

	public void buyTower(int price) {
		GameManager.Instance.subtractMoney(price);
	}

	public void selectedTower(TowerBtn towerSelected) {
		if(GameManager.Instance.gameStarted) {
			towerBtnPressed = towerSelected;
			enableDragSprite(towerBtnPressed.DragSprite);
		}
		
	}

	public void followMouse() {
		transform.position= Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D checkSpot = Physics2D.Raycast(worldPoint,Vector2.zero);
		if(checkSpot.collider != null) {
			if(checkSpot.collider.tag == "BuildSite" && GameManager.Instance.TotalMoney >= towerBtnPressed.TowerPrice) {
				towerSpriteRender.color = good;
			} else {
				towerSpriteRender.color = bad;
			}
		}
	}

	public void enableDragSprite(Sprite sprite) {
		towerSpriteRender.enabled = true;
		towerSpriteRender.sprite = sprite;
	
		
	}

	public void disableDragSprite() {
		towerSpriteRender.enabled = false;
		towerBtnPressed = null;
	}

}
