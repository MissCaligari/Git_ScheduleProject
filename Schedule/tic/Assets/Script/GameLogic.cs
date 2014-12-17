using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{
	
	public Board m_board = new Board();
	
	enum eStatus
	{
		X_TURN,
		O_TURN,
		X_WINNER,
		O_WINNER,
		DRAW
	};
	
	eStatus m_status;
	
	string GetName()
	{
		return gameObject.name;
	}
	
	void Start ()
	{
		m_status = eStatus.O_TURN;
		m_board.CreateBoard();

		//start the game in half a second
		RTEventManager.Get().Schedule(RTAudioManager.GetName(), "Play", 0.5f, "start_game");

		//show this message in 1 second
		RTEventManager.Get().Schedule(GetName(), "Msg", 1, "Welcome to the most advanced Tic Tac Toe Simulation ever built.  Watch your ass, Carmack. Click square to move.");
	}

	void OnGameOver()
	{
		RTAudioManager.Get().Play("game_over"); //no need to use RTEventManager to schedule it as now is fine
		RTEventManager.Get().Schedule(GetName(), "Start", 4); //schedule a new game to start in 4 seconds
	}
	
	void DoAITurn()
	{
		
		if (m_board.CountEmptyCells() < 1)
		{
			RTEventManager.Get().Schedule(GetName(), "Msg", 1, "GAME OVER, OBVIOUSLY");
			m_status = eStatus.O_WINNER;
			return; //game over	
		}
		
		int cellIndex = m_board.GetRandomEmptyCell();
		
		//ok, we chose, apply it
		m_board.GetCell(cellIndex).Set(G.CELL_X);
			
		m_status = eStatus.O_TURN;
		RTEventManager.Get().Schedule(RTAudioManager.GetName(), "PlayEx", 0, new RTDB("fileName", "chalk",
			"volume", 1.0f, "pitch", 2.0f));
				
		
		if (m_board.DidWin())
		{
				RTEventManager.Get().Schedule(GetName(), "Msg", 0.4f, "COMPUTER HAS WON, AGAIN");
				m_status = eStatus.X_WINNER;
				OnGameOver();
				return;
		}
					
		if (m_board.CountEmptyCells() < 1)
		{
			OnItsaDraw();
			return; //game over	
		}

		string msg = "Computer has moved.  Your turn again, player 1";
					
		int r = Random.Range(0, 2);
					
		switch (r)
		{
			case 0: msg = "Computer has moved.  Your turn";	 break;
			case 1: msg = "It's hopeless, but your turn";	 break;
			case 2: msg = "Finished move.  Human's turn again";	 break;
		}
					
		RTEventManager.Get().Schedule(GetName(), "Msg", 0.4f, msg );
			
	}
	
	void OnItsaDraw()
	{
		RTEventManager.Get().Schedule(GetName(), "Msg", 1, "IT'S A DRAW, DILLWEED");
		m_status = eStatus.DRAW;
		OnGameOver();
	}
	
	// Update is called once per frame
	
	void DoPlayerInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast(ray.origin, ray.direction, out hit)) 
			{
				
				Cell cellScript = hit.collider.gameObject.GetComponent("Cell") as Cell;
			
				if (cellScript.m_status == G.CELL_EMPTY)
				{
					hit.collider.gameObject.renderer.material.mainTexture = G.Get().m_cellTex_o;
					cellScript.m_status = G.CELL_O;
			
					RTEventManager.Get().Schedule(RTAudioManager.GetName(), "PlayEx", 0, new RTDB("fileName", "chalk"));
				
					if (m_board.DidWin())
					{
						
						RTEventManager.Get().Schedule(GetName(), "Msg", 0.4f, "YOU HAVE WON THE GAME!");
						m_status = eStatus.O_WINNER;		
						OnGameOver();
						return;
					}
					
					if (m_board.CountEmptyCells() < 1)
					{
						OnItsaDraw();
						return; //game over	
					}
					
					
					string msg = "The computer is thinking...";
					
					int r = Random.Range(0, 2);
					
					switch (r)
					{
						case 0: msg = "Computer is calculating min-max tree...";	 break;
						case 1: msg = "Computer is solving, please wait";	 break;
						case 2: msg = "Computer is evaluating move tree, please wait";	 break;
					}
					
					RTEventManager.Get().Schedule(GetName(), "Msg", 0.4f, msg);
					
					float AITimer = 1+Random.Range(1.0f,2.0f);
					
					if (Debug.isDebugBuild)
					{
						//if we want to avoid the slow waiting while in debug mode for quicker testing
						//AITimer = 0.2f;
					}
					
					RTEventManager.Get().Schedule(GetName(), "DoAITurn", AITimer);
					m_status = eStatus.X_TURN;
				} 
				
			}
		}
				
	}
	
	void Update () 
	{
		
		if (m_status == eStatus.O_TURN)
		{
			DoPlayerInput();
		}
	}
}
