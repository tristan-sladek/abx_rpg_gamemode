using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace RPGGame
{
	[Library]
	public partial class RPGMana : Panel
	{
		public static RPGMana Instance;
		public Label MP;
		public Panel MPBack;
		public Panel MPFore;
		public float Mana { get; set; }
		public RPGMana()
		{
			Instance = this;
			StyleSheet.Load( "/ui/RPGHud.scss" );
			MPBack = Add.Panel( "MPBack" );
			MPFore = MPBack.Add.Panel( "MPFore" );
			Add.Panel( "MPBorder" ); // hacky standin
			Add.Label( "MP", "MPText" );
			MP = Add.Label( "100", "MPValue" );


		}

		public override void Tick()
		{
			base.Tick();
			float size = 196; //magic number, size of element's width

			var ply = Local.Pawn as RPGPlayer;
			var actor = ply.Actor;
			if ( actor == null ) return;

			Mana = actor.MP;
			//Mana = (Time.Now % 100);
			MP.Text = Mana.CeilToInt().ToString();
			MPFore.Style.Width = Mana * (size / 100);
			MPFore.Style.Dirty();
		}
	}
}
