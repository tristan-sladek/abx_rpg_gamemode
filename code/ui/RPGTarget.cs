using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Sandbox
{
	[Library]
	public partial class RPGTarget : Panel
	{
		public static RPGTarget Instance;
		public Label HP;
		public Panel HPBack;
		public Panel HPFore;
		public float Health { get; set; }
		public RPGTarget()
		{
			Instance = this;
			StyleSheet.Load( "/ui/RPGHud.scss" );
			HPBack = Add.Panel( "HPBack" );
			HPFore = HPBack.Add.Panel( "HPFore" );
			Add.Panel( "HPBorder" ); // hacky standin
			Add.Label( "HP", "HPText" );
			HP = Add.Label( "100", "HPValue" );
		}

		public override void Tick()
		{
			base.Tick();
			float size = 196; //magic number, size of element's width
			var ply = Local.Pawn as RPGPlayer;
			if ( ply == null ) return;
			var actor = ply.Actor;

			Health = actor.HP;
			//Health = ((Time.Now * 2) % 100);
			HP.Text = Health.CeilToInt().ToString();
			HPFore.Style.Width = Health * (size / 100);
			HPFore.Style.Dirty();
		}
	}
}
