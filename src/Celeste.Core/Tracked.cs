#pragma warning disable
using System;

namespace Monocle;

public class Tracked : Attribute
{
	public bool Inherited;

	public Tracked(bool inherited = false)
	{
		Inherited = inherited;
	}
}
#pragma warning restore
