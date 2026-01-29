#pragma warning disable
using System.Xml.Serialization;

namespace Celeste;

public enum ScreenshakeAmount
{
	[XmlEnum("false")]
	Off,
	[XmlEnum("true")]
	Half,
	On
}
#pragma warning restore
