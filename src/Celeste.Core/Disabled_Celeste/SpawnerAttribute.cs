#pragma warning disable
using System;

namespace Celeste;

public class SpawnerAttribute : Attribute
{
	public string Name;

	public SpawnerAttribute(string name = null)
	{
		Name = name;
	}
}
#pragma warning restore
