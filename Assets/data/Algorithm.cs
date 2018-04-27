using System;
using System.Collections;

namespace MAC
{
	public class State : ICloneable
	{
		public int missionary;
		public int cannibal;
		public bool boatMove;

		public State(int missionary, int cannibal, bool boatMove)
		{
			this.missionary = missionary;
			this.cannibal = cannibal;
			this.boatMove = boatMove;
		}

		/*
         * 목표에 도달했는지 검증하는 메소드입니다.
         * @return 도달한 상태면 true
         */
		public bool isGoal()
		{
			return missionary == 0 && cannibal == 0;
		}

		override
		public string ToString()
		{
			return "Missionary : " + missionary + ", " +
				"Cannibal : " + cannibal + ", " +
				"Boats : " + (boatMove ? "right" : "left");
		}

		public object Clone()
		{
			return new State(missionary, cannibal, boatMove);
		}

		public static bool operator ==(State state1, State state2)
		{
			return state1.missionary == state2.missionary &&
				state1.cannibal == state2.cannibal &&
				state1.boatMove == state2.boatMove;
		}

		public static bool operator !=(State state1, State state2)
		{
			return state1.missionary != state2.missionary ||
				state1.cannibal != state2.cannibal ||
				state1.boatMove != state2.boatMove;
		}

		public override bool Equals(object obj)
		{
			return this == (State)obj;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	class Node
	{
		public int id;
		public int depth;
		public Node parent;
		public State state;

		public Node(int id, int depth, Node parent, State state)
		{
			this.id = id;
			this.depth = depth;
			this.parent = parent;
			this.state = state;
		}

		override
		public string ToString()
		{
			return " [id:" + id + "] " + state;
		}
	}

	class Solve
	{
		public const bool LEFT_BOAT_STATE = false;
		public const bool RIGHT_BOAT_STATE = true;

		public int maxMissionary, maxCannibal, maxRidingBoat;

		private Queue treeQueue = new Queue();
		private ArrayList resultList = new ArrayList();
		private ArrayList resultNodes = new ArrayList(); // 결과 노드(0, 0, 1)들이 들어가는 벡터(리스트)
		private ArrayList visitedStates = new ArrayList(); // 가지치기를 위해서 방문했던 상태를 저장함

		public Solve(int maxMissionary, int maxCannibal, int maxRidingBoat)
		{
			this.maxMissionary = maxMissionary;
			this.maxCannibal = maxCannibal;
			this.maxRidingBoat = maxRidingBoat;
		}

		private bool isValid(int missionary, int cannibal)
		{
			if (missionary >= maxMissionary && cannibal >= maxCannibal)
			{
				return false;
			}

			if ((missionary < 0) || (missionary > maxMissionary)
				|| (cannibal < 0) || (cannibal > maxCannibal))
			{
				return false;
			}

			if ((maxMissionary - missionary < maxCannibal - cannibal)
			    && maxMissionary - missionary > 0 && maxCannibal - cannibal > 0) {
				return false;
			}

			if (missionary < cannibal && missionary > 0)
			{
				return false;
			}

			return true;
		}

		/**
         * BFS 가지치기 방식을 사용하여 문제를 풀어내는 메소드입니다.
         */
		private void PruningBFS(Node parent)
		{
			int tempMissionary;
			int tempCannibal;
			int idNum = parent.id;

			treeQueue.Enqueue(parent);
			resultList.Add(parent);
			visitedStates.Add(parent.state);

			int n = 0;
			while (treeQueue.Count > 0)
			{
				Node currentNode = (Node) treeQueue.Peek();
				Node currentResultNode = (Node) resultList[n++];

				for (int i = 0; i <= maxRidingBoat; i++)
				{
					for (int j = 0; j <= maxRidingBoat; j++)
					{
						if (i + j < 1) continue;
						if (i + j > maxRidingBoat) break;

						State currentNodeState = currentNode.state;
						if (currentNodeState.boatMove == LEFT_BOAT_STATE)
						{
							tempMissionary = -i + currentNodeState.missionary;
							tempCannibal = -j + currentNodeState.cannibal;
						}
						else
						{
							tempMissionary = i + currentNodeState.missionary;
							tempCannibal = j + currentNodeState.cannibal;
						}

						idNum++;
						if (isValid(tempMissionary, tempCannibal))
						{
							State tempState = (State) currentNodeState.Clone();
							tempState.missionary = tempMissionary;
							tempState.cannibal = tempCannibal;
							tempState.boatMove = !currentNodeState.boatMove;

							// 가지치기를 위해 미리 방문했었던 상태들(visitedStates)을 우선 검색
							bool isVisited = false;
							foreach (State state in visitedStates)
							{
								if (state == tempState)
								{
									isVisited = true;
									break;
								}
							}
							if (isVisited) continue;

							Node newNode = new Node(idNum, currentNode.depth + 1,
								currentNode, tempState);
							Node newResultNode = new Node(idNum, currentNode.depth + 1,
								currentResultNode, tempState);
							treeQueue.Enqueue(newNode);
							// emplace_back도 push랑 똑같음
							resultList.Add(newNode);

							Console.WriteLine(newNode);

							if (newNode.state.isGoal())
							{
								// 결과 노드들만 resultNodes에 저장
								resultNodes.Add(newResultNode);
							}
							else
							{
								// 방문했던 상태들만 visitedStates에 저장
								visitedStates.Add(newNode.state);
							}
						}
					}
				}

				treeQueue.Dequeue();
			}
		}

		public void findResult(Node parent)
		{
			PruningBFS(parent);

			int i = 0;
			int total = resultNodes.Count;
			// 결과 노드들에서 포인터 역순으로 올라감
			foreach (Node resultNode in resultNodes)
			{  
				Console.WriteLine(" =============== " + ++i + "번째 해결책입니다." + " ===============");
				Stack printNodeStack = new Stack();
				printNodeStack.Push(resultNode);

				// 역순으로 올라가는 리스트를 printNodeQueue에 저장
				Node currentNode = resultNode;
				while (currentNode.parent != null)
				{
					currentNode = currentNode.parent;
					printNodeStack.Push(currentNode);
				}

				// 역순으로 저장된 큐를 정순으로 프린트
				while (printNodeStack.Count > 0)
				{
					Console.WriteLine(printNodeStack.Peek());
					printNodeStack.Pop();
				}
			}
			Console.WriteLine("총 " + total + "개의 해결책을 찾았습니다.");
		}

		public ArrayList findAndReturnResultList(Node parent)
		{
			PruningBFS (parent);

			int total = resultNodes.Count;
			ArrayList resultFinalList = new ArrayList ();
			// 결과 노드들에서 포인터 역순으로 올라감
			foreach (Node resultNode in resultNodes)
			{
				ArrayList printNodeList = new ArrayList();
				printNodeList.Add (resultNode);

				// 역순으로 올라가는 리스트를 printNodeList에 저장
				Node currentNode = resultNode;
				bool isNotCorrect = false;
				while (currentNode.parent != null)
				{
					currentNode = currentNode.parent;
					if (currentNode.state.isGoal () && currentNode.parent != null) {
						isNotCorrect = true;
						break;
					}
					printNodeList.Add(currentNode);
				}
				if (isNotCorrect) {
					continue;
				}
				printNodeList.Reverse ();
				resultFinalList.Add (printNodeList);
			}

			return resultFinalList;
		}
	}
}
