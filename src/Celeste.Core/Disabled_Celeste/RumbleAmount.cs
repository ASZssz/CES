#pragma warning disable
using System.Xml.Serialization;

namespace Celeste;

public enum RumbleAmount
{
	[XmlEnum("false")]
	Off,
	Half,
	[XmlEnum("true")]
	On
}
#pragma warning restore
