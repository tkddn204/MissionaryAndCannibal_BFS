using System.Collections;
using System.Collections.Generic;
using State;

public class GameState
{
	public State.People peopleState;

	public int leftMissionaryNumber;
	public int leftCannibalNumber;
	public int boatMissionaryNumber;
	public int boatCannibalNumber;
	public int rightMissionaryNumber;
	public int rightCannibalNumber;
	public State.Boat boatState;

	public GameState(int leftMissionaryNumber, int leftCannibalNumber, int rightMissionaryNumber, int rightCannibalNumber, State.Boat boatState)
    {
		peopleState = State.People.READY;
        this.leftMissionaryNumber = leftMissionaryNumber;
        this.leftCannibalNumber = leftCannibalNumber;
        this.rightMissionaryNumber = rightMissionaryNumber;
        this.rightCannibalNumber = rightCannibalNumber;
        this.boatState = boatState;
    }

    public GameState(State.People peopleState, int leftMissionaryNumber, int leftCannibalNumber, int boatMissionaryNumber, int boatCannibalNumber, int rightMissionaryNumber, int rightCannibalNumber, State.Boat boatState)
    {
        this.peopleState = peopleState;
        this.leftMissionaryNumber = leftMissionaryNumber;
        this.leftCannibalNumber = leftCannibalNumber;
        this.boatMissionaryNumber = boatMissionaryNumber;
        this.boatCannibalNumber = boatCannibalNumber;
        this.rightMissionaryNumber = rightMissionaryNumber;
        this.rightCannibalNumber = rightCannibalNumber;
        this.boatState = boatState;
    }

	public State.People PeopleState
	{
		get { return peopleState; }
		set { peopleState = value; }
	}

	public int LeftMissionaryNumber
	{
		get { return leftMissionaryNumber; }
		set { leftMissionaryNumber = value; }
	}

	public int LeftCannibalNumber
	{
		get { return leftCannibalNumber; }
		set { leftCannibalNumber = value; }
	}

	public int BoatMissionaryNumber
	{
		get { return boatMissionaryNumber; }
		set { boatMissionaryNumber = value; }
	}

	public int BoatCannibalNumber
	{
		get { return boatCannibalNumber; }
		set { boatCannibalNumber = value; }
	}

	public int RightMissionaryNumber
	{
		get { return rightMissionaryNumber; }
		set { rightMissionaryNumber = value; }
	}

	public int RightCannibalNumber
	{
		get { return rightCannibalNumber; }
		set { rightCannibalNumber = value; }
	}

	public State.Boat BoatState
	{
		get { return boatState; }
		set { boatState = value; }
	}

	public override string ToString ()
	{
		return string.Format ("[GameState]: {0} // ({1} {2}) - B({3} {4})<{7}> - ({5} {6})",
			PeopleState, LeftMissionaryNumber, LeftCannibalNumber, BoatMissionaryNumber, 
			BoatCannibalNumber, RightMissionaryNumber, RightCannibalNumber, BoatState);
	}

	public string printState()
	{
		int boatDirection;
		if (boatState == Boat.LEFT_R || boatState == Boat.FROM_LEFT_TO_RIGHT_R)
		{
			boatDirection = 1;
		}
		else
		{
			boatDirection = 0;
		}
		return string.Format ("현재 상태 : ({0}, {1}, {2})",
			LeftMissionaryNumber, LeftCannibalNumber, boatDirection);
	}
}
