using UnityEngine;
using System.Collections;

public class GameCell
{
	
	public void Destroy()
	{
		GameObject.Destroy(m_cell);
		m_cellScript = null;
	}

	public void Set(int status)
	{
		m_cellScript.m_status = status; //set it
		
		if (GetStatus() == G.CELL_X)
		{
			m_cell.renderer.material.mainTexture = G.Get().m_cellTex_x;
		} else
		if (GetStatus() == G.CELL_O)
		{
			m_cell.renderer.material.mainTexture = G.Get().m_cellTex_o;
		} else
		{
			//default
			m_cell.renderer.material.mainTexture = G.Get().m_cellTex;
		}

	}
	
	public int GetStatus() {return m_cellScript.m_status;}
	
	public GameObject m_cell;	
	public Cell m_cellScript;
}

public class Board  
{
	const int C_BOARD_SIZE = 9;
	public const int C_CELL_SIZE = 3;
	const int C_BOARD_WIDTH = 3;

	GameCell[] m_board;
		
	int GetCellIndex(int x, int y)
	{
		return y*C_BOARD_WIDTH+x;	
	}
	
	public GameCell GetCell(int index)
	{
		return m_board[index];
	}
	
	public GameCell GetCell(int x, int y)
	{
		return m_board[GetCellIndex(x,y)];
	}

	public void CreateBoard()
	{
		if (m_board != null)
		{
			//kill the existing board
			for (int i=0; i < C_BOARD_SIZE; i++)
			{
				m_board[i].Destroy();
				m_board[i] = null;
			}
			
			m_board = null;
			
		}
		Debug.Log("Creating board");
		
		m_board = new GameCell[C_BOARD_SIZE];

		float startX = -((C_CELL_SIZE*C_BOARD_WIDTH)/2);
		float startY = -((C_CELL_SIZE*C_BOARD_WIDTH)/2);
			
		for (int x = 0; x < C_BOARD_WIDTH; x++)
		{
			for (int y = 0; y < C_BOARD_WIDTH; y++)
			{
				
				GameCell c = new GameCell();
				c.m_cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
				c.m_cellScript = c.m_cell.AddComponent("Cell") as Cell;
				//c.m_cell.AddComponent("MeshFlasher");
			 
				c.m_cell.transform.position = new Vector3(startX+x*C_CELL_SIZE,startY+y*C_CELL_SIZE,0);
				c.m_cell.transform.localScale = new Vector3(C_CELL_SIZE,C_CELL_SIZE,0);
			
				c.m_cellScript.m_x = x;
				c.m_cellScript.m_y = y;

				c.Set(G.CELL_EMPTY);

				//put it in its slot:
				m_board[GetCellIndex(x,y)] = c;
			}
		}
	}
	
	public bool DidWin()
	{
		//check vertical rows
	
		int[] match = new int[C_BOARD_WIDTH];
		
		int matchStatus = G.CELL_EMPTY;
		
		for (int x = 0; x < C_BOARD_WIDTH; x++)
		{
			int temp =  GetCell (x,0).GetStatus();
			match[0] = GetCellIndex(x,0);
			
			for (int y=1; y < C_BOARD_WIDTH; y++)
			{
				if (GetCell (x,y).GetStatus() == temp)
				{
					//we seem to still have a match..
					match[y] =  GetCellIndex(x,y);
					matchStatus = GetCell (x,y).GetStatus();
				} else
				{
					matchStatus = G.CELL_EMPTY;
					break; //nope	
				}
			}
			
			if (matchStatus != G.CELL_EMPTY) break; //quit now, we have a winner
		}
		
		if (matchStatus == G.CELL_EMPTY)
		{
			//horizontal
			for (int y = 0; y < C_BOARD_WIDTH; y++)
			{
				int temp =  GetCell (0,y).GetStatus();
				match[0] = GetCellIndex(0,y);
				
				for (int x=1; x < C_BOARD_WIDTH; x++)
				{
					if (GetCell (x,y).GetStatus() == temp)
					{
						//we seem to still have a match..
						match[x] =  GetCellIndex(x,y);
						matchStatus = GetCell (x,y).GetStatus();
					} else
					{
						matchStatus = G.CELL_EMPTY;
						break; //nope	
					}
				}
				
				if (matchStatus != G.CELL_EMPTY) break; //quit now, we have a winner
			}
		}
		
		
		if (matchStatus == G.CELL_EMPTY)
		{
			if (C_BOARD_WIDTH == 3)
			{
				//diagonal .. hardcoded for 3x3 board
				if (GetCell (0,0).GetStatus() != G.CELL_EMPTY &&
					GetCell (0,0).GetStatus() == 
					GetCell (1,1).GetStatus() && GetCell (0,0).GetStatus() ==
					GetCell (2,2).GetStatus())
				{
					matchStatus = GetCell (0,0).GetStatus();
					match[0] = GetCellIndex(0,0);
					match[1] = GetCellIndex(1,1);
					match[2] = GetCellIndex(2,2);
					
					//yup	
				} else
				if (GetCell (2,0).GetStatus() == 
					GetCell (1,1).GetStatus() && GetCell (2,0).GetStatus() ==
					GetCell (0,2).GetStatus())
				{
					matchStatus = GetCell (2,0).GetStatus();
					match[0] = GetCellIndex(2,0);
					match[1] = GetCellIndex(1,1);
					match[2] = GetCellIndex(0,2);
				}
			
			}
			
		}
		if (matchStatus != G.CELL_EMPTY)
		{
			
			for (int i=0; i < C_BOARD_WIDTH; i++)
			{
				GetCell(match[i]).m_cell.AddComponent("MeshFlasher");
			}
			return true;	
		}
		
		return false;
		//return true;
	}
	
	public int CountEmptyCells()
	{
		int empty = 0;
		for (int i=0; i < C_BOARD_SIZE; i++)
		{
			if (m_board[i].m_cellScript.m_status == G.CELL_EMPTY) empty++;
		}
		
		return empty;
	}
	
	public int GetRandomEmptyCell()
	{
		if (CountEmptyCells() < 1)
		{
			Debug.LogError("No empty cells!");
			return 0; //game over	
		}
		
		while (true)
		{
			
			int r = Random.Range(0, C_BOARD_SIZE);
			
			if ( m_board[r].m_cellScript.m_status == G.CELL_EMPTY)
			{
				return r;
			}
		}
	}
	
	
}
