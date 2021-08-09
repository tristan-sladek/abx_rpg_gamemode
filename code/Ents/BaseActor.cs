using Sandbox;

namespace Sandbox
{
	partial class BaseActor : AnimEntity
	{
		[Net] public string ActorName { get; set; }		
		[Net] public float HP { get; set; } //Temporary
		[Net] public float MP { get; set; }
	}
}
