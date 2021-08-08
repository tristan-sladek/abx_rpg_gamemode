using Sandbox;
using System;
using System.Linq;

namespace RPGGame
{
	partial class RPGPlayer : Player
	{
		private bool dressed = false;
		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			base.Respawn();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			Camera = new MMOCamera();
			Animator = new StandardPlayerAnimator();
			Controller = new WalkController();

			if ( !dressed )
			{
				{
					var model = Rand.FromArray( new[]
					{
					"models/citizen_clothes/trousers/trousers.jeans.vmdl",
					"models/citizen_clothes/trousers/trousers.lab.vmdl",
					"models/citizen_clothes/trousers/trousers.police.vmdl",
					"models/citizen_clothes/trousers/trousers.smart.vmdl",
					"models/citizen_clothes/trousers/trousers.smarttan.vmdl",
					"models/citizen_clothes/trousers/trousers_tracksuitblue.vmdl",
					"models/citizen_clothes/trousers/trousers_tracksuit.vmdl",
					"models/citizen_clothes/shoes/shorts.cargo.vmdl",
				} );

					ModelEntity pants = new ModelEntity();
					pants.SetModel( model );
					pants.SetParent( this, true );
					pants.EnableShadowInFirstPerson = true;
					pants.EnableHideInFirstPerson = true;

					SetBodyGroup( "Legs", 1 );
					dressed = true;
				}
				{
					var model = "models/citizen_clothes/jacket/jacket.red.vmdl";
					ModelEntity jacket = new ModelEntity();
					jacket.SetModel( model );
					jacket.SetParent( this, true );
					jacket.EnableShadowInFirstPerson = true;
					jacket.EnableHideInFirstPerson = true;

					var propInfo = jacket.GetModel().GetPropData();
					if ( propInfo.ParentBodyGroupName != null )
					{
						SetBodyGroup( propInfo.ParentBodyGroupName, propInfo.ParentBodyGroupValue );
					}
					else
					{
						SetBodyGroup( "Chest", 0 );
					}
				}
			}
			
			/*
			

			//
			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			//
			Controller = new BasePlayerController();

			//
			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			//
			Animator = new StandardPlayerAnimator();

			//
			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			//
			Camera = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			*/


		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			//
			// If you have active children (like a weapon etc) you should call this to 
			// simulate those too.
			//
			SimulateActiveChild( cl, ActiveChild );

			//
			// If we're running serverside and Attack1 was just pressed, spawn a ragdoll
			//
			

			/*
			if ( IsServer && Input.Pressed( InputButton.Attack1 ) )
			{
				var ragdoll = new ModelEntity();
				ragdoll.SetModel( "models/citizen/citizen.vmdl" );  
				ragdoll.Position = EyePos + EyeRot.Forward * 40;
				ragdoll.Rotation = Rotation.LookAt( Vector3.Random.Normal );
				ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
				ragdoll.PhysicsGroup.Velocity = EyeRot.Forward * 1000;
			}
			*/
		}

		public override void OnKilled()
		{
			base.OnKilled();

			//EnableDrawing = false;
		}
		[Event.Frame]
		protected void OnFrame()
		{
			var pawn = Local.Pawn;
			if ( pawn == null ) return;

			RenderAlpha = (((Position + Vector3.Up * 64).Distance( Input.Position ) - 5.0f) / 50.0f).Clamp( 0, 1 );
			foreach( ModelEntity ent in pawn.Children){
				ent.RenderAlpha = RenderAlpha;
			}
		}

		public override void BuildInput(InputBuilder input)
		{
			if ( input.Released( InputButton.Attack1 ) && !((MMOCamera)Camera).DraggingPerformed )
			{
				var ent = new Prop
				{
					Position = Trace.Ray( CurrentView.Position, CurrentView.Position + ((MMOCamera)Camera).ClickStart * 10000 ).Run().EndPos
				};

				ent.SetModel( "models/citizen_props/crate01.vmdl" );

			}
		}

	}
}
