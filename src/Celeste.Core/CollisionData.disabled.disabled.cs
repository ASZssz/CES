#pragma warning disable
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste;

public struct CollisionData
{
	public Vector2 Direction;

	public Vector2 Moved;

	public Vector2 TargetPosition;

	public Entity Hit; // Was Platform, now Entity

	public Solid Pusher;

	public static readonly CollisionData Empty;
}
#pragma warning restore
