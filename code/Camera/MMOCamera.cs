
namespace Sandbox
{
	public class MMOCamera : Camera
	{
		[ConVar.Replicated]
		public static float abx_zoom_speed { get; set; } = 20.0f;

		private Angles orbitAngles;
		private float orbitDistance = 150;
		private float orbitDistanceTarget = 150;
		private float orbitYawTarget;
		private bool smoothYawReturn = false;
		private bool mouse_left = false;
		private bool mouse_right = false;
		public Vector3 ClickStart { get; set; }
		private Angles ViewStart { get; set; }
		private bool isDragging = false;
		public bool DraggingPerformed { get; set; }
		
		public override void Update()
		{
			var pawn = Local.Pawn as AnimEntity;
			if ( pawn == null ) return;
			Pos = pawn.Position + Vector3.Up * 64;

			//look follow mouse vs return to back
			if ( mouse_left || mouse_right )
				Rot = Rotation.From( orbitAngles );

			if ( smoothYawReturn )
			{
				orbitAngles.yaw = Rotation.Slerp( Rotation.FromYaw( orbitAngles.yaw ), Rotation.FromYaw( orbitYawTarget ), Time.Delta * 4.0f ).Yaw();
				Rot = Rotation.From( orbitAngles );
			}

			//Mouse Scrolling
			orbitDistanceTarget = orbitDistanceTarget.LerpTo( orbitDistance, Time.Delta * 5.0f );
			var targetPos = Pos + Rot.Backward * orbitDistanceTarget;
			var tr = Trace.Ray( Pos, targetPos )
				.Ignore( pawn )
				.Radius( 8 )
				.Run();
			Pos = tr.EndPos;


			FieldOfView = 75;
			Viewer = null;
		}

		public override void BuildInput( InputBuilder input )
		{
			orbitDistance -= input.MouseWheel * abx_zoom_speed;
			orbitDistance = orbitDistance.Clamp( 0, 1000 );

			mouse_left = input.Down( InputButton.Attack1 );
			mouse_right = input.Down( InputButton.Attack2 );

			if ( mouse_left && !isDragging )
			{
				ClickStart = input.Cursor.Direction; //Trace.Ray( CurrentView.Position, CurrentView.Position + Input.Cursor.Direction * 10000 ).Run().EndPos;
				ViewStart = orbitAngles;
				isDragging = true;
			}
			if ( isDragging && ViewStart.Direction.Distance(orbitAngles.Direction) > 0.01f)
			{
				DraggingPerformed = true;
			}
			if(!isDragging)
				DraggingPerformed = false;

			if ( input.Released( InputButton.Attack1 ) )
			{
				isDragging = false;
			}
				
			

			// Camera Rotation on Mouse L or R 
			if ( mouse_left || mouse_right )
			{
				orbitAngles.yaw += input.AnalogLook.yaw;
				orbitAngles.pitch += input.AnalogLook.pitch;
				orbitAngles = orbitAngles.Normal;
				orbitAngles.pitch = orbitAngles.pitch.Clamp( -89, 89 );
				smoothYawReturn = false;
			}
			else
			{
				//Rotate Camera to original position when moving
				if ( !input.AnalogMove.IsNearlyZero() )
				{
					orbitYawTarget = input.ViewAngles.Normal.yaw;
				}
				smoothYawReturn = !input.AnalogMove.IsNearlyZero();
			}

			//Character movement based upon mouse
			if ( !mouse_right )
				input.AnalogLook = Angles.Zero;
			else
				input.ViewAngles = orbitAngles;

			//Rotate Camera to original position when moving
			
			base.BuildInput( input );
		}
	}
}
