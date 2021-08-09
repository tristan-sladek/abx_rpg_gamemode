using Sandbox;
using Sandbox.UI;

namespace Sandbox
{ 
	class BaseRPGPawn : BaseActor
	{
		public override void Spawn()
		{
			base.Spawn();

			Tags.Add( "actor" );
			
			SetModel( "models/citizen/citizen.vmdl" );
			EyePos = Position + Vector3.Up * 64;
			CollisionGroup = CollisionGroup.Player;
			SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, Capsule.FromHeightAndRadius( 72, 8 ) );
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
		}

		public override void FrameSimulate( Client cl )
		{
			base.FrameSimulate( cl );
		}

	}
}
