namespace Fragsurf.Movement
{
	public static class IntExtensions
	{
		public static bool HasFlag(this int a, int b)
		{
			return (a & b) == b;
		}

		// Unity 5.4.1f1's mscorlib doesn't have System.Enum.HasFlag (added in .NET 4.0), so
		// calls like `someInputActions.HasFlag(InputActions.Jump)` can't bind to a built-in
		// method and fall through to extension resolution instead - but InputActions doesn't
		// implicitly convert to int, so the (this int, int) overload above doesn't match either.
		// This overload gives InputActions.HasFlag(InputActions) the same bitwise-AND behavior
		// the built-in Enum.HasFlag would have provided.
		public static bool HasFlag(this InputActions a, InputActions b)
		{
			return (a & b) == b;
		}

		public static int AddFlag(this int a, int b)
		{
			return a |= b;
		}

		public static int RemoveFlag(this int a, int b)
		{
			return a &= ~b;
		}
	}
}
