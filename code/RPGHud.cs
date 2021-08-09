using Sandbox;
using Sandbox.UI;

namespace Sandbox
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
			RootPanel.AddChild<RPGStatus>();
			RootPanel.AddChild<RPGTargetBar>();
			RootPanel.AddChild<RPGNameTag>();
		}
	}
}
