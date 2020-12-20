using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    private enum Mode {MatchOnly, FreeMove};

	[SerializeField] private Mode mode; // два режима перемещения, 'MatchOnly' означает, что передвинуть узел можно если произошло совпадение, иначе произойдет возврат
	[SerializeField] private float speed = 5.5f; // скорость движения объектов
	[SerializeField] private float destroyTimeout = .5f; // пауза в секундах, перед тем как уничтожить совпадения
	[SerializeField] private LayerMask layerMask; // маска узла (префаба)
	[SerializeField] private Color[] color; // набор цветов/id
	[SerializeField] private int gridWidth = 7; // ширина игрового поля
	[SerializeField] private int gridHeight = 10; // высота игрового поля
	[SerializeField] private Node sampleObject; // образец узла (префаб)
	[SerializeField] private float sampleSize = 1; // размер узла (ширина и высота)

	private Node[,] grid;
	private Node[] nodeArray;
	private Vector3[,] position;
	private Node current, last;
	private Vector3 currentPos, lastPos;
	private List<Node> lines;
	private bool isLines, isMove, isMode;
	private float timeout;

	void Start()
	{
		// создание игрового поля (2D массив) с заданными параметрами
		grid = Create2DGrid(sampleObject, gridWidth, gridHeight, sampleSize, transform);

		SetupField();
	}

	void SetupField() // стартовые установки, подготовка игрового поля
	{
		position = new Vector3[gridWidth, gridHeight];
		nodeArray = new Node[gridWidth * gridHeight];

		int i = 0;
		int id = -1;
		int step = 0;

		for(int y = 0; y < gridHeight; y++)
		{
			for(int x = 0; x < gridWidth; x++)
			{
				int j = Random.Range(0, color.Length);
				if(id != j) id = j; else step++;
				if(step > 2)
				{
					step = 0;
					id = (id + 1 < color.Length-1) ? id + 1 : id - 1;
				}

				grid[x, y].Ready = false;
				grid[x, y].X = x;
				grid[x, y].Y = y;
				grid[x, y].Id = id;
				grid[x, y].sprite.color = color[id];
				grid[x, y].gameObject.SetActive(true);
				grid[x, y].highlight.SetActive(false);
				position[x, y] = grid[x, y].transform.position;
				nodeArray[i] = grid[x, y];
				i++;
			}
		}

		current = null;
		last = null;
	}

	void DestroyLines() // уничтожаем совпадения с задержкой
	{
		if(!isLines) return;

		timeout += Time.deltaTime;

		if(timeout > destroyTimeout)
		{
			for(int i = 0; i < lines.Count; i++)
			{
				// здесь можно подсчитывать очки +1
				lines[i].gameObject.SetActive(false);
				grid[lines[i].X, lines[i].Y] = null;

				for(int y = lines[i].Y-1; y >= 0; y--)
				{
					if(grid[lines[i].X, y] != null)
					{
						//grid[lines[i].X, y].move = true;
					}
				}
			}

			isMove = true;
			isLines = false;
		}
	}

	void MoveNodes() // передвижение узлов и заполнение поля, после проверки совпадений
	{
		if(!isMove) return;

		for(int y = 0; y < gridHeight; y++)
		{
			for(int x = 0; x < gridWidth; x++)
			{
				if(grid[x, 0] == null)
				{
					bool check = true;

					for(int i = 0; i < gridWidth; i++)
					{
						if(grid[i, 0] == null)
						{
							grid[i, 0] = GetFree(position[i, 0]);
						}
					}

					for(int i = 0; i < nodeArray.Length; i++)
					{
						if(!nodeArray[i].gameObject.activeSelf) check = false;
					}

					if(check)
					{
						isMove = false;
						GridUpdate();

						if(IsLine())
						{
							timeout = 0;
							isLines = true;
						}
						else
						{
							isMode = false;
						}
					}
				}

				if(grid[x, y] != null)
				if(y+1 < gridHeight && grid[x, y].gameObject.activeSelf && grid[x, y+1] == null)
				{
					grid[x, y].transform.position = Vector3.MoveTowards(grid[x, y].transform.position, position[x, y+1], speed * Time.deltaTime);

					if(grid[x, y].transform.position == position[x, y+1])
					{
						grid[x, y+1] = grid[x, y];
						grid[x, y] = null;
					}
				}
			}
		}
	}

	void Update()
	{
		DestroyLines();

		MoveNodes();

		if(isLines || isMove) return;

		if(last == null)
		{
			Control();
		}
		else
		{
			MoveCurrent();
		}
	}

	Node GetFree(Vector3 pos) // возвращает неактивный узел
	{
		for(int i = 0; i < nodeArray.Length; i++)
		{
			if(!nodeArray[i].gameObject.activeSelf)
			{
				int j = Random.Range(0, color.Length);
				nodeArray[i].Id = j;
				nodeArray[i].sprite.color = color[j];
				nodeArray[i].transform.position = pos;
				nodeArray[i].gameObject.SetActive(true);
				return nodeArray[i];
			}
		}

		return null;
	}

	void GridUpdate() // обновление игрового поля с помощью рейкаста
	{
		for(int y = 0; y < gridHeight; y++)
		{
			for(int x = 0; x < gridWidth; x++)
			{
				RaycastHit2D hit = Physics2D.Raycast(position[x, y], Vector2.zero, Mathf.Infinity, layerMask);

				if(hit.transform != null)
				{
					grid[x, y] = hit.transform.GetComponent<Node>();
					grid[x, y].Ready = false;
					grid[x, y].X = x;
					grid[x, y].Y = y;	
				}
			}
		}
	}

	void MoveCurrent() // перемещение выделенного мышкой узла
	{
		current.transform.position = Vector3.MoveTowards(current.transform.position, lastPos, speed * Time.deltaTime);
		last.transform.position = Vector3.MoveTowards(last.transform.position, currentPos, speed * Time.deltaTime);

		if(current.transform.position == lastPos && last.transform.position == currentPos)
		{
			GridUpdate();

			if(mode == Mode.MatchOnly && isMode && !CheckNearNodes(current) && !CheckNearNodes(last))
			{
				currentPos = position[current.X, current.Y];
				lastPos = position[last.X, last.Y];
				isMode = false;
				return;
			}
			else
			{
				isMode = false;
			}

			current = null;
			last = null;

			if(IsLine())
			{
				timeout = 0;
				isLines = true;
			}
		}
	}

	bool CheckNearNodes(Node node) // проверка, возможно-ли совпадение на текущем ходу
	{
		if(node.X-2 >= 0) 
			if(grid[node.X-1, node.Y].Id == node.Id && grid[node.X-2, node.Y].Id == node.Id) return true;

		if(node.Y-2 >= 0) 
			if(grid[node.X, node.Y-1].Id == node.Id && grid[node.X, node.Y-2].Id == node.Id) return true;

		if(node.X+2 < gridWidth) 
			if(grid[node.X+1, node.Y].Id == node.Id && grid[node.X+2, node.Y].Id == node.Id) return true;

		if(node.Y+2 < gridHeight) 
			if(grid[node.X, node.Y+1].Id == node.Id && grid[node.X, node.Y+2].Id == node.Id) return true;

		if(node.X-1 >= 0 && node.X+1 < gridWidth) 
			if(grid[node.X-1, node.Y].Id == node.Id && grid[node.X+1, node.Y].Id == node.Id) return true;

		if(node.Y-1 >= 0 && node.Y+1 < gridHeight) 
			if(grid[node.X, node.Y-1].Id == node.Id && grid[node.X, node.Y+1].Id == node.Id) return true;

		return false;
	}

	void SetNode(Node node, bool value) // метка для узлов, которые находятся рядом с выбранным (чтобы нельзя было выбрать другие)
	{
		if(node == null) return;

		if(node.X-1 >= 0) grid[node.X-1, node.Y].Ready = value;
		if(node.Y-1 >= 0) grid[node.X, node.Y-1].Ready = value;
		if(node.X+1 < gridWidth) grid[node.X+1, node.Y].Ready = value;
		if(node.Y+1 < gridHeight) grid[node.X, node.Y+1].Ready = value;
	}

	void Control() // управление ЛКМ
	{
		if(Input.GetMouseButtonDown(0) && !isMode)
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

			if(hit.transform != null && current == null)
			{
				current = hit.transform.GetComponent<Node>();
				SetNode(current, true);
				current.highlight.SetActive(true);
			}
			else if(hit.transform != null && current != null)
			{
				last = hit.transform.GetComponent<Node>();

				if(last != null && !last.Ready)
				{
					current.highlight.SetActive(false);
					last.highlight.SetActive(true);
					SetNode(current, false);
					SetNode(last, true);
					current = last;
					last = null;
					return;
				}

				current.highlight.SetActive(false);
				currentPos = current.transform.position;
				lastPos = last.transform.position;
				isMode = true;
			}
		}
	}

	bool IsLine() // поиск совпадений по горизонтали и вертикали
	{
		int j = -1;

		lines = new List<Node>();

		for(int y = 0; y < gridHeight; y++)
		{
			for(int x = 0; x < gridWidth; x++)
			{
				if(x+2 < gridWidth && j < 0 && grid[x+1,y].Id == grid[x,y].Id && grid[x+2,y].Id == grid[x,y].Id)
				{
					j = grid[x,y].Id;
				}

				if(j == grid[x,y].Id)
				{
					lines.Add(grid[x,y]);
				}
				else
				{
					j = -1;
				}
			}

			j = -1;
		}

		j = -1;

		for(int y = 0; y < gridWidth; y++)
		{
			for(int x = 0; x < gridHeight; x++)
			{
				if(x+2 < gridHeight && j < 0 && grid[y,x+1].Id == grid[y,x].Id && grid[y,x+2].Id == grid[y,x].Id)
				{
					j = grid[y,x].Id;
				}

				if(j == grid[y,x].Id)
				{
					lines.Add(grid[y,x]);
				}
				else
				{
					j = -1;
				}
			}

			j = -1;
		}

		return (lines.Count > 0) ? true : false;
	}


	// функция создания 2D массива на основе шаблона
	private Node[,] Create2DGrid(Node sample, int width, int height, float size, Transform parent)
	{
		var field = new Node[width, height];

		float posX = -size * width / 2f - size / 2f;
		float posY = size * height / 2f - size / 2f;

		float Xreset = posX;

		int z = 0;

		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				posX += size;
				field[x, y] = Instantiate(sample, new Vector3(posX, posY, 0), Quaternion.identity, parent) as Node;
				field[x, y].name = "Node-" + z;
				z++;
			}
			posY -= size;
			posX = Xreset;
		}

		return field;
	}
}
