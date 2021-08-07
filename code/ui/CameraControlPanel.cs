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

		var body = Add.Panel("body");

	}

	public override void Tick()
	{
		base.Tick();
		
		SetClass( "enableCursor", !(Input.Down( InputButton.Attack1 ) || Input.Down( InputButton.Attack2 )) );
	}
}
