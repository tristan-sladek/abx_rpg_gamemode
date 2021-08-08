using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;

[Library]
public partial class CameraControlPanel : Panel
{
	public static CameraControlPanel Instance;
	public CameraControlPanel()
	{
		Instance = this;

		StyleSheet.Load( "/ui/RPGHud.scss" );		
	}

	public override void Tick()
	{
		base.Tick();
		
		SetClass( "enableCursor", !((MMOCamera)Local.Pawn.Camera).isDragging );
		//Trace.Ray( CurrentView.Position, CurrentView.Position + Input.Cursor.Direction * 10000 ).Run().EndPos;				
	}
}
