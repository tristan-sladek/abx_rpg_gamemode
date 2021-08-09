using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Sandbox
{
	[Library]
	public partial class RPGTargetBar : Panel
	{
		public static RPGTargetBar Instance;
		public Label Name; 
		public Panel HPBack;
		public Panel HPFore;
		public RPGTargetBar()
		{
			Instance = this;
			StyleSheet.Load( "/ui/RPGHud.scss" );
			HPBack = Add.Panel( "HPBack" );
			HPFore = HPBack.Add.Panel( "HPFore" );
			Add.Panel( "HPBorder" ); // hacky standin
			Name = Add.Label( "TEST", "Name" );

			//Add.Label( "HP", "HPText" );
			//HP = Add.Label( "100", "HPValue" );
		}

		public override void Tick()
		{
			base.Tick();
			float size = 196; //magic number, size of element's width
			var ply = Local.Pawn as RPGPlayer;
			if ( ply == null ) return;
			var tar = ply.Target as BaseActor;
			SetClass( "hideBar", tar == null );
			if (tar != null )
			{
				Name.Text = tar.ActorName;
			}

			
			//HP.Text = Health.CeilToInt().ToString();
			//HPFore.Style.Width = Health * (size / 100);
			//HPFore.Style.Dirty();
		}
	}
}
