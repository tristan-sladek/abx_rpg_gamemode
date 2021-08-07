using Sandbox;
using Sandbox.UI;

[Library]
public partial class RPGHud : HudEntity<RootPanel>
{
	public RPGHud()
	{
		if ( !IsClient )
			return;

		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<CameraControlPanel>();
	}
}
