using Sandbox;
using Sandbox.UI;

namespace RPGGame
{
	[Library]
	public partial class RPGHud : HudEntity<RootPanel>
	{
		public RPGHud()
		{
			if ( !IsClient )
				return;

			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<CameraControlPanel>();
			RootPanel.AddChild<RPGHealth>();
			RootPanel.AddChild<RPGMana>();
		}
	}
}
