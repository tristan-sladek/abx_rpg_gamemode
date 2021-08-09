using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Sandbox
{
	[Library]
	public partial class RPGStatus : Panel
	{
		public static RPGStatus Instance;
		public Label HP;
		public Panel HPBack;
		public Panel HPFore;

		public Label MP;
		public Panel MPBack;
		public Panel MPFore;

		public RPGStatus()
		{
			Instance = this;
			StyleSheet.Load( "/ui/RPGHud.scss" );

			var HPBlock = Add.Panel( "HPBlock" );
			var MPBlock = Add.Panel( "MPBlock" );

			HPBack = HPBlock.Add.Panel( "HPBack" );
			HPFore = HPBack.Add.Panel( "HPFore" );
			HPBlock.Add.Panel( "HPBorder" ); // hacky standin
			HPBlock.Add.Label( "HP", "HPText" );
			HP = HPBlock.Add.Label( "100", "HPValue" );

			MPBack = MPBlock.Add.Panel( "MPBack" );
			MPFore = MPBack.Add.Panel( "MPFore" );
			MPBlock.Add.Panel( "MPBorder" ); // hacky standin
			MPBlock.Add.Label( "MP", "MPText" );
			MP = MPBlock.Add.Label( "100", "MPValue" );
		}

		public override void Tick()
		{
			base.Tick();
			float size = 196; //magic number, size of element's width
			var ply = Local.Pawn as RPGPlayer;
			var actor = ply.Actor;
			if ( actor == null ) return;

			//TODO: change when you figure out how to do % based style updates
			HP.Text = actor.HP.CeilToInt().ToString();
			HPFore.Style.Width = actor.HP * (size / 100);
			HPFore.Style.Dirty();

			MP.Text = actor.MP.CeilToInt().ToString();
			MPFore.Style.Width = actor.MP * (size / 100);
			MPFore.Style.Dirty();
		}
	}
}
