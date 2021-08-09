using Sandbox;
using Sandbox.UI;
using System;

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

			string[] names = { "Terry", "Larry", "Jerry" };
			var ind = new Random().Next( names.Length );
			ActorName = names[ind];
			HP = new Random().Next(1,100);
			MP = 20;

			var model = new[]
			{
				"models/citizen_clothes/hat/hat_hardhat.vmdl",
				"models/citizen_clothes/hat/hat_woolly.vmdl",
				"models/citizen_clothes/hat/hat_securityhelmet.vmdl"
			}[ind];

			var hat = new ModelEntity();
			hat.SetModel( model );
			hat.SetParent( this, true );
			hat.EnableShadowInFirstPerson = true;
			hat.EnableHideInFirstPerson = true;
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
