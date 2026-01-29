#pragma warning disable
// LevelData Stub - For compilation only
using System.Collections.Generic;
namespace Celeste
{
	public class LevelData
	{
		public int ID { get; set; }
		public List<EntityData> Entities { get; set; }
	}
	
	public class DashCollision
	{
		public enum Directions { Up, Down, Left, Right }
	}
}
#pragma warning restore
