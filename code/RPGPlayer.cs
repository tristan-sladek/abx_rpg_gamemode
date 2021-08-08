using Sandbox;
using System;
using System.Linq;

namespace RPGGame
{
	partial class RPGPlayer : Player
	{
		[Net]
		public BaseActor Actor { get; set; }
		
		private bool dressed = false;

		public override void Spawn()
		{
			base.Spawn();			
		}
		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );
			
			base.Respawn();

			if ( Actor == null )
			{
				Actor = new BaseActor();
				Actor.ActorName = "PLAYER";
				Actor.HP = 100;
				Actor.MP = 50;
			}

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			Camera = new MMOCamera();
			Animator = new RPGAnimator();
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
			SimulateActiveChild( cl, ActiveChild );

				
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

		[ServerCmd]
		public static void SpawnProp(Vector3 pos)
		{
			var ent = new BaseRPGPawn
			{
				Position = pos
			};
			/*
			var ent = new Prop
			{
				Position = pos
			};

			ent.SetModel( "models/citizen_props/crate01.vmdl" );
			*/
		}
		[ServerCmd]
		public static void DeleteEnt(Vector3 start, Vector3 end )
		{
			var tr = Trace.Ray(start,end).EntitiesOnly().Run();
			if ( tr.Entity != null && !(tr.Entity is Player) )
			{
				tr.Entity.Delete();
			}
		}

		public override void BuildInput(InputBuilder input)
		{
			var cam = Camera as MMOCamera;
			var anim = Animator as RPGAnimator;

			if ( input.Released( InputButton.Menu ) )
			{
				SpawnProp( Trace.Ray( CurrentView.Position, CurrentView.Position + Input.Cursor.Direction * 10000 ).Run().EndPos );
			}
			if ( input.Released( InputButton.Attack1 ) && !cam.DraggingPerformed )
			{
				//DeleteEnt(  );
				var tr = Trace.Ray( CurrentView.Position, CurrentView.Position + cam.ClickStart * 10000 ).EntitiesOnly().Run();
				var actor = tr.Entity;// as BaseActor;
				Log.Info( tr.Entity );
				if ( actor != null )
				{
					anim.LookAtEntity =  tr.Entity;
				}
				else
				{
					anim.LookAtEntity = null;
				}				
			}
		}

	}
}
