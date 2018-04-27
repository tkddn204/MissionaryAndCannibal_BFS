using System;

namespace State {
	public enum People
	{
		READY,

		// LEFT TO RIGHT
		LEFT_RIDDEN_BOAT, MOVING_FROM_LEFT_TO_RIGHT,

		// RIGHT TO LEFT
		RIGHT_RIDDEN_BOAT, MOVING_FROM_RIGHT_TO_LEFT
	}

	public enum Boat
	{
		LEFT_R, FROM_LEFT_TO_RIGHT_R,
		RIGHT_L, FROM_RIGHT_TO_LEFT_L
	}
}